using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WinSECS.structure
{
   public  interface IFormat:ICloneable
    {
        // Methods
        IFormat add(IFormat format);
        IFormat add(byte type, int length, string name, string value_Renamed);
        IFormat addFormat(IFormat format);
        object Clone();
        IFormat fromElement(XmlElement e);
        string getformatTypeRegex();
        int getMaxPossibleByteLength();
        string GetRegexInput();
        string GetRegexPattern();
        bool hasNext();
        void remove();
        XmlElement toElement();
        XmlElement toElement(XmlDocument doc);

        // Properties
        IFormatCollection Children { get; set; }
        int DefaultByteLength { get; }
        IFormat this[int index] { get; }
        IFormat this[string itemName] { get; }
        bool ItemKey { get; set; }
        int Length { get; set; }
        int Level { get; set; }
        string LogFormat { get; }
        string Name { get; set; }
        IFormat Next { get; set; }
        IFormatCollection Owner { get; set; }
        IFormat Parent { get; set; }
        IFormat Previous { get; set; }
        byte Type { get; }
        string Value { get; set; }
        bool Variable { get; set; }



    }
}
