
namespace EQPIO.Common
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class Item : ICloneable
    {
        public object Clone()
        {
            return new Item { Name = this.Name, Offset = this.Offset, Points = this.Points, Representation = this.Representation, Value = this.Value, SyncValue = this.SyncValue, ItemAddress = this.ItemAddress, Multiplier = this.Multiplier };
        }

        [XmlAttribute]
        public string ItemAddress { get; set; }

        [XmlAttribute]
        public string Multiplier { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Offset { get; set; }

        [XmlAttribute]
        public string Points { get; set; }

        [XmlAttribute]
        public string Representation { get; set; }

        [XmlAttribute]
        public bool SyncValue { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}
