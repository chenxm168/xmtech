using System;
using System.Collections.Generic;
using System.Text;
using kr.co.aim.secomenabler.plugin;
using kr.co.aim.secomenabler.callback;
using kr.co.aim.secomenabler.global;
using kr.co.aim.secomenabler.structure;

namespace BMDT.SECS.Message
{
    class SECSWrapper : ISECSListener
    {
        SinglePlugIn mPlugIn = new SinglePlugIn();
        SECSConfig mConfig = new SECSConfig();

        #region Event
        delegate void ReceivedMessage(string driverID, SECSTransaction transaction);

		event ReceivedMessage OnS1F1_AreYouThere;
		event ReceivedMessage OnS1F2_IAmHere;
		event ReceivedMessage OnS1F15_RequestOffLine;
		event ReceivedMessage OnS1F17_RequestOnLine;
		event ReceivedMessage OnS2F17_DateTimeRequest;
		event ReceivedMessage OnS2F18_DateTimeDataRequestAck;
		event ReceivedMessage OnS2F31_DateAndTimeSetRequest;
		event ReceivedMessage OnS2F113_CurrentEQPDataRequest;
		event ReceivedMessage OnS2F111_CurrentRecipeDataRequest;
		event ReceivedMessage OnS5F2_AlarmReportAck;
		event ReceivedMessage OnS6F112_CurrentRecipeDataReportAck;
		event ReceivedMessage OnS6F114_CurrentEQPDataReportAck;
		event ReceivedMessage OnS6F4_ProcessDataReportAck;
		event ReceivedMessage OnS6F152_TackTimeReportAck;
		event ReceivedMessage OnS6F12_ProcessStateReportAck;
		event ReceivedMessage OnS2F41_OPCallRequest;
		event ReceivedMessage OnS2F41_EQPDownRequest;
		event ReceivedMessage OnS6F12_ControlOfflineChangeReportAck;
		event ReceivedMessage OnS6F12_ControlLocalChangeReportAck;
		event ReceivedMessage OnS6F12EQPStateChangeReportAck;

        #endregion

        public SECSWrapper()
        {
            LinkedReceivedMessageEvent();
        }

        public void Initialize1()
        {
            InitializeConfig();
            bool flag = this.mPlugIn.AddSECSListener(this);
            IReturnObject returnObject = this.mPlugIn.Initialize(this.mConfig);
            if (returnObject.isSuccess())
                Console.Out.WriteLine("Initialize Success");
            else
                Console.Out.WriteLine("Initialize Error: " + returnObject.getErrorDescription());
        }

        public void Initialize2()
        {
            this.mPlugIn.AddSECSListener(this);
            IReturnObject returnObject = this.mPlugIn.Initialize("HOST01", @"C:\Program Files\AIM Systems, Inc\SEComEnabler 4.0\Project\SEComINI\SEComINI.xml");
            if (returnObject.isSuccess())
                Console.Out.WriteLine("Initialize Success");
            else
                Console.Out.WriteLine("Initialize Error: " + returnObject.getErrorDescription());
        }

        public void Initialize3()
        {
            this.mPlugIn.AddSECSListener(this);
            IReturnObject returnObject = this.mPlugIn.Initialize("HOST01", "SEComINI.xml");
            if (returnObject.isSuccess())
                Console.Out.WriteLine("Initialize Success");
            else
                Console.Out.WriteLine("Initialize Error: " + returnObject.getErrorDescription());
        }

        private void InitializeConfig()
        {
            this.mConfig.Active = true;
            this.mConfig.AnalyzerOption = 7;
            this.mConfig.DeviceId = 0;
            this.mConfig.DispatchOn = true;
            this.mConfig.DriverId = "HOST01";
            this.mConfig.DriverLogLevel = 0;
            this.mConfig.Host = true;
            this.mConfig.IpAddress = "127.0.0.1";
            this.mConfig.LinktestDuration = 120;
            this.mConfig.LogModeDaily = true;
            this.mConfig.LogModeDeleteDuration = 3;
            this.mConfig.LogRootPath = ".";
            this.mConfig.ModelingInfoFromFile = @"C:\Program Files\AIM Systems, Inc\SEComEnabler 4.0\Project\Type\Test.smd";//Message File Path
            this.mConfig.OverRawBinaryLength = 10;
            this.mConfig.Port = 6000;
            this.mConfig.SecsLogMode = 1;
            this.mConfig.UseRawBinary = false;
            this.mConfig.Timeout6 = 30;
        }

