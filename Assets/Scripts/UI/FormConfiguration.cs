using System;
using System.Collections.Generic;

namespace UI
{
    public class FormConfiguration
    {
        public FormConfiguration(string title, List<FormConfigurationItem> items, Action<Dictionary<string, string>> onSave, Action onCancel)
        {
            Title = title;
            Items = items;
            OnSave = onSave;
            OnCancel = onCancel;
        }

        public readonly string Title;
        public readonly List<FormConfigurationItem> Items;
        public readonly Action<Dictionary<string, string>> OnSave;
        public readonly Action OnCancel;
    }

    public class FormConfigurationItem
    {
        public FormConfigurationItem(string label, string id, string defaultValue)
        {
            ID = id;
            DefaultValue = defaultValue;
            Label = label;
        }

        public readonly string Label;
        public readonly string ID;
        public readonly string DefaultValue;
    }
}