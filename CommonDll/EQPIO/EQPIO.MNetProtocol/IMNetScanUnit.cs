
namespace EQPIO.MNetProtocol
{
    using EQPIO.Common;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class IMNetScanUnit
    {
        public event EventHandler<ConnectedEventArgs> Connected;

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        public event EventHandler<FDCScanReceivedEventArgs> FDCScanReceived;

        public event EventHandler<LinkSignalScanReceivedEventArgs> LinkSignalScanReceived;

        public event EventHandler<FDCScanReceivedEventArgs> RGAScanReceived;

        public event EventHandler<ScanReceivedEventArgs> ScanReceived;

        public event EventHandler<ScanReceivedEventArgs> VirtaulEQPScanReceived;

        protected IMNetScanUnit()
        {
        }

        public abstract void CacheThreadStart();
        public abstract void CacheThreadStop();
        public abstract void Close();
        public abstract void Init();
        public abstract void ListUpCacheBlock();
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

        protected virtual void OnFDCScanReceived(FDCScanReceivedEventArgs e)
        {
            EventHandler<FDCScanReceivedEventArgs> fDCScanReceived = this.FDCScanReceived;
            if (fDCScanReceived != null)
            {
                fDCScanReceived(this, e);
            }
        }

        protected virtual void OnLinkSignalScanReceived(LinkSignalScanReceivedEventArgs e)
        {
            EventHandler<LinkSignalScanReceivedEventArgs> linkSignalScanReceived = this.LinkSignalScanReceived;
            if (linkSignalScanReceived != null)
            {
                linkSignalScanReceived(this, e);
            }
        }

        protected virtual void OnRGAScanReceived(FDCScanReceivedEventArgs e)
        {
            EventHandler<FDCScanReceivedEventArgs> rGAScanReceived = this.RGAScanReceived;
            if (rGAScanReceived != null)
            {
                rGAScanReceived(this, e);
            }
        }

        protected virtual void OnScanReceived(ScanReceivedEventArgs e)
        {
            EventHandler<ScanReceivedEventArgs> scanReceived = this.ScanReceived;
            if (scanReceived != null)
            {
                scanReceived(this, e);
            }
        }

        protected virtual void OnVirtaulEQPScanReceived(ScanReceivedEventArgs e)
        {
            EventHandler<ScanReceivedEventArgs> virtaulEQPScanReceived = this.VirtaulEQPScanReceived;
            if (virtaulEQPScanReceived != null)
            {
                virtaulEQPScanReceived(this, e);
            }
        }

        public abstract string ReadCacheData(string deviceCode, string address, string points);
        public abstract void StartReconnect();
        public abstract void StopReconnect();
        public abstract void ThreadClose();
        public abstract void ThreadStart();
        public abstract void ThreadStop();
        public abstract void WriteCacheData(string deviceCode, string address, string points);

        public abstract EQPIO.Common.BlockMap BlockMap { get; set; }

        public abstract string ConnectionUnitName { get; set; }

        public abstract bool IsConnection { get; set; }

        public abstract string MultiBlockName { get; set; }

        public abstract Dictionary<Block, string> ScanDataCollectDictionary { get; set; }

        public enum ProtocolType
        {
            None,
            WriteIntWord,
            WriteASCIIWord,
            WriteBitProtocol,
            ReadBinaryWord,
            ReadBitWord,
            ReadIntWord,
            ReadIntWord2,
            ReadASCIIWord,
            ReadBitProtocol,
            Word,
            Bit
        }

        public enum ReceiveStatus
        {
            Read,
            Write
        }
    }
}
