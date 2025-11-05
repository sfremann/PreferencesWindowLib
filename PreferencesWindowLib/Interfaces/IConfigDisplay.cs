// Copyright (c) 2025 Sarah Frémann.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Windows;

namespace PreferencesWindowLib.Interfaces
{
    public interface IConfigDisplay
    {
        bool PropertyHasCustomEditor(string propertyName);
        UIElement GetCustomEditorForProperty(string propertyName);
        string? GetPropertyLabel(string propertyName);
        string? GetPropertyTooltip(string propertyName);
        string? GetSectionHeader();
    }
}
