using System;
using System.Collections.Generic;

namespace UI
{
    /// <summary>
    /// Represents a form configuration that contains a list of items, a title, and actions to perform when the configuration is saved or canceled.
    /// </summary>
    public class FormConfiguration
    {
        /// <summary>
        /// The title of the form configuration.
        /// </summary>
        public readonly string Title;

        /// <summary>
        /// The list of items in the form configuration.
        /// </summary>
        public readonly List<FormConfigurationItem> Items;

        /// <summary>
        /// The action to perform when the form configuration is saved.
        /// </summary>
        public readonly Action<Dictionary<string, string>> OnSave;

        /// <summary>
        /// The action to perform when the form configuration is canceled.
        /// </summary>
        public readonly Action OnCancel;

        /// <summary>
        /// Creates a new instance of the <see cref="FormConfiguration"/> class.
        /// </summary>
        /// <param name="title">The title of the form configuration.</param>
        /// <param name="items">The list of items in the form configuration.</param>
        /// <param name="onSave">The action to perform when the form configuration is saved.</param>
        /// <param name="onCancel">The action to perform when the form configuration is canceled.</param>
        public FormConfiguration(string title, List<FormConfigurationItem> items,
            Action<Dictionary<string, string>> onSave, Action onCancel)
        {
            Title = title;
            Items = items;
            OnSave = onSave;
            OnCancel = onCancel;
        }
    }

    /// <summary>
    /// Represents an item in a form configuration.
    /// </summary>
    public class FormConfigurationItem
    {
        /// <summary>
        /// The label of the form configuration item.
        /// </summary>
        public readonly string Label;

        /// <summary>
        /// The ID of the form configuration item.
        /// </summary>
        public readonly string ID;

        /// <summary>
        /// The default value of the form configuration item.
        /// </summary>
        public readonly string DefaultValue;

        /// <summary>
        /// Creates a new instance of the <see cref="FormConfigurationItem"/> class.
        /// </summary>
        /// <param name="label">The label of the form configuration item.</param>
        /// <param name="id">The ID of the form configuration item.</param>
        /// <param name="defaultValue">The default value of the form configuration item.</param>
        public FormConfigurationItem(string label, string id, string defaultValue)
        {
            Label = label;
            ID = id;
            DefaultValue = defaultValue;
        }
    }
}
