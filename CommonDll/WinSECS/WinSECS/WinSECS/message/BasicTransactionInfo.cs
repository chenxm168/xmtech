using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class BasicTransactionInfo
    {
        private int stream = 0;
        private int function = 0;
        private String MessageName = "";
        private bool isWait = false;
        private bool isAutoReply = false;
        private String messageData = "";
        private Object correlation = null;

        public int Stream
        {
            get { return stream; }
            set { stream = value; }
        }

        public int Function
        {
            get { return function; }
            set { function = value; }
        }

        public String MessageName1
        {
            get { return MessageName; }
            set { MessageName = value; }
        }

        public bool IsWait
        {
            get { return isWait; }
            set { isWait = value; }
        }

        public bool IsAutoReply
        {
            get { return isAutoReply; }
            set { isAutoReply = value; }
        }

        public String MessageData
        {
            get { return messageData; }
            set { messageData = value; }
        }

        public Object Correlation
        {
            get { return correlation; }
            set { correlation = value; }
        }

        public BasicTransactionInfo(SECSTransaction trx)
        {
            this.stream = trx.Stream;
            this.function = trx.Function;
            this.MessageName = trx.MessageName;
            this.isWait = trx.Wbit;
            this.isAutoReply = trx.Autoreply;
            this.correlation = trx.Correlation;
            this.messageData = trx.MessageData;
        }

        public void dispose()
        {
        }
    }
}