        public void Terminate()
        {
            IReturnObject returnObject = this.mPlugIn.Terminate();
            if (returnObject.isSuccess())
                Console.Out.WriteLine("Terminate Success");
            else
                Console.Out.WriteLine("Terminate Error: " + returnObject.getErrorDescription());
        }

        public void HotSend(int stream, int function, string messageName)
        {
            IReturnObject returnObj = mPlugIn.GetDefinedMessage(stream, function, messageName);
            if(returnObj.isSuccess())
            {
                this.mPlugIn.request(returnObj.getReturnData() as SECSTransaction);
            }
        }

        internal void SendRequest(SECSTransaction trx)
        {
            this.mPlugIn.request(trx);
        }

        public void SendWithMessageClass(int stream, int function, string messageName)
        {
            //For Example: CTestMessage S6F9
            List<CTestMessage_DSIDCOUNT> dsidCount = new List<CTestMessage_DSIDCOUNT>();
            
            List<String> dvList = new List<String>();
            dvList.Add("ABC");
            dvList.Add("ABCDEF");
            dvList.Add("EEEE");
            dvList.Add("FFSE");
            CTestMessage_DSIDCOUNT dsid1 = new CTestMessage_DSIDCOUNT("33", dvList);

            List<String> dvList2 = new List<String>();
            dvList2.Add("ABC");
            dvList2.Add("WESD");
            dvList2.Add("DF");
            dvList2.Add("HGHDFRGSES");
            dvList2.Add("FGH4R633");
            CTestMessage_DSIDCOUNT dsid2 = new CTestMessage_DSIDCOUNT("23", dvList2);
            dsidCount.Add(dsid1);
            dsidCount.Add(dsid2);

            this.mPlugIn.request(CTestMessage.makeTransaction(true, "0111 11", "12", "23", dsidCount));
        }

        public void LinkedReceivedMessageEvent()
        {
			this.OnS1F1_AreYouThere += new ReceivedMessage(SECSWrapper_OnS1F1_AreYouThere);
			this.OnS1F2_IAmHere += new ReceivedMessage(SECSWrapper_OnS1F2_IAmHere);
			this.OnS1F15_RequestOffLine += new ReceivedMessage(SECSWrapper_OnS1F15_RequestOffLine);
			this.OnS1F17_RequestOnLine += new ReceivedMessage(SECSWrapper_OnS1F17_RequestOnLine);
			this.OnS2F17_DateTimeRequest += new ReceivedMessage(SECSWrapper_OnS2F17_DateTimeRequest);
			this.OnS2F18_DateTimeDataRequestAck += new ReceivedMessage(SECSWrapper_OnS2F18_DateTimeDataRequestAck);
			this.OnS2F31_DateAndTimeSetRequest += new ReceivedMessage(SECSWrapper_OnS2F31_DateAndTimeSetRequest);
			this.OnS2F113_CurrentEQPDataRequest += new ReceivedMessage(SECSWrapper_OnS2F113_CurrentEQPDataRequest);
			this.OnS2F111_CurrentRecipeDataRequest += new ReceivedMessage(SECSWrapper_OnS2F111_CurrentRecipeDataRequest);
			this.OnS5F2_AlarmReportAck += new ReceivedMessage(SECSWrapper_OnS5F2_AlarmReportAck);
			this.OnS6F112_CurrentRecipeDataReportAck += new ReceivedMessage(SECSWrapper_OnS6F112_CurrentRecipeDataReportAck);
			this.OnS6F114_CurrentEQPDataReportAck += new ReceivedMessage(SECSWrapper_OnS6F114_CurrentEQPDataReportAck);
			this.OnS6F4_ProcessDataReportAck += new ReceivedMessage(SECSWrapper_OnS6F4_ProcessDataReportAck);
			this.OnS6F152_TackTimeReportAck += new ReceivedMessage(SECSWrapper_OnS6F152_TackTimeReportAck);
			this.OnS6F12_ProcessStateReportAck += new ReceivedMessage(SECSWrapper_OnS6F12_ProcessStateReportAck);
			this.OnS2F41_OPCallRequest += new ReceivedMessage(SECSWrapper_OnS2F41_OPCallRequest);
			this.OnS2F41_EQPDownRequest += new ReceivedMessage(SECSWrapper_OnS2F41_EQPDownRequest);
			this.OnS6F12_ControlOfflineChangeReportAck += new ReceivedMessage(SECSWrapper_OnS6F12_ControlOfflineChangeReportAck);
			this.OnS6F12_ControlLocalChangeReportAck += new ReceivedMessage(SECSWrapper_OnS6F12_ControlLocalChangeReportAck);
			this.OnS6F12EQPStateChangeReportAck += new ReceivedMessage(SECSWrapper_OnS6F12EQPStateChangeReportAck);

        }

