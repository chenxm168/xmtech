
namespace EQPIO.MNetProtocol
{
    using System;

    public class ConnectedEventArgs : EventArgs
    {
        private object objUnit;

        public ConnectedEventArgs(object unit)
        {
            this.objUnit = unit;
        }

        public object Unit
        {
            get
            {
                return this.objUnit;
            }
        }
    }
}
