
namespace EQPIO.RabbitMQInterface
{
    using EQPIO.RabbitMQInterface.Parser;
    using EQPIO.RabbitMQInterface.Parser.Impl;
    using log4net;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class MQInterface
    {
        private IModel _consumerChannel;
        private IConnection _consumerConnection;
        private string _consumerExchange;
        private string _consumerQueue;
        private string[] _consumerRoutingKeys;
        private Thread _consumerThread;
        private string _expiration;
        private string _hostName;
        private bool _isAutoDeleteQueue;
        private volatile bool _isOpen;
        private IMessageParser _messageParser;
        private IModel _producerChannel;
        private IConnection _producerConnection;
        private string _producerExchange;
        private string _producerRoutingKey;
        private ConnectionFactory factory;
        private ILog logger;
        private bool m_bRemoteConnection;
        private bool m_bRunConsumer;
        private string m_strPassword;
        private string m_strUserid;

        public event MessageReceivedEventHandler OnMessageReceived;

        public MQInterface(string hostName, string producerExchange, string producerRoutingKey, string consumerExchange, string consumerRoutingKey, string consumerQueue)
            : this(hostName, producerExchange, producerRoutingKey, consumerExchange, new string[] { consumerRoutingKey }, consumerQueue)
        {
        }

        public MQInterface(string hostName, string producerExchange, string producerRoutingKey, string consumerExchange, string[] consumerRoutingKeys, string consumerQueue)
            : this(hostName, producerExchange, producerRoutingKey, consumerExchange, consumerRoutingKeys, consumerQueue, new DefaultMessageParser())
        {
        }

        public MQInterface(string hostName, string producerExchange, string producerRoutingKey, string consumerExchange, string consumerRoutingKey, string consumerQueue, IMessageParser messageParser)
            : this(hostName, producerExchange, producerRoutingKey, consumerExchange, new string[] { consumerRoutingKey }, consumerQueue, messageParser)
        {
        }

        public MQInterface(string hostName, string producerExchange, string producerRoutingKey, string consumerExchange, string[] consumerRoutingKeys, string consumerQueue, IMessageParser messageParser)
        {
            this.logger = LogManager.GetLogger(typeof(MQInterface));
            this.factory = null;
            this.m_bRunConsumer = false;
            this._isOpen = false;
            this._isAutoDeleteQueue = false;
            this._expiration = string.Empty;
            this.m_bRemoteConnection = false;
            this.m_strUserid = string.Empty;
            this.m_strPassword = string.Empty;
            this._hostName = hostName;
            this._producerExchange = producerExchange;
            this._producerRoutingKey = producerRoutingKey;
            this._consumerExchange = consumerExchange;
            this._consumerRoutingKeys = consumerRoutingKeys;
            this._consumerQueue = consumerQueue;
            this._messageParser = messageParser;
        }

        public void Close()
        {
            try
            {
                this.m_bRunConsumer = false;
                this._isOpen = false;
                if (this._consumerThread != null)
                {
                    this._consumerThread.Abort();
                    this._consumerThread = null;
                    this.logger.Info("Success to close consumer thread.");
                }
                if (this._consumerChannel != null)
                {
                    this._consumerChannel.Close();
                    this._consumerChannel.Dispose();
                    this._consumerChannel = null;
                    this.logger.Info("Success to close consumer channel.");
                }
                if (this._consumerConnection != null)
                {
                    this._consumerConnection.Close();
                    this._consumerConnection.Dispose();
                    this._consumerConnection = null;
                    this.logger.Info("Success to close consumer connection.");
                }
                if (this._producerChannel != null)
                {
                    this._producerChannel.Close();
                    this._producerChannel.Dispose();
                    this._producerChannel = null;
                    this.logger.Info("Success to close producer channel.");
                }
                if (this._producerConnection != null)
                {
                    this._producerConnection.Close();
                    this._producerConnection.Dispose();
                    this._producerConnection = null;
                    this.logger.Info("Success to close producer connection.");
                }
                this.logger.Info("Success to close MQInterface.");
            }
            catch (Exception exception)
            {
                this.logger.Error("Failed to close MQInterface.");
                this.logger.Error(exception);
            }
        }

        private IConnection CreateConnection()
        {
            lock (this)
            {
                if (this.factory == null)
                {
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        HostName = this._hostName
                    };
                    this.factory = factory;
                }
            }
            return this.factory.CreateConnection();
        }