        #region ISECSListener Member

        public void onConnected(string driverID)
        {
            Console.Out.WriteLine("DriverID : Connected");
            SendWithMessageClass(6, 9, "TestMessage");
        }

        public void onDisconnected(string driverID)
        {
            Console.Out.WriteLine("DriverID : Disconnected");
        }

        public void onIllegal(string driverID, SECSTransaction transaction)
        {
            Console.Out.WriteLine("Illegal : " + transaction.MessageName);
        }

        public void onLog(string driverID, string log)
        {
            Console.Out.WriteLine(log);
        }

        public void onReceived(string driverID, SECSTransaction transaction)
        {
            Console.Out.WriteLine("Receive : " + transaction.MessageName);
            
            switch (transaction.MessageName)
            {
				case "S1F1_AreYouThere":
					OnS1F1_AreYouThere(driverID, transaction); break;
				case "S1F2_IAmHere":
					OnS1F2_IAmHere(driverID, transaction); break;
				case "S1F15_RequestOffLine":
					OnS1F15_RequestOffLine(driverID, transaction); break;
				case "S1F17_RequestOnLine":
					OnS1F17_RequestOnLine(driverID, transaction); break;
				case "S2F17_DateTimeRequest":
					OnS2F17_DateTimeRequest(driverID, transaction); break;
				case "S2F18_DateTimeDataRequestAck":
					OnS2F18_DateTimeDataRequestAck(driverID, transaction); break;
				case "S2F31_DateAndTimeSetRequest":
					OnS2F31_DateAndTimeSetRequest(driverID, transaction); break;
				case "S2F113_CurrentEQPDataRequest":
					OnS2F113_CurrentEQPDataRequest(driverID, transaction); break;
				case "S2F111_CurrentRecipeDataRequest":
					OnS2F111_CurrentRecipeDataRequest(driverID, transaction); break;
				case "S5F2_AlarmReportAck":
					OnS5F2_AlarmReportAck(driverID, transaction); break;
				case "S6F112_CurrentRecipeDataReportAck":
					OnS6F112_CurrentRecipeDataReportAck(driverID, transaction); break;
				case "S6F114_CurrentEQPDataReportAck":
					OnS6F114_CurrentEQPDataReportAck(driverID, transaction); break;
				case "S6F4_ProcessDataReportAck":
					OnS6F4_ProcessDataReportAck(driverID, transaction); break;
				case "S6F152_TackTimeReportAck":
					OnS6F152_TackTimeReportAck(driverID, transaction); break;
				case "S6F12_ProcessStateReportAck":
					OnS6F12_ProcessStateReportAck(driverID, transaction); break;
				case "S2F41_OPCallRequest":
					OnS2F41_OPCallRequest(driverID, transaction); break;
				case "S2F41_EQPDownRequest":
					OnS2F41_EQPDownRequest(driverID, transaction); break;
				case "S6F12_ControlOfflineChangeReportAck":
					OnS6F12_ControlOfflineChangeReportAck(driverID, transaction); break;
				case "S6F12_ControlLocalChangeReportAck":
					OnS6F12_ControlLocalChangeReportAck(driverID, transaction); break;
				case "S6F12EQPStateChangeReportAck":
					OnS6F12EQPStateChangeReportAck(driverID, transaction); break;

                default:
                    break;
            }

        }

        public void onSendComplete(string driverID, SECSTransaction transaction)
        {
            Console.Out.WriteLine("SendComplete : " + transaction.MessageName);
        }

        public void onTimeout(string driverID, kr.co.aim.secomenabler.timeout.SECSTimeout timeout)
        {
            Console.Out.WriteLine("Timeout : " + timeout.Type);
        }

        public void onUnknownReceived(string driverID, SECSTransaction transaction)
        {
            Console.Out.WriteLine("Unknown Received : " + transaction.MessageName);
        }

        #endregion

        #region Received Message Event Function
		void SECSWrapper_OnS1F1_AreYouThere(string driverID, SECSTransaction transaction)
		{
			S1F1_AreYouThere S1F1_AreYouThere = new S1F1_AreYouThere(transaction);
		}

		void SECSWrapper_OnS1F2_IAmHere(string driverID, SECSTransaction transaction)
		{
			S1F2_IAmHere S1F2_IAmHere = new S1F2_IAmHere(transaction);
		}

		void SECSWrapper_OnS1F15_RequestOffLine(string driverID, SECSTransaction transaction)
		{
			S1F15_RequestOffLine S1F15_RequestOffLine = new S1F15_RequestOffLine(transaction);
		}

