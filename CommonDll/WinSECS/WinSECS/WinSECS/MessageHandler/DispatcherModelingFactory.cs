using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinSECS.structure;
using WinSECS.global;

namespace WinSECS.MessageHandler
{
    [ComVisible(false)]
    public class DispatcherModelingFactory
    {
        internal HashFactory modelingFacotry;
        internal HashFactory modelingFactoryWithItemKey = new HashFactory();

        public DispatcherModelingFactory()
        {
            this.InitBlock();
        }

        public virtual bool AddModelingInfo(string SxFx, string MessageName, SECSTransaction trx)
        {
            if (trx.HasItemKey)
            {
                return this.modelingFactoryWithItemKey.Add(SxFx, MessageName, trx);
            }
            return this.modelingFacotry.Add(SxFx, MessageName, trx);
        }

        public virtual bool ClearModelingInfo()
        {
            this.modelingFactoryWithItemKey.clear();
            this.modelingFacotry.clear();
            return true;
        }

        private void InitBlock()
        {
            this.modelingFacotry = new HashFactory();
        }

        public virtual int Size()
        {
            return (this.modelingFacotry.size() + this.modelingFactoryWithItemKey.size());
        }
    }
}
