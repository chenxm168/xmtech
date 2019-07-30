
namespace EQPIO.MNetProtocol
{
    using System;

    public class DisconnectedEventArgs : EventArgs
    {
        private object objUnit;

        public DisconnectedEventArgs(object unit)
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
