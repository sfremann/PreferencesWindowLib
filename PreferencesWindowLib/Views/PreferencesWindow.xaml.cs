// Copyright (c) 2025 Sarah Frémann.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using PreferencesWindowLib.Interfaces;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PreferencesWindowLib.Views
{
    /// <summary>
    /// Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        private readonly Configuration _appConfig;

        public PreferencesWindow(Configuration AppConfig)
        {
            InitializeComponent();

            _appConfig = AppConfig;
            DataContext = _appConfig;

            // Init config sections / tabs
            foreach (var configSection in _appConfig.Sections)
            {
                // Ignore system sections
                if (configSection != null && configSection.GetType() is Type itemType
                    && itemType.FullName != null && !itemType.FullName.StartsWith("System."))
                {
                    StackPanel stackPanel = new();
                    foreach (var property in itemType.GetProperties())
                    {
                        if (property.GetCustomAttribute<ConfigurationPropertyAttribute>() != null)
                        {
                            stackPanel.Children.Add(GetConfigEditor(property, configSection));
                        }
                    }

                    TabItem tabItem = new()
                    {
                        Header = itemType.Name,
                        DataContext = configSection,
                        Content = new ScrollViewer { Content = stackPanel }
                    };
                    if (configSection is IConfigDisplay configSectionWithDisplay)
                    {
                        tabItem.Header = configSectionWithDisplay.GetSectionHeader() ?? itemType.Name;
                    }
                    ConfigSections.Items.Add(tabItem);
                }
            }
        }

        // --- Helpers

        private static StackPanel GetConfigEditor(PropertyInfo property, object configSection)
        {
            StackPanel stackPanel = new() { Orientation = Orientation.Horizontal };
            Label label = new() { Content = $"{property.Name}: " };
            stackPanel.Children.Add(label);

            // Use custom editor if available
            if (configSection is IConfigDisplay configSectionWithDisplay)
            {
                label.ToolTip = configSectionWithDisplay.GetPropertyTooltip(property.Name);
                if (configSectionWithDisplay.GetPropertyLabel(property.Name) is string labelString)
                {
                    label.Content = labelString;
                }
                if (configSectionWithDisplay.PropertyHasCustomEditor(property.Name))
                {
                    stackPanel.Children.Add(configSectionWithDisplay.GetCustomEditorForProperty(property.Name));
                    return stackPanel;
                }
            }

            // Use defaults
            object? value = property.GetValue(configSection);

            if (value is string)
            {
                TextBox textBox = new() { DataContext = configSection };
                textBox.SetBinding(TextBox.TextProperty, new Binding(property.Name)
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
                });
                stackPanel.Children.Add(textBox);
                return stackPanel;
            }

            return stackPanel;
        }
    }
}
