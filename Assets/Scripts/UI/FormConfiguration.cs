using System.Collections.Generic;

namespace UI
{
    public class FormConfiguration
    {
        FormConfiguration(string title, List<FormConfigurationItem> items)
        {
            Title = title;
            Items = items;
        }

        public string Title;
        public List<FormConfigurationItem> Items;
    }

    public class FormConfigurationItem
    {
        FormConfigurationItem(string label, string id, string defaultValue)
        {
            ID = id;
            DefaultValue = defaultValue;
            Label = label;
        }

        public string Label;
        public string ID;
        public string DefaultValue;
    }
}