using System.Configuration;

namespace SdShare.Configuration
{
    public class SdShareReceiverConfigurationSection : ConfigurationSection
    {
        private const string ReceiverCollectionName = "Receivers";

        [ConfigurationProperty(ReceiverCollectionName)]
        [ConfigurationCollection(typeof(ReceiverTypeCollection), AddItemName = "add")]
        public ReceiverTypeCollection Receivers
        {
            get { return (ReceiverTypeCollection) base[ReceiverCollectionName]; }
        }
    }

    public class ReceiverTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReceiverTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReceiverTypeElement) element).Name;
        }
    }

    public class ReceiverTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("graph", IsRequired = false)]
        public string Graph
        {
            get { return (string)this["graph"]; }
            set { this["graph"] = value; }
        }
    }
}
