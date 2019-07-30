
namespace EQPIO.Common
{
    using System;
    using System.Collections.Generic;

    public class ScanStatusEachConnectionType
    {
        private Dictionary<string, Dictionary<MultiBlock, bool>> m_dicMultiBlockOnOff;

        public ScanStatusEachConnectionType()
        {
            this.m_dicMultiBlockOnOff = new Dictionary<string, Dictionary<MultiBlock, bool>>();
        }

        public ScanStatusEachConnectionType(string localName, MultiBlock multiblock, bool on)
        {
            this.m_dicMultiBlockOnOff = new Dictionary<string, Dictionary<MultiBlock, bool>>();
            Dictionary<MultiBlock, bool> dictionary = new Dictionary<MultiBlock, bool>();
            dictionary.Add(multiblock, on);
            this.m_dicMultiBlockOnOff.Add(localName, dictionary);
        }

        public Dictionary<string, Dictionary<MultiBlock, bool>> MultiBlockOnOff
        {
            get
            {
                return this.m_dicMultiBlockOnOff;
            }
        }
    }
}
