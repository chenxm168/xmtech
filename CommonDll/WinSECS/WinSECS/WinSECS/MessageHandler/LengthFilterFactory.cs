using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinSECS.structure;

namespace WinSECS.MessageHandler
{
    [ComVisible(false)]
    public class LengthFilterFactory
    {
        private Dictionary<string, LengthFilterInfo> lengthLists = new Dictionary<string, LengthFilterInfo>();

        public void add(string SxFy, int length, bool isUserDefined)
        {
            LengthFilterInfo info = null;
            if (this.lengthLists.ContainsKey(SxFy))
            {
                info = this.lengthLists[SxFy];
                if (isUserDefined)
                {
                    info.Length = length;
                    info.IsUserDefined = true;
                }
                else if (!info.IsUserDefined && (length > info.Length))
                {
                    info.Length = length;
                }
            }
            else
            {
                info = new LengthFilterInfo
                {
                    Length = length
                };
                if (isUserDefined)
                {
                    info.IsUserDefined = true;
                }
                else
                {
                    info.IsUserDefined = false;
                }
                this.lengthLists.Add(SxFy, info);
            }
        }

        public LengthFilterInfo getMaxLength(string SxFy)
        {
            if (this.lengthLists.ContainsKey(SxFy))
            {
                return this.lengthLists[SxFy];
            }
            return null;
        }
    }
}