		void SECSWrapper_OnS1F17_RequestOnLine(string driverID, SECSTransaction transaction)
		{
			S1F17_RequestOnLine S1F17_RequestOnLine = new S1F17_RequestOnLine(transaction);
		}

		void SECSWrapper_OnS2F17_DateTimeRequest(string driverID, SECSTransaction transaction)
		{
			S2F17_DateTimeRequest S2F17_DateTimeRequest = new S2F17_DateTimeRequest(transaction);
		}

		void SECSWrapper_OnS2F18_DateTimeDataRequestAck(string driverID, SECSTransaction transaction)
		{
			S2F18_DateTimeDataRequestAck S2F18_DateTimeDataRequestAck = new S2F18_DateTimeDataRequestAck(transaction);
		}

		void SECSWrapper_OnS2F31_DateAndTimeSetRequest(string driverID, SECSTransaction transaction)
		{
			S2F31_DateAndTimeSetRequest S2F31_DateAndTimeSetRequest = new S2F31_DateAndTimeSetRequest(transaction);
		}

		void SECSWrapper_OnS2F113_CurrentEQPDataRequest(string driverID, SECSTransaction transaction)
		{
			S2F113_CurrentEQPDataRequest S2F113_CurrentEQPDataRequest = new S2F113_CurrentEQPDataRequest(transaction);
		}

		void SECSWrapper_OnS2F111_CurrentRecipeDataRequest(string driverID, SECSTransaction transaction)
		{
			S2F111_CurrentRecipeDataRequest S2F111_CurrentRecipeDataRequest = new S2F111_CurrentRecipeDataRequest(transaction);
		}

		void SECSWrapper_OnS5F2_AlarmReportAck(string driverID, SECSTransaction transaction)
		{
			S5F2_AlarmReportAck S5F2_AlarmReportAck = new S5F2_AlarmReportAck(transaction);
		}

		void SECSWrapper_OnS6F112_CurrentRecipeDataReportAck(string driverID, SECSTransaction transaction)
		{
			S6F112_CurrentRecipeDataReportAck S6F112_CurrentRecipeDataReportAck = new S6F112_CurrentRecipeDataReportAck(transaction);
		}

		void SECSWrapper_OnS6F114_CurrentEQPDataReportAck(string driverID, SECSTransaction transaction)
		{
			S6F114_CurrentEQPDataReportAck S6F114_CurrentEQPDataReportAck = new S6F114_CurrentEQPDataReportAck(transaction);
		}

		void SECSWrapper_OnS6F4_ProcessDataReportAck(string driverID, SECSTransaction transaction)
		{
			S6F4_ProcessDataReportAck S6F4_ProcessDataReportAck = new S6F4_ProcessDataReportAck(transaction);
		}

		void SECSWrapper_OnS6F152_TackTimeReportAck(string driverID, SECSTransaction transaction)
		{
			S6F152_TackTimeReportAck S6F152_TackTimeReportAck = new S6F152_TackTimeReportAck(transaction);
		}

		void SECSWrapper_OnS6F12_ProcessStateReportAck(string driverID, SECSTransaction transaction)
		{
			S6F12_ProcessStateReportAck S6F12_ProcessStateReportAck = new S6F12_ProcessStateReportAck(transaction);
		}

		void SECSWrapper_OnS2F41_OPCallRequest(string driverID, SECSTransaction transaction)
		{
			S2F41_OPCallRequest S2F41_OPCallRequest = new S2F41_OPCallRequest(transaction);
		}

		void SECSWrapper_OnS2F41_EQPDownRequest(string driverID, SECSTransaction transaction)
		{
			S2F41_EQPDownRequest S2F41_EQPDownRequest = new S2F41_EQPDownRequest(transaction);
		}

		void SECSWrapper_OnS6F12_ControlOfflineChangeReportAck(string driverID, SECSTransaction transaction)
		{
			S6F12_ControlOfflineChangeReportAck S6F12_ControlOfflineChangeReportAck = new S6F12_ControlOfflineChangeReportAck(transaction);
		}

		void SECSWrapper_OnS6F12_ControlLocalChangeReportAck(string driverID, SECSTransaction transaction)
		{
			S6F12_ControlLocalChangeReportAck S6F12_ControlLocalChangeReportAck = new S6F12_ControlLocalChangeReportAck(transaction);
		}

		void SECSWrapper_OnS6F12EQPStateChangeReportAck(string driverID, SECSTransaction transaction)
		{
			S6F12EQPStateChangeReportAck S6F12EQPStateChangeReportAck = new S6F12EQPStateChangeReportAck(transaction);
		}


        #endregion
    }
}
