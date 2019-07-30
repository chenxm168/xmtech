
namespace EQPIO.Common
{
    using System;

    public class ConfigManager
    {
        //private Configration m_configration = new Configration();
        private static ConfigManager m_instance = null;

        public void ShowConfigration()
        {
            //if (!this.m_configration.IsShowing)
            //{
            //    //this.m_configration = new Configration(Instance);
            //    this.m_configration.Show();
            //    this.m_configration.IsShowing = true;
            //}
        }

        public static ConfigManager Instance
        {
            get
            {
                return m_instance;
            }
        }

        public enum ConfigAttribute
        {
            LocalName,
            DriverName,
            Channel,
            PLCMapFile,
            btnPLCMapFile,
            IpAddress,
            MelsecPort,
            FixedBufferPort,
            IsMelsecEnabled,
            IsFixedBufferEnabled,
            CodeType,
            NetworkNo,
            PCNo,
            StationNo,
            CPUType,
            MessageType,
            HostName,
            HostIP,
            ProducerExchange,
            ProducerRoutingKey,
            ConsumerExchange,
            ConsumerRoutingKey,
            ConsumerQueue,
            Value
        }

        public enum Driver
        {
            RabbitMQInterface,
            MNetDriver,
            MNetProtocol,
            EIPDriver
        }
    }
}
