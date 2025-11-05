# PreferencesWindowLib
Simple WPF library for a modular preferences window (or settings, config... window)

## Usage

The `PreferencesWindow` is a modular window displaying all custom configuration sections added to the app config. By default, it will create a tab for every section and a field for every configuration property. Fields are editable when applicable, by default only applies to strings associated with text boxes.

When a configuration section implements the `IConfigDisplay` interface, it is possible to customize the property editors, labels and tooltips.

### `bool PropertyHasCustomEditor(string propertyName)`

If `true`, the property named `propertyName` will be edited using a custom editor.

### `UIElement GetCustomEditorForProperty(string propertyName)`

In this method, you can customize the appearance and behavior of the editor for the property named `propertyName`.

### `string? GetPropertyLabel(string propertyName)`

If not `null`, will replace the default label associated with property named `propertyName`. Default label is `propertyName`.

### `string? GetPropertyTooltip(string propertyName)`

If not `null`, will add a tooltip to the editor associated with property named `propertyName`.

### `string? GetSectionHeader()`

If not `null`, will replace the default label associated with the configuration section. Default label is name of class of the configuration section.