        private IConnection CreateConnection(string userid, string password)
        {
            lock (this)
            {
                if (this.factory == null)
                {
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        HostName = this._hostName
                    };
                    this.factory = factory;
                }
            }
            this.factory.UserName = "eosuser";
            this.factory.Password = "eosuser";
            return this.factory.CreateConnection();
        }

        private void CreateConsumer()
        {
            try
            {
                this._consumerConnection = this.CreateConnection();
                this._consumerChannel = this._consumerConnection.CreateModel();
                this._consumerChannel.ExchangeDeclare(this._consumerExchange, "direct");
                this._consumerChannel.QueueDeclare(this._consumerQueue, true, false, this._isAutoDeleteQueue, null);
                foreach (string str in this._consumerRoutingKeys)
                {
                    this._consumerChannel.QueueBind(this._consumerQueue, this._consumerExchange, str);
                }
                this._consumerChannel.BasicQos(0, 1, false);
                this.m_bRunConsumer = true;
                this._consumerThread = new Thread(new ThreadStart(this.RunConsumer));
                this._consumerThread.IsBackground = true;
                this._consumerThread.Start();
                this.logger.Info("Success to create Consumer.");
                StringBuilder builder = new StringBuilder();
                foreach (string str in this._consumerRoutingKeys)
                {
                    builder.Append(string.Format("{0},", str));
                }
                this.logger.Info(string.Format("Consumer: exchange[{0}] routingkey[{1}] Queue[{2}]", this._consumerExchange, builder.ToString(0, builder.Length - 1), this._consumerQueue));
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                if (exception.Message.Contains("text=\"PRECONDITION_FAILED"))
                {
                    this._consumerConnection = this.CreateConnection();
                    this._consumerChannel = this._consumerConnection.CreateModel();
                    this._consumerChannel.ExchangeDeclare(this._consumerExchange, "direct");
                    if (this._consumerChannel.QueueDelete(this._consumerQueue) == 0)
                    {
                        this.CreateConsumer();
                    }
                }
            }
        }

        private void CreateConsumer(string userid, string password)
        {
            try
            {
                this._consumerConnection = this.CreateConnection(userid, password);
                this._consumerChannel = this._consumerConnection.CreateModel();
                this._consumerChannel.ExchangeDeclare(this._consumerExchange, "direct");
                this._consumerChannel.QueueDeclare(this._consumerQueue, true, false, this._isAutoDeleteQueue, null);
                foreach (string str in this._consumerRoutingKeys)
                {
                    this._consumerChannel.QueueBind(this._consumerQueue, this._consumerExchange, str);
                }
                this._consumerChannel.BasicQos(0, 1, false);
                this.m_bRunConsumer = true;
                this._consumerThread = new Thread(new ThreadStart(this.RunConsumer));
                this._consumerThread.IsBackground = true;
                this._consumerThread.Start();
                this.logger.Info("Success to create Consumer.");
                StringBuilder builder = new StringBuilder();
                foreach (string str in this._consumerRoutingKeys)
                {
                    builder.Append(string.Format("{0},", str));
                }
                this.logger.Info(string.Format("Consumer: exchange[{0}] routingkey[{1}] Queue[{2}]", this._consumerExchange, builder.ToString(0, builder.Length - 1), this._consumerQueue));
            }
            catch (Exception exception)
            {
                this.logger.Error(exception);
                if (exception.Message.Contains("text=\"PRECONDITION_FAILED"))
                {
                    this._consumerConnection = this.CreateConnection(userid, password);
                    this._consumerChannel = this._consumerConnection.CreateModel();
                    this._consumerChannel.ExchangeDeclare(this._consumerExchange, "direct");
                    if (this._consumerChannel.QueueDelete(this._consumerQueue) == 0)
                    {
                        this.CreateConsumer();
                    }
                }
            }
        }

        private void CreateProducer()
        {
            this._producerConnection = this.CreateConnection();
            this._producerChannel = this._producerConnection.CreateModel();
            this._producerChannel.ExchangeDeclare(this._producerExchange, "direct");
            this.logger.Info("Success to create producer.");
            this.logger.Info(string.Format("HostName: {0}", this._hostName));
            this.logger.Info(string.Format("Producer: exchange[{0}] routingkey[{1}]", this._producerExchange, this._producerRoutingKey));
        }

        private void CreateProducer(string userid, string password)
        {
            this._producerConnection = this.CreateConnection();
            this._producerChannel = this._producerConnection.CreateModel();
            this._producerChannel.ExchangeDeclare(this._producerExchange, "direct");
            this.logger.Info("Success to create producer.");
            this.logger.Info(string.Format("HostName: {0}", this._hostName));
            this.logger.Info(string.Format("Producer: exchange[{0}] routingkey[{1}]", this._producerExchange, this._producerRoutingKey));
        }

        public bool Open()
        {
            Exception exception;
            try
            {
                this.Close();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this.logger.Error(exception);
            }
            try
            {
                this.CreateProducer();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                this.logger.Error("Failed to create producer.");
                this.logger.Error(exception);
                return false;
            }
            try
            {
                this.CreateConsumer();
            }
            catch (Exception exception3)
            {
                exception = exception3;
                this.logger.Error("Failed to create consumer.");
                this.logger.Error(exception);
                return false;
            }
            this._isOpen = true;
            this.logger.Info("Success to create MQInterface.");
            return true;
        }

        public bool Open(string userid, string password)
        {
            Exception exception;
            try
            {
                this.Close();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this.logger.Error(exception);
            }
            try
            {
                this.CreateProducer(userid, password);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                this.logger.Error("Failed to create producer.");
                this.logger.Error(exception);
                return false;
            }
            try
            {
                this.CreateConsumer(userid, password);
            }
            catch (Exception exception3)
            {
                exception = exception3;
                this.logger.Error("Failed to create consumer.");
                this.logger.Error(exception);
                return false;
            }
            this._isOpen = true;
            this.m_bRemoteConnection = true;
            this.m_strUserid = userid;
            this.m_strPassword = password;
            this.logger.Info("Success to create MQInterface.");
            return true;
        }

        private void RecoveryConsumer(object argument)
        {
            Exception exception;
            try
            {
                if (this._consumerChannel != null)
                {
                    this._consumerChannel.Close();
                    this._consumerChannel.Dispose();
                    this._consumerChannel = null;
                    this.logger.Info("Success to close consumer channel.");
                }
                if (this._consumerConnection != null)
                {
                    this._consumerConnection = null;
                    this.logger.Info("Success to close consumer connection.");
                }
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this.logger.Error("Failed to close Consumer.");
                this.logger.Error(exception);
            }
            try
            {
                if (!this.m_bRemoteConnection)
                {
                    this.CreateConsumer();
                }
                else
                {
                    this.CreateConsumer(this.m_strUserid, this.m_strPassword);
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                this.logger.Error("Failed to create consumer.");
                this.logger.Error(exception);
            }
        }

        private void RecoveryProducer(object message, string targetRoutingkey)
        {
            lock (this._producerChannel)
            {
                Exception exception;
                try
                {
                    if (this._producerChannel != null)
                    {
                        this._producerChannel.Close();
                        this._producerChannel.Dispose();
                        this._producerChannel = null;
                        this.logger.Info("Success to close producer channel.");
                    }
                    if (this._producerConnection != null)
                    {
                        this._producerConnection = null;
                        this.logger.Info("Success to close producer connection.");
                    }
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    this.logger.Error("Failed to Close producer");
                    this.logger.Error(exception);
                }
                try
                {
                    if (!this.m_bRemoteConnection)
                    {
                        this.CreateProducer();
                    }
                    else
                    {
                        this.CreateProducer(this.m_strUserid, this.m_strPassword);
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    this.logger.Error("Failed to create producer.");
                    this.logger.Error(exception);
                }
                this.Send(message, targetRoutingkey);
            }
        }

        private void RunConsumer()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(this._consumerChannel);
            this._consumerChannel.BasicConsume(this._consumerQueue, false, consumer);
            BasicDeliverEventArgs args = null;
            while (this.m_bRunConsumer)
            {
                try
                {
                    args = consumer.Queue.Dequeue();
                    this.logger.Info(string.Format("{0} - Recv: {1}byte", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), args.Body.Length));
                    object message = this._messageParser.ByteArrayToObject(args.Body);
                    if ((message != null) && (this.OnMessageReceived != null))
                    {
                        this.OnMessageReceived(this, message);
                    }
                }
                catch (Exception exception)
                {
                    this.logger.Error("Error Consumer");
                    this.logger.Error(exception);
                    if (this._isOpen)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.RecoveryConsumer));
                    }
                    break;
                }
                finally
                {
                    if ((args != null) && (this._consumerChannel != null))
                    {
                        this._consumerChannel.BasicAck(args.DeliveryTag, false);
                        args = null;
                    }
                }
            }
        }

        public bool Send(object message)
        {
            return this.Send(message, this._producerRoutingKey);
        }

        public bool Send(object message, string targetRoutingkey)
        {
            if (this._isOpen)
            {
                IModel model;
                if (message == null)
                {
                    return false;
                }
                Monitor.Enter(model = this._producerChannel);
                try
                {
                    DateTime now = DateTime.Now;
                    IBasicProperties basicProperties = this._producerChannel.CreateBasicProperties();
                    basicProperties.Timestamp = new AmqpTimestamp(now.Ticks);
                    if (!string.IsNullOrEmpty(this._expiration))
                    {
                        basicProperties.Expiration = this._expiration;
                    }
                    byte[] body = this._messageParser.ObjectToByteArray(message);
                    this._producerChannel.BasicPublish(this._producerExchange, targetRoutingkey, basicProperties, body);
                    this.logger.Info(string.Format("{0} - Sent: {1}byte", now.ToString("yyyy-MM-dd HH:mm:ss.fff"), body.Length));
                    return true;
                }
                catch (Exception exception)
                {
                    this.logger.Error("Error Producer.");
                    this.logger.Error(exception);
                    if (this._isOpen)
                    {
                        this.RecoveryProducer(message, targetRoutingkey);
                    }
                }
                finally
                {
                    Monitor.Exit(model);
                }
            }
            return false;
        }

        public void SetAutoDeleteQueue()
        {
            this._isAutoDeleteQueue = true;
        }

        public string Expiration
        {
            get
            {
                return this._expiration;
            }
            set
            {
                if (value == "0")
                {
                    this._expiration = string.Empty;
                }
                int num = int.Parse(value);
                if (num < 0x3e8)
                {
                    num = 0x3e8;
                }
                this._expiration = num.ToString();
            }
        }

        public delegate void MessageReceivedEventHandler(object sender, object message);
    }
}
