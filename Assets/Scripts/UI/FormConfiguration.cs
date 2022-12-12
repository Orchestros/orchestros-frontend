using System;
using System.Collections.Generic;

namespace UI
{
    public class FormConfiguration
    {
        public readonly List<FormConfigurationItem> Items;
        public readonly Action OnCancel;
        public readonly Action<Dictionary<string, string>> OnSave;

        public readonly string Title;

        public FormConfiguration(string title, List<FormConfigurationItem> items,
            Action<Dictionary<string, string>> onSave, Action onCancel)
        {
            Title = title;
            Items = items;
            OnSave = onSave;
            OnCancel = onCancel;
        }
    }

    public class FormConfigurationItem
    {
        public readonly string DefaultValue;
        public readonly string ID;

        public readonly string Label;

        public FormConfigurationItem(string label, string id, string defaultValue)
        {
            ID = id;
            DefaultValue = defaultValue;
            Label = label;
        }
    }
}