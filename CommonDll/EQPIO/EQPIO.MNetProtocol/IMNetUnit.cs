
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class IMNetUnit
    {
        public event EventHandler<ConnectedEventArgs> Connected;

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        protected IMNetUnit()
        {
        }

        public abstract void Close();
        public abstract void Init();
        protected virtual void OnConnected(ConnectedEventArgs e)
        {
            EventHandler<ConnectedEventArgs> connected = this.Connected;
            if (connected != null)
            {
                connected(this, e);
            }
        }

        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
            EventHandler<DisconnectedEventArgs> disconnected = this.Disconnected;
            if (disconnected != null)
            {
                disconnected(this, e);
            }
        }

        public abstract Dictionary<string, string> ReadBit(Block block, bool isHex);
        public abstract Dictionary<string, string> ReadRBit(Block block, bool isHex, int networkNo, int pcNo);
        public abstract Dictionary<string, string> ReadRWord(Block block, bool isHex, int networkNo, int pcNo);
        public abstract Dictionary<string, string> ReadWord(Block block, bool isHex);
        public abstract Dictionary<string, string> ReadWordOnce(Block block, bool isHex);
        public abstract void ReConnect();
        public abstract void StopConnect();
        public abstract void ThreadClose();
        public abstract void ThreadStart();
        public abstract void ThreadStop();
        public abstract void TimeoutCheckStart();
        public abstract void TimeoutCheckStop();
        public abstract bool WriteBit(Block block, Dictionary<string, string> data);
        public abstract bool WriteRBit(Block block, Dictionary<string, string> data, int networkNo, int pcNo);
        public abstract bool WriteRWord(Block block, Dictionary<string, string> data, int networkNo, int pcNo);
        public abstract bool WriteWord(Block block, Dictionary<string, string> data);

        public abstract string Name { get; set; }
    }
}
