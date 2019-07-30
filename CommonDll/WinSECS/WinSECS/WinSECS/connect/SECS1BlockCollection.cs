using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.CompilerServices;
using WinSECS.structure;
using WinSECS.Utility;

namespace WinSECS.connect
{
    internal class SECS1BlockCollection
    {
        private SortedDictionary<int, SECS1Block> blockCollection = new SortedDictionary<int, SECS1Block>();
        private string key;
        private int lastBlockNumber = 0;
        private int t4;
        private Timer t4Timer = null;

        public event T4ElapsedEventHandler OnT4Elapsed;

        internal SECS1BlockCollection(string key, int t4)
        {
            this.key = key;
            this.t4 = t4;
        }

        internal void AddMessage(SECS1Block msg)
        {
            if (msg.IsLastBlock)
            {
                this.lastBlockNumber = msg.BlockNumber;
            }
            if (!this.blockCollection.ContainsKey(msg.BlockNumber))
            {
                this.blockCollection.Add(msg.BlockNumber, msg);
            }
            this.StopT4Timer();
            if (!this.IsCompleted)
            {
                this.StartT4Timer();
            }
        }

        internal byte[] MakeHSMSHeader()
        {
            foreach (SECS1Block block in this.blockCollection.Values)
            {
                byte[] destinationArray = new byte[10];
                Array.Copy(block.Header, destinationArray, 10);
                destinationArray[0] = (byte)(destinationArray[0] & 0x7f);
                destinationArray[4] = 0;
                destinationArray[5] = 0;
                return destinationArray;
            }
            return null;
        }

        private void OnT4Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.OnT4Elapsed != null)
            {
                this.OnT4Elapsed(this);
            }
        }

        private void StartT4Timer()
        {
            this.t4Timer = new Timer((double)this.t4);
            this.t4Timer.AutoReset = false;
            this.t4Timer.Elapsed += new ElapsedEventHandler(this.OnT4Timer_Elapsed);
            this.t4Timer.Start();
        }

        private void StopT4Timer()
        {
            if (this.t4Timer != null)
            {
                this.t4Timer.Stop();
                this.t4Timer.Elapsed -= new ElapsedEventHandler(this.OnT4Timer_Elapsed);
                this.t4Timer = null;
            }
        }

        internal SECSTransaction ToSECSMessage()
        {
            ByteStream stream = new ByteStream();
            foreach (SECS1Block block in this.blockCollection.Values)
            {
                stream.Write(block.Text);
            }
            return new SECSTransaction { Header = this.MakeHSMSHeader(), Body = stream.ToArray(), Receive = true };
        }

        internal bool IsCompleted
        {
            get
            {
                return ((this.lastBlockNumber > 0) && (this.lastBlockNumber == this.blockCollection.Count));
            }
        }

        internal string Key
        {
            get
            {
                return this.key;
            }
        }
    }
}
