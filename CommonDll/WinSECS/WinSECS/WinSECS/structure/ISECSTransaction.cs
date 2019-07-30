using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WinSECS.structure
{
    public interface ISECSTransaction
    {
        IFormat add(byte type, int length, string name, string value_Renamed);
        IFormat addFormat(IFormat format);
        object Clone();
        bool equals(SECSTransaction comparatee);
        IFormat fromElement(XmlElement element);
        IFormat getByIndex(string indexString, string delimeter);
        IFormat getByName(string itemname);
        void setStreamNWbit(int stream, bool wbit);

        bool Autoreply { get; set; }

        byte[] Body { get; set; }

        IFormatCollection Children { get; set; }

        bool ControlMessage { get; }

        object Correlation { get; set; }

        string Description { get; set; }

        int DeviceId { get; set; }

        string Direction { get; }

        int Function { get; set; }

        byte[] Header { get; set; }

        string HeaderString { get; set; }

        byte[] LengthBytes { get; }

        string MessageData { get; set; }

        string MessageName { get; set; }

        string PairName { get; set; }

        bool Receive { get; set; }

        bool Secondary { get; }

        string SECS1BodyString { get; set; }

        string SECS1HeaderLoggingString { get; set; }

        string SECS2BodyString { get; set; }

        string SECS2HeaderLoggingString { get; set; }

        int Stream { get; }

        long Systembyte { get; set; }

        bool Wbit { get; }
    }
}
