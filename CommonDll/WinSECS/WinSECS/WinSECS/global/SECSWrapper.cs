using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.driver;
using WinSECS.callback;
using WinSECS.global;
using WinSECS.structure;

namespace WinSECS
{
  public  class SECSWrapper : ISECSListener,IDisposable
    {
        SinglePlugIn mPlugIn = new SinglePlugIn();
        SECSConfig mConfig = new SECSConfig();
        private string driverName = "driver01";
        private string driverFile = "SecsConfig.xml";
        public bool IsConnected = false;

        #region Event
        delegate void ReceivedMessage(string driverID, SECSTransaction transaction);

		event ReceivedMessage OnS1F0_ABORTTRANSACTION;
		event ReceivedMessage OnS1F1_AREYOUTHERE_TOEQP;
		event ReceivedMessage OnS1F2_IAMHERE;
		event ReceivedMessage OnS1F3_FDCEQPSTATUSREQUEST;
		event ReceivedMessage OnS1F3_NOUSE;
		event ReceivedMessage OnS1F3_NOUSE_1;
		event ReceivedMessage OnS1F5_NOUSE_21;
		event ReceivedMessage OnS1F5_NOUSE_22;
		event ReceivedMessage OnS1F5_NOUSE_2;
		event ReceivedMessage OnS1F5_NOUSE_32;
		event ReceivedMessage OnS1F5_NOUSE_6_TYPE1;
		event ReceivedMessage OnS1F5_NOUSE_6_TYPE2;
		event ReceivedMessage OnS1F5_NOUSE_6_TYPE3;
		event ReceivedMessage OnS1F5_NOUSE_3_TYPE1;
		event ReceivedMessage OnS1F5_NOUSE_3;
		event ReceivedMessage OnS1F5_NOUSE_33;
		event ReceivedMessage OnS1F5_NOUSE_11;
		event ReceivedMessage OnS1F5_EQPSTATUSREQUEST;
		event ReceivedMessage OnS1F5_NOUSE_4_TYPE1;
		event ReceivedMessage OnS1F5_NOUSE_4_TYPE2;
		event ReceivedMessage OnS1F5_NOUSE_31;
		event ReceivedMessage OnS1F5_NOUSE_5;
		event ReceivedMessage OnS1F5_iNOUSE_2;
		event ReceivedMessage OnS1F5_iNOUSE_32;
		event ReceivedMessage OnS1F5_iNOUSE_33;
		event ReceivedMessage OnS1F5_iNOUSE_4;
		event ReceivedMessage OnS1F5_iNOUSE_31;
		event ReceivedMessage OnS1F5_iEQPSTATUSREQUEST;
		event ReceivedMessage OnS1F5_iNOUSE_5;
		event ReceivedMessage OnS1F11_FDCEQPSTATUSNAMELISTREQUEST;
		event ReceivedMessage OnS1F11_NOUSE;
		event ReceivedMessage OnS1F15_iOFFLINECHANGEREQUEST;
		event ReceivedMessage OnS1F15_OFFLINECHANGEREQUEST;
		event ReceivedMessage OnS2F0_ABORTTRANSACTION;
		event ReceivedMessage OnS1F17_iONLINECHANGEREQUEST;
		event ReceivedMessage OnS1F17_ONLINECHANGEREQUEST;
		event ReceivedMessage OnS1F101_SVSTATUSREQUEST;
		event ReceivedMessage OnS2F15_ECMEQPNEWCONSTANTREQUEST;
		event ReceivedMessage OnS2F18_ONLINEDATETIMEDATA;
		event ReceivedMessage OnS2F23_FDCTRACEINITREQUEST;
		event ReceivedMessage OnS2F29_ECMEQPCONSTANTNAMELISTREQUEST;
		event ReceivedMessage OnS2F41_GLASSCOMMAND;
		event ReceivedMessage OnS2F41_iEQPCOMMAND;
		event ReceivedMessage OnS2F41_iGLASSCOMMAND;
		event ReceivedMessage OnS2F41_iJOBPROCESSCOMMAND;
		event ReceivedMessage OnS2F41_iSPECIFICAREARWCOMMAND;
		event ReceivedMessage OnS2F41_JOBPROCESSCOMMAND;
		event ReceivedMessage OnS2F41_PORTCOMMAND;
		event ReceivedMessage OnS3F0_ABORTTRANSACTION;
		event ReceivedMessage OnS2F41_UNITCOMMAND;
		event ReceivedMessage OnS2F101_OPERATORCALLSEND;
		event ReceivedMessage OnS2F103_EOIDEQPPARAMETERCHANGEREQUEST;
		event ReceivedMessage OnS3F1_MASKINFORMAIONREQEUST;
		event ReceivedMessage OnS3F101_CASSETTEINFORMATIONSEND_TYPE1;
		event ReceivedMessage OnS3F101_CASSETTEINFORMATIONSEND_TYPE2;
		event ReceivedMessage OnS5F0_ABORTTRANSACTION;
		event ReceivedMessage OnS3F101_CASSETTEINFORMATIONSEND_TYPE3;
		event ReceivedMessage OnS3F101_CASSETTEINFORMATIONSEND_TYPE4;
		event ReceivedMessage OnS3F101_iCASSETTEINFORMATIONSEND;
		event ReceivedMessage OnS5F2_ALARMREPORTREPLY;
		event ReceivedMessage OnS6F0_ABORTTRANSACTION;
		event ReceivedMessage OnS5F101_ALARMLISTREQUEST;
		event ReceivedMessage OnS5F101_NOUSE;
		event ReceivedMessage OnS5F103_ALARMRESETREQEUST;
		event ReceivedMessage OnS6F2_FDCTRACEDATAREPLY;
		event ReceivedMessage OnS6F2_AUTOREPLY;
		event ReceivedMessage OnS6F12_AUTOREPLY;
		event ReceivedMessage OnS6F12_AUTOREPLY_1;
		event ReceivedMessage OnS6F12_AUTOREPLY_2;
		event ReceivedMessage OnS6F12_AUTOREPLY_3;
		event ReceivedMessage OnS6F12_AUTOREPLY_4;
		event ReceivedMessage OnS6F12_AUTOREPLY_5;
		event ReceivedMessage OnS6F12_AUTOREPLY_6;
		event ReceivedMessage OnS6F12_AUTOREPLY_7;
		event ReceivedMessage OnS6F12_AUTOREPLY_8;
		event ReceivedMessage OnS6F12_AUTOREPLY_9;
		event ReceivedMessage OnS6F12_AUTOREPLY_10;
		event ReceivedMessage OnS6F12_AUTOREPLY_11;
		event ReceivedMessage OnS6F12_AUTOREPLY_12;
		event ReceivedMessage OnS6F12_AUTOREPLY_13;
		event ReceivedMessage OnS6F12_AUTOREPLY_14;
		event ReceivedMessage OnS6F12_AUTOREPLY_15;
		event ReceivedMessage OnS6F12_AUTOREPLY_17;
		event ReceivedMessage OnS6F12_AUTOREPLY_18;
		event ReceivedMessage OnS6F12_AUTOREPLY_19;
		event ReceivedMessage OnS6F12_AUTOREPLY_16;
		event ReceivedMessage OnS6F12_AUTOREPLY_20;
		event ReceivedMessage OnS6F12_AUTOREPLY_21;
		event ReceivedMessage OnS6F14_AUTOREPLY;
		event ReceivedMessage OnS6F14_AUTOREPLY_1;
		event ReceivedMessage OnS6F14_AUTOREPLY_2;
		event ReceivedMessage OnS6F14_AUTOREPLY_3;
		event ReceivedMessage OnS6F14_AUTOREPLY_4;
		event ReceivedMessage OnS6F14_AUTOREPLY_5;
		event ReceivedMessage OnS6F14_AUTOREPLY_6;
		event ReceivedMessage OnS6F14_AUTOREPLY_8;
		event ReceivedMessage OnS6F14_AUTOREPLY_9;
		event ReceivedMessage OnS7F0_ABORTTRANSACTION;
		event ReceivedMessage OnS6F14_AUTOREPLY_9_1;
		event ReceivedMessage OnS6F14_AUTOREPLY_7;
		event ReceivedMessage OnS6F102_LOTLISTREPLY;
		event ReceivedMessage OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A;
		event ReceivedMessage OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B;
		event ReceivedMessage OnS7F25_RMSFORMATTEDPPIDDATAREQUEST;
		event ReceivedMessage OnS7F25_NOUSE;
		event ReceivedMessage OnS9F0_ABORTTRANSACTION;
		event ReceivedMessage OnS7F101_RMSCURRENTPPIDREQUEST;
		event ReceivedMessage OnS7F103_RMSPPIDEXISTENCEREQUEST;
		event ReceivedMessage OnS7F105_RMSPPIDCHANGETIMEREQUEST;
		event ReceivedMessage OnS10F0_ABORTTRANSACTION;
		event ReceivedMessage OnS10F2_AUTOREPLY;
		event ReceivedMessage OnS10F3_TERMINALDISPLAYSEND;
		event ReceivedMessage OnS10F9_BROADCASTSEND;
		event ReceivedMessage OnS6F12;
		event ReceivedMessage OnS6F14;
		event ReceivedMessage OnS7F101_F1PSH01;
		event ReceivedMessage OnS7F25_NOUSE_1;
		event ReceivedMessage OnS6F12_1;

        #endregion

        public SECSWrapper()
        {
            LinkedReceivedMessageEvent();
        }

        public SECSWrapper(string driver_name, string driver_file)
        {
            this.driverName = driver_name;
            this.driverFile = driver_file;
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
            IReturnObject returnObject = this.mPlugIn.Initialize(driverName, driverFile);
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

      public void HotSend(SECSTransaction trx)
        {
            SendRequest(trx);
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
			this.OnS1F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS1F0_ABORTTRANSACTION);
			this.OnS1F1_AREYOUTHERE_TOEQP += new ReceivedMessage(SECSWrapper_OnS1F1_AREYOUTHERE_TOEQP);
			this.OnS1F2_IAMHERE += new ReceivedMessage(SECSWrapper_OnS1F2_IAMHERE);
			this.OnS1F3_FDCEQPSTATUSREQUEST += new ReceivedMessage(SECSWrapper_OnS1F3_FDCEQPSTATUSREQUEST);
			this.OnS1F3_NOUSE += new ReceivedMessage(SECSWrapper_OnS1F3_NOUSE);
			this.OnS1F3_NOUSE_1 += new ReceivedMessage(SECSWrapper_OnS1F3_NOUSE_1);
			this.OnS1F5_NOUSE_21 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_21);
			this.OnS1F5_NOUSE_22 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_22);
			this.OnS1F5_NOUSE_2 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_2);
			this.OnS1F5_NOUSE_32 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_32);
			this.OnS1F5_NOUSE_6_TYPE1 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_6_TYPE1);
			this.OnS1F5_NOUSE_6_TYPE2 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_6_TYPE2);
			this.OnS1F5_NOUSE_6_TYPE3 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_6_TYPE3);
			this.OnS1F5_NOUSE_3_TYPE1 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_3_TYPE1);
			this.OnS1F5_NOUSE_3 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_3);
			this.OnS1F5_NOUSE_33 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_33);
			this.OnS1F5_NOUSE_11 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_11);
			this.OnS1F5_EQPSTATUSREQUEST += new ReceivedMessage(SECSWrapper_OnS1F5_EQPSTATUSREQUEST);
			this.OnS1F5_NOUSE_4_TYPE1 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_4_TYPE1);
			this.OnS1F5_NOUSE_4_TYPE2 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_4_TYPE2);
			this.OnS1F5_NOUSE_31 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_31);
			this.OnS1F5_NOUSE_5 += new ReceivedMessage(SECSWrapper_OnS1F5_NOUSE_5);
			this.OnS1F5_iNOUSE_2 += new ReceivedMessage(SECSWrapper_OnS1F5_iNOUSE_2);
			this.OnS1F5_iNOUSE_32 += new ReceivedMessage(SECSWrapper_OnS1F5_iNOUSE_32);
			this.OnS1F5_iNOUSE_33 += new ReceivedMessage(SECSWrapper_OnS1F5_iNOUSE_33);
			this.OnS1F5_iNOUSE_4 += new ReceivedMessage(SECSWrapper_OnS1F5_iNOUSE_4);
			this.OnS1F5_iNOUSE_31 += new ReceivedMessage(SECSWrapper_OnS1F5_iNOUSE_31);
			this.OnS1F5_iEQPSTATUSREQUEST += new ReceivedMessage(SECSWrapper_OnS1F5_iEQPSTATUSREQUEST);
			this.OnS1F5_iNOUSE_5 += new ReceivedMessage(SECSWrapper_OnS1F5_iNOUSE_5);
			this.OnS1F11_FDCEQPSTATUSNAMELISTREQUEST += new ReceivedMessage(SECSWrapper_OnS1F11_FDCEQPSTATUSNAMELISTREQUEST);
			this.OnS1F11_NOUSE += new ReceivedMessage(SECSWrapper_OnS1F11_NOUSE);
			this.OnS1F15_iOFFLINECHANGEREQUEST += new ReceivedMessage(SECSWrapper_OnS1F15_iOFFLINECHANGEREQUEST);
			this.OnS1F15_OFFLINECHANGEREQUEST += new ReceivedMessage(SECSWrapper_OnS1F15_OFFLINECHANGEREQUEST);
			this.OnS2F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS2F0_ABORTTRANSACTION);
			this.OnS1F17_iONLINECHANGEREQUEST += new ReceivedMessage(SECSWrapper_OnS1F17_iONLINECHANGEREQUEST);
			this.OnS1F17_ONLINECHANGEREQUEST += new ReceivedMessage(SECSWrapper_OnS1F17_ONLINECHANGEREQUEST);
			this.OnS1F101_SVSTATUSREQUEST += new ReceivedMessage(SECSWrapper_OnS1F101_SVSTATUSREQUEST);
			this.OnS2F15_ECMEQPNEWCONSTANTREQUEST += new ReceivedMessage(SECSWrapper_OnS2F15_ECMEQPNEWCONSTANTREQUEST);
			this.OnS2F18_ONLINEDATETIMEDATA += new ReceivedMessage(SECSWrapper_OnS2F18_ONLINEDATETIMEDATA);
			this.OnS2F23_FDCTRACEINITREQUEST += new ReceivedMessage(SECSWrapper_OnS2F23_FDCTRACEINITREQUEST);
			this.OnS2F29_ECMEQPCONSTANTNAMELISTREQUEST += new ReceivedMessage(SECSWrapper_OnS2F29_ECMEQPCONSTANTNAMELISTREQUEST);
			this.OnS2F41_GLASSCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_GLASSCOMMAND);
			this.OnS2F41_iEQPCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_iEQPCOMMAND);
			this.OnS2F41_iGLASSCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_iGLASSCOMMAND);
			this.OnS2F41_iJOBPROCESSCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_iJOBPROCESSCOMMAND);
			this.OnS2F41_iSPECIFICAREARWCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_iSPECIFICAREARWCOMMAND);
			this.OnS2F41_JOBPROCESSCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_JOBPROCESSCOMMAND);
			this.OnS2F41_PORTCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_PORTCOMMAND);
			this.OnS3F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS3F0_ABORTTRANSACTION);
			this.OnS2F41_UNITCOMMAND += new ReceivedMessage(SECSWrapper_OnS2F41_UNITCOMMAND);
			this.OnS2F101_OPERATORCALLSEND += new ReceivedMessage(SECSWrapper_OnS2F101_OPERATORCALLSEND);
			this.OnS2F103_EOIDEQPPARAMETERCHANGEREQUEST += new ReceivedMessage(SECSWrapper_OnS2F103_EOIDEQPPARAMETERCHANGEREQUEST);
			this.OnS3F1_MASKINFORMAIONREQEUST += new ReceivedMessage(SECSWrapper_OnS3F1_MASKINFORMAIONREQEUST);
			this.OnS3F101_CASSETTEINFORMATIONSEND_TYPE1 += new ReceivedMessage(SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE1);
			this.OnS3F101_CASSETTEINFORMATIONSEND_TYPE2 += new ReceivedMessage(SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE2);
			this.OnS5F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS5F0_ABORTTRANSACTION);
			this.OnS3F101_CASSETTEINFORMATIONSEND_TYPE3 += new ReceivedMessage(SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE3);
			this.OnS3F101_CASSETTEINFORMATIONSEND_TYPE4 += new ReceivedMessage(SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE4);
			this.OnS3F101_iCASSETTEINFORMATIONSEND += new ReceivedMessage(SECSWrapper_OnS3F101_iCASSETTEINFORMATIONSEND);
			this.OnS5F2_ALARMREPORTREPLY += new ReceivedMessage(SECSWrapper_OnS5F2_ALARMREPORTREPLY);
			this.OnS6F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS6F0_ABORTTRANSACTION);
			this.OnS5F101_ALARMLISTREQUEST += new ReceivedMessage(SECSWrapper_OnS5F101_ALARMLISTREQUEST);
			this.OnS5F101_NOUSE += new ReceivedMessage(SECSWrapper_OnS5F101_NOUSE);
			this.OnS5F103_ALARMRESETREQEUST += new ReceivedMessage(SECSWrapper_OnS5F103_ALARMRESETREQEUST);
			this.OnS6F2_FDCTRACEDATAREPLY += new ReceivedMessage(SECSWrapper_OnS6F2_FDCTRACEDATAREPLY);
			this.OnS6F2_AUTOREPLY += new ReceivedMessage(SECSWrapper_OnS6F2_AUTOREPLY);
			this.OnS6F12_AUTOREPLY += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY);
			this.OnS6F12_AUTOREPLY_1 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_1);
			this.OnS6F12_AUTOREPLY_2 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_2);
			this.OnS6F12_AUTOREPLY_3 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_3);
			this.OnS6F12_AUTOREPLY_4 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_4);
			this.OnS6F12_AUTOREPLY_5 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_5);
			this.OnS6F12_AUTOREPLY_6 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_6);
			this.OnS6F12_AUTOREPLY_7 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_7);
			this.OnS6F12_AUTOREPLY_8 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_8);
			this.OnS6F12_AUTOREPLY_9 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_9);
			this.OnS6F12_AUTOREPLY_10 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_10);
			this.OnS6F12_AUTOREPLY_11 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_11);
			this.OnS6F12_AUTOREPLY_12 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_12);
			this.OnS6F12_AUTOREPLY_13 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_13);
			this.OnS6F12_AUTOREPLY_14 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_14);
			this.OnS6F12_AUTOREPLY_15 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_15);
			this.OnS6F12_AUTOREPLY_17 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_17);
			this.OnS6F12_AUTOREPLY_18 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_18);
			this.OnS6F12_AUTOREPLY_19 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_19);
			this.OnS6F12_AUTOREPLY_16 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_16);
			this.OnS6F12_AUTOREPLY_20 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_20);
			this.OnS6F12_AUTOREPLY_21 += new ReceivedMessage(SECSWrapper_OnS6F12_AUTOREPLY_21);
			this.OnS6F14_AUTOREPLY += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY);
			this.OnS6F14_AUTOREPLY_1 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_1);
			this.OnS6F14_AUTOREPLY_2 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_2);
			this.OnS6F14_AUTOREPLY_3 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_3);
			this.OnS6F14_AUTOREPLY_4 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_4);
			this.OnS6F14_AUTOREPLY_5 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_5);
			this.OnS6F14_AUTOREPLY_6 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_6);
			this.OnS6F14_AUTOREPLY_8 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_8);
			this.OnS6F14_AUTOREPLY_9 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_9);
			this.OnS7F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS7F0_ABORTTRANSACTION);
			this.OnS6F14_AUTOREPLY_9_1 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_9_1);
			this.OnS6F14_AUTOREPLY_7 += new ReceivedMessage(SECSWrapper_OnS6F14_AUTOREPLY_7);
			this.OnS6F102_LOTLISTREPLY += new ReceivedMessage(SECSWrapper_OnS6F102_LOTLISTREPLY);
			this.OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A += new ReceivedMessage(SECSWrapper_OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A);
			this.OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B += new ReceivedMessage(SECSWrapper_OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B);
			this.OnS7F25_RMSFORMATTEDPPIDDATAREQUEST += new ReceivedMessage(SECSWrapper_OnS7F25_RMSFORMATTEDPPIDDATAREQUEST);
			this.OnS7F25_NOUSE += new ReceivedMessage(SECSWrapper_OnS7F25_NOUSE);
			this.OnS9F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS9F0_ABORTTRANSACTION);
			this.OnS7F101_RMSCURRENTPPIDREQUEST += new ReceivedMessage(SECSWrapper_OnS7F101_RMSCURRENTPPIDREQUEST);
			this.OnS7F103_RMSPPIDEXISTENCEREQUEST += new ReceivedMessage(SECSWrapper_OnS7F103_RMSPPIDEXISTENCEREQUEST);
			this.OnS7F105_RMSPPIDCHANGETIMEREQUEST += new ReceivedMessage(SECSWrapper_OnS7F105_RMSPPIDCHANGETIMEREQUEST);
			this.OnS10F0_ABORTTRANSACTION += new ReceivedMessage(SECSWrapper_OnS10F0_ABORTTRANSACTION);
			this.OnS10F2_AUTOREPLY += new ReceivedMessage(SECSWrapper_OnS10F2_AUTOREPLY);
			this.OnS10F3_TERMINALDISPLAYSEND += new ReceivedMessage(SECSWrapper_OnS10F3_TERMINALDISPLAYSEND);
			this.OnS10F9_BROADCASTSEND += new ReceivedMessage(SECSWrapper_OnS10F9_BROADCASTSEND);
			this.OnS6F12 += new ReceivedMessage(SECSWrapper_OnS6F12);
			this.OnS6F14 += new ReceivedMessage(SECSWrapper_OnS6F14);
			this.OnS7F101_F1PSH01 += new ReceivedMessage(SECSWrapper_OnS7F101_F1PSH01);
			this.OnS7F25_NOUSE_1 += new ReceivedMessage(SECSWrapper_OnS7F25_NOUSE_1);
			this.OnS6F12_1 += new ReceivedMessage(SECSWrapper_OnS6F12_1);

        }

        #region ISECSListener Member

        public void onConnected(string driverID)
        {
            IsConnected = true;
            Console.Out.WriteLine("DriverID : Connected");
            //SendWithMessageClass(6, 9, "TestMessage");
        }

        public void onDisconnected(string driverID)
        {
            IsConnected = false;
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
				case "S1F0_ABORTTRANSACTION":
					OnS1F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S1F1_AREYOUTHERE_TOEQP":
					OnS1F1_AREYOUTHERE_TOEQP(driverID, transaction); break;
				case "S1F2_IAMHERE":
					OnS1F2_IAMHERE(driverID, transaction); break;
				case "S1F3_FDCEQPSTATUSREQUEST":
					OnS1F3_FDCEQPSTATUSREQUEST(driverID, transaction); break;
				case "S1F3_NOUSE":
					OnS1F3_NOUSE(driverID, transaction); break;
				case "S1F3_NOUSE_1":
					OnS1F3_NOUSE_1(driverID, transaction); break;
				case "S1F5_NOUSE_21":
					OnS1F5_NOUSE_21(driverID, transaction); break;
				case "S1F5_NOUSE_22":
					OnS1F5_NOUSE_22(driverID, transaction); break;
				case "S1F5_NOUSE_2":
					OnS1F5_NOUSE_2(driverID, transaction); break;
				case "S1F5_NOUSE_32":
					OnS1F5_NOUSE_32(driverID, transaction); break;
				case "S1F5_NOUSE_6_TYPE1":
					OnS1F5_NOUSE_6_TYPE1(driverID, transaction); break;
				case "S1F5_NOUSE_6_TYPE2":
					OnS1F5_NOUSE_6_TYPE2(driverID, transaction); break;
				case "S1F5_NOUSE_6_TYPE3":
					OnS1F5_NOUSE_6_TYPE3(driverID, transaction); break;
				case "S1F5_NOUSE_3_TYPE1":
					OnS1F5_NOUSE_3_TYPE1(driverID, transaction); break;
				case "S1F5_NOUSE_3":
					OnS1F5_NOUSE_3(driverID, transaction); break;
				case "S1F5_NOUSE_33":
					OnS1F5_NOUSE_33(driverID, transaction); break;
				case "S1F5_NOUSE_11":
					OnS1F5_NOUSE_11(driverID, transaction); break;
				case "S1F5_EQPSTATUSREQUEST":
					OnS1F5_EQPSTATUSREQUEST(driverID, transaction); break;
				case "S1F5_NOUSE_4_TYPE1":
					OnS1F5_NOUSE_4_TYPE1(driverID, transaction); break;
				case "S1F5_NOUSE_4_TYPE2":
					OnS1F5_NOUSE_4_TYPE2(driverID, transaction); break;
				case "S1F5_NOUSE_31":
					OnS1F5_NOUSE_31(driverID, transaction); break;
				case "S1F5_NOUSE_5":
					OnS1F5_NOUSE_5(driverID, transaction); break;
				case "S1F5_iNOUSE_2":
					OnS1F5_iNOUSE_2(driverID, transaction); break;
				case "S1F5_iNOUSE_32":
					OnS1F5_iNOUSE_32(driverID, transaction); break;
				case "S1F5_iNOUSE_33":
					OnS1F5_iNOUSE_33(driverID, transaction); break;
				case "S1F5_iNOUSE_4":
					OnS1F5_iNOUSE_4(driverID, transaction); break;
				case "S1F5_iNOUSE_31":
					OnS1F5_iNOUSE_31(driverID, transaction); break;
				case "S1F5_iEQPSTATUSREQUEST":
					OnS1F5_iEQPSTATUSREQUEST(driverID, transaction); break;
				case "S1F5_iNOUSE_5":
					OnS1F5_iNOUSE_5(driverID, transaction); break;
				case "S1F11_FDCEQPSTATUSNAMELISTREQUEST":
					OnS1F11_FDCEQPSTATUSNAMELISTREQUEST(driverID, transaction); break;
				case "S1F11_NOUSE":
					OnS1F11_NOUSE(driverID, transaction); break;
				case "S1F15_iOFFLINECHANGEREQUEST":
					OnS1F15_iOFFLINECHANGEREQUEST(driverID, transaction); break;
				case "S1F15_OFFLINECHANGEREQUEST":
					OnS1F15_OFFLINECHANGEREQUEST(driverID, transaction); break;
				case "S2F0_ABORTTRANSACTION":
					OnS2F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S1F17_iONLINECHANGEREQUEST":
					OnS1F17_iONLINECHANGEREQUEST(driverID, transaction); break;
				case "S1F17_ONLINECHANGEREQUEST":
					OnS1F17_ONLINECHANGEREQUEST(driverID, transaction); break;
				case "S1F101_SVSTATUSREQUEST":
					OnS1F101_SVSTATUSREQUEST(driverID, transaction); break;
				case "S2F15_ECMEQPNEWCONSTANTREQUEST":
					OnS2F15_ECMEQPNEWCONSTANTREQUEST(driverID, transaction); break;
				case "S2F18_ONLINEDATETIMEDATA":
					OnS2F18_ONLINEDATETIMEDATA(driverID, transaction); break;
				case "S2F23_FDCTRACEINITREQUEST":
					OnS2F23_FDCTRACEINITREQUEST(driverID, transaction); break;
				case "S2F29_ECMEQPCONSTANTNAMELISTREQUEST":
					OnS2F29_ECMEQPCONSTANTNAMELISTREQUEST(driverID, transaction); break;
				case "S2F41_GLASSCOMMAND":
					OnS2F41_GLASSCOMMAND(driverID, transaction); break;
				case "S2F41_iEQPCOMMAND":
					OnS2F41_iEQPCOMMAND(driverID, transaction); break;
				case "S2F41_iGLASSCOMMAND":
					OnS2F41_iGLASSCOMMAND(driverID, transaction); break;
				case "S2F41_iJOBPROCESSCOMMAND":
					OnS2F41_iJOBPROCESSCOMMAND(driverID, transaction); break;
				case "S2F41_iSPECIFICAREARWCOMMAND":
					OnS2F41_iSPECIFICAREARWCOMMAND(driverID, transaction); break;
				case "S2F41_JOBPROCESSCOMMAND":
					OnS2F41_JOBPROCESSCOMMAND(driverID, transaction); break;
				case "S2F41_PORTCOMMAND":
					OnS2F41_PORTCOMMAND(driverID, transaction); break;
				case "S3F0_ABORTTRANSACTION":
					OnS3F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S2F41_UNITCOMMAND":
					OnS2F41_UNITCOMMAND(driverID, transaction); break;
				case "S2F101_OPERATORCALLSEND":
					OnS2F101_OPERATORCALLSEND(driverID, transaction); break;
				case "S2F103_EOIDEQPPARAMETERCHANGEREQUEST":
					OnS2F103_EOIDEQPPARAMETERCHANGEREQUEST(driverID, transaction); break;
				case "S3F1_MASKINFORMAIONREQEUST":
					OnS3F1_MASKINFORMAIONREQEUST(driverID, transaction); break;
				case "S3F101_CASSETTEINFORMATIONSEND_TYPE1":
					OnS3F101_CASSETTEINFORMATIONSEND_TYPE1(driverID, transaction); break;
				case "S3F101_CASSETTEINFORMATIONSEND_TYPE2":
					OnS3F101_CASSETTEINFORMATIONSEND_TYPE2(driverID, transaction); break;
				case "S5F0_ABORTTRANSACTION":
					OnS5F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S3F101_CASSETTEINFORMATIONSEND_TYPE3":
					OnS3F101_CASSETTEINFORMATIONSEND_TYPE3(driverID, transaction); break;
				case "S3F101_CASSETTEINFORMATIONSEND_TYPE4":
					OnS3F101_CASSETTEINFORMATIONSEND_TYPE4(driverID, transaction); break;
				case "S3F101_iCASSETTEINFORMATIONSEND":
					OnS3F101_iCASSETTEINFORMATIONSEND(driverID, transaction); break;
				case "S5F2_ALARMREPORTREPLY":
					OnS5F2_ALARMREPORTREPLY(driverID, transaction); break;
				case "S6F0_ABORTTRANSACTION":
					OnS6F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S5F101_ALARMLISTREQUEST":
					OnS5F101_ALARMLISTREQUEST(driverID, transaction); break;
				case "S5F101_NOUSE":
					OnS5F101_NOUSE(driverID, transaction); break;
				case "S5F103_ALARMRESETREQEUST":
					OnS5F103_ALARMRESETREQEUST(driverID, transaction); break;
				case "S6F2_FDCTRACEDATAREPLY":
					OnS6F2_FDCTRACEDATAREPLY(driverID, transaction); break;
				case "S6F2_AUTOREPLY":
					OnS6F2_AUTOREPLY(driverID, transaction); break;
				case "S6F12_AUTOREPLY":
					OnS6F12_AUTOREPLY(driverID, transaction); break;
				case "S6F12_AUTOREPLY_1":
					OnS6F12_AUTOREPLY_1(driverID, transaction); break;
				case "S6F12_AUTOREPLY_2":
					OnS6F12_AUTOREPLY_2(driverID, transaction); break;
				case "S6F12_AUTOREPLY_3":
					OnS6F12_AUTOREPLY_3(driverID, transaction); break;
				case "S6F12_AUTOREPLY_4":
					OnS6F12_AUTOREPLY_4(driverID, transaction); break;
				case "S6F12_AUTOREPLY_5":
					OnS6F12_AUTOREPLY_5(driverID, transaction); break;
				case "S6F12_AUTOREPLY_6":
					OnS6F12_AUTOREPLY_6(driverID, transaction); break;
				case "S6F12_AUTOREPLY_7":
					OnS6F12_AUTOREPLY_7(driverID, transaction); break;
				case "S6F12_AUTOREPLY_8":
					OnS6F12_AUTOREPLY_8(driverID, transaction); break;
				case "S6F12_AUTOREPLY_9":
					OnS6F12_AUTOREPLY_9(driverID, transaction); break;
				case "S6F12_AUTOREPLY_10":
					OnS6F12_AUTOREPLY_10(driverID, transaction); break;
				case "S6F12_AUTOREPLY_11":
					OnS6F12_AUTOREPLY_11(driverID, transaction); break;
				case "S6F12_AUTOREPLY_12":
					OnS6F12_AUTOREPLY_12(driverID, transaction); break;
				case "S6F12_AUTOREPLY_13":
					OnS6F12_AUTOREPLY_13(driverID, transaction); break;
				case "S6F12_AUTOREPLY_14":
					OnS6F12_AUTOREPLY_14(driverID, transaction); break;
				case "S6F12_AUTOREPLY_15":
					OnS6F12_AUTOREPLY_15(driverID, transaction); break;
				case "S6F12_AUTOREPLY_17":
					OnS6F12_AUTOREPLY_17(driverID, transaction); break;
				case "S6F12_AUTOREPLY_18":
					OnS6F12_AUTOREPLY_18(driverID, transaction); break;
				case "S6F12_AUTOREPLY_19":
					OnS6F12_AUTOREPLY_19(driverID, transaction); break;
				case "S6F12_AUTOREPLY_16":
					OnS6F12_AUTOREPLY_16(driverID, transaction); break;
				case "S6F12_AUTOREPLY_20":
					OnS6F12_AUTOREPLY_20(driverID, transaction); break;
				case "S6F12_AUTOREPLY_21":
					OnS6F12_AUTOREPLY_21(driverID, transaction); break;
				case "S6F14_AUTOREPLY":
					OnS6F14_AUTOREPLY(driverID, transaction); break;
				case "S6F14_AUTOREPLY_1":
					OnS6F14_AUTOREPLY_1(driverID, transaction); break;
				case "S6F14_AUTOREPLY_2":
					OnS6F14_AUTOREPLY_2(driverID, transaction); break;
				case "S6F14_AUTOREPLY_3":
					OnS6F14_AUTOREPLY_3(driverID, transaction); break;
				case "S6F14_AUTOREPLY_4":
					OnS6F14_AUTOREPLY_4(driverID, transaction); break;
				case "S6F14_AUTOREPLY_5":
					OnS6F14_AUTOREPLY_5(driverID, transaction); break;
				case "S6F14_AUTOREPLY_6":
					OnS6F14_AUTOREPLY_6(driverID, transaction); break;
				case "S6F14_AUTOREPLY_8":
					OnS6F14_AUTOREPLY_8(driverID, transaction); break;
				case "S6F14_AUTOREPLY_9":
					OnS6F14_AUTOREPLY_9(driverID, transaction); break;
				case "S7F0_ABORTTRANSACTION":
					OnS7F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S6F14_AUTOREPLY_9_1":
					OnS6F14_AUTOREPLY_9_1(driverID, transaction); break;
				case "S6F14_AUTOREPLY_7":
					OnS6F14_AUTOREPLY_7(driverID, transaction); break;
				case "S6F102_LOTLISTREPLY":
					OnS6F102_LOTLISTREPLY(driverID, transaction); break;
				case "S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A":
					OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A(driverID, transaction); break;
				case "S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B":
					OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B(driverID, transaction); break;
				case "S7F25_RMSFORMATTEDPPIDDATAREQUEST":
					OnS7F25_RMSFORMATTEDPPIDDATAREQUEST(driverID, transaction); break;
				case "S7F25_NOUSE":
					OnS7F25_NOUSE(driverID, transaction); break;
				case "S9F0_ABORTTRANSACTION":
					OnS9F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S7F101_RMSCURRENTPPIDREQUEST":
					OnS7F101_RMSCURRENTPPIDREQUEST(driverID, transaction); break;
				case "S7F103_RMSPPIDEXISTENCEREQUEST":
					OnS7F103_RMSPPIDEXISTENCEREQUEST(driverID, transaction); break;
				case "S7F105_RMSPPIDCHANGETIMEREQUEST":
					OnS7F105_RMSPPIDCHANGETIMEREQUEST(driverID, transaction); break;
				case "S10F0_ABORTTRANSACTION":
					OnS10F0_ABORTTRANSACTION(driverID, transaction); break;
				case "S10F2_AUTOREPLY":
					OnS10F2_AUTOREPLY(driverID, transaction); break;
				case "S10F3_TERMINALDISPLAYSEND":
					OnS10F3_TERMINALDISPLAYSEND(driverID, transaction); break;
				case "S10F9_BROADCASTSEND":
					OnS10F9_BROADCASTSEND(driverID, transaction); break;
				case "S6F12":
					OnS6F12(driverID, transaction); break;
				case "S6F14":
					OnS6F14(driverID, transaction); break;
				case "S7F101_F1PSH01":
					OnS7F101_F1PSH01(driverID, transaction); break;
				case "S7F25_NOUSE_1":
					OnS7F25_NOUSE_1(driverID, transaction); break;
				case "S6F12_1":
					OnS6F12_1(driverID, transaction); break;

                default:
                    break;
            }

        }

        public void onSendComplete(string driverID, SECSTransaction transaction)
        {
            Console.Out.WriteLine("SendComplete : " + transaction.MessageName);
        }

        public void onTimeout(string driverID, WinSECS.timeout.SECSTimeout timeout)
        {
            Console.Out.WriteLine("Timeout : " + timeout.Type);
        }

        public void onUnknownReceived(string driverID, SECSTransaction transaction)
        {
            Console.Out.WriteLine("Unknown Received : " + transaction.MessageName);
        }

        #endregion

        #region Received Message Event Function
		void SECSWrapper_OnS1F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S1F0_ABORTTRANSACTION S1F0_ABORTTRANSACTION = new S1F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS1F1_AREYOUTHERE_TOEQP(string driverID, SECSTransaction transaction)
		{
			S1F1_AREYOUTHERE_TOEQP S1F1_AREYOUTHERE_TOEQP = new S1F1_AREYOUTHERE_TOEQP(transaction);
            //if(IsConnected)
            //{
            //    SECSTransaction trx = S1F2_ONLINEDATA.makeTransaction(false, "MDLN", "MODE");
            //    HotSend(trx);
            //}

		}

		void SECSWrapper_OnS1F2_IAMHERE(string driverID, SECSTransaction transaction)
		{
			S1F2_IAMHERE S1F2_IAMHERE = new S1F2_IAMHERE(transaction);
		}

		void SECSWrapper_OnS1F3_FDCEQPSTATUSREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F3_FDCEQPSTATUSREQUEST S1F3_FDCEQPSTATUSREQUEST = new S1F3_FDCEQPSTATUSREQUEST(transaction);
		}

		void SECSWrapper_OnS1F3_NOUSE(string driverID, SECSTransaction transaction)
		{
			S1F3_NOUSE S1F3_NOUSE = new S1F3_NOUSE(transaction);
		}

		void SECSWrapper_OnS1F3_NOUSE_1(string driverID, SECSTransaction transaction)
		{
			S1F3_NOUSE_1 S1F3_NOUSE_1 = new S1F3_NOUSE_1(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_21(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_21 S1F5_NOUSE_21 = new S1F5_NOUSE_21(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_22(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_22 S1F5_NOUSE_22 = new S1F5_NOUSE_22(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_2(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_2 S1F5_NOUSE_2 = new S1F5_NOUSE_2(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_32(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_32 S1F5_NOUSE_32 = new S1F5_NOUSE_32(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_6_TYPE1(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_6_TYPE1 S1F5_NOUSE_6_TYPE1 = new S1F5_NOUSE_6_TYPE1(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_6_TYPE2(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_6_TYPE2 S1F5_NOUSE_6_TYPE2 = new S1F5_NOUSE_6_TYPE2(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_6_TYPE3(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_6_TYPE3 S1F5_NOUSE_6_TYPE3 = new S1F5_NOUSE_6_TYPE3(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_3_TYPE1(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_3_TYPE1 S1F5_NOUSE_3_TYPE1 = new S1F5_NOUSE_3_TYPE1(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_3(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_3 S1F5_NOUSE_3 = new S1F5_NOUSE_3(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_33(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_33 S1F5_NOUSE_33 = new S1F5_NOUSE_33(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_11(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_11 S1F5_NOUSE_11 = new S1F5_NOUSE_11(transaction);
		}

		void SECSWrapper_OnS1F5_EQPSTATUSREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F5_EQPSTATUSREQUEST S1F5_EQPSTATUSREQUEST = new S1F5_EQPSTATUSREQUEST(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_4_TYPE1(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_4_TYPE1 S1F5_NOUSE_4_TYPE1 = new S1F5_NOUSE_4_TYPE1(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_4_TYPE2(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_4_TYPE2 S1F5_NOUSE_4_TYPE2 = new S1F5_NOUSE_4_TYPE2(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_31(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_31 S1F5_NOUSE_31 = new S1F5_NOUSE_31(transaction);
		}

		void SECSWrapper_OnS1F5_NOUSE_5(string driverID, SECSTransaction transaction)
		{
			S1F5_NOUSE_5 S1F5_NOUSE_5 = new S1F5_NOUSE_5(transaction);
		}

		void SECSWrapper_OnS1F5_iNOUSE_2(string driverID, SECSTransaction transaction)
		{
			S1F5_iNOUSE_2 S1F5_iNOUSE_2 = new S1F5_iNOUSE_2(transaction);
		}

		void SECSWrapper_OnS1F5_iNOUSE_32(string driverID, SECSTransaction transaction)
		{
			S1F5_iNOUSE_32 S1F5_iNOUSE_32 = new S1F5_iNOUSE_32(transaction);
		}

		void SECSWrapper_OnS1F5_iNOUSE_33(string driverID, SECSTransaction transaction)
		{
			S1F5_iNOUSE_33 S1F5_iNOUSE_33 = new S1F5_iNOUSE_33(transaction);
		}

		void SECSWrapper_OnS1F5_iNOUSE_4(string driverID, SECSTransaction transaction)
		{
			S1F5_iNOUSE_4 S1F5_iNOUSE_4 = new S1F5_iNOUSE_4(transaction);
		}

		void SECSWrapper_OnS1F5_iNOUSE_31(string driverID, SECSTransaction transaction)
		{
			S1F5_iNOUSE_31 S1F5_iNOUSE_31 = new S1F5_iNOUSE_31(transaction);
		}

		void SECSWrapper_OnS1F5_iEQPSTATUSREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F5_iEQPSTATUSREQUEST S1F5_iEQPSTATUSREQUEST = new S1F5_iEQPSTATUSREQUEST(transaction);
		}

		void SECSWrapper_OnS1F5_iNOUSE_5(string driverID, SECSTransaction transaction)
		{
			S1F5_iNOUSE_5 S1F5_iNOUSE_5 = new S1F5_iNOUSE_5(transaction);
		}

		void SECSWrapper_OnS1F11_FDCEQPSTATUSNAMELISTREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F11_FDCEQPSTATUSNAMELISTREQUEST S1F11_FDCEQPSTATUSNAMELISTREQUEST = new S1F11_FDCEQPSTATUSNAMELISTREQUEST(transaction);
		}

		void SECSWrapper_OnS1F11_NOUSE(string driverID, SECSTransaction transaction)
		{
			S1F11_NOUSE S1F11_NOUSE = new S1F11_NOUSE(transaction);
		}

		void SECSWrapper_OnS1F15_iOFFLINECHANGEREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F15_iOFFLINECHANGEREQUEST S1F15_iOFFLINECHANGEREQUEST = new S1F15_iOFFLINECHANGEREQUEST(transaction);
		}

		void SECSWrapper_OnS1F15_OFFLINECHANGEREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F15_OFFLINECHANGEREQUEST S1F15_OFFLINECHANGEREQUEST = new S1F15_OFFLINECHANGEREQUEST(transaction);
		}

		void SECSWrapper_OnS2F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S2F0_ABORTTRANSACTION S2F0_ABORTTRANSACTION = new S2F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS1F17_iONLINECHANGEREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F17_iONLINECHANGEREQUEST S1F17_iONLINECHANGEREQUEST = new S1F17_iONLINECHANGEREQUEST(transaction);
		}

		void SECSWrapper_OnS1F17_ONLINECHANGEREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F17_ONLINECHANGEREQUEST S1F17_ONLINECHANGEREQUEST = new S1F17_ONLINECHANGEREQUEST(transaction);
		}

		void SECSWrapper_OnS1F101_SVSTATUSREQUEST(string driverID, SECSTransaction transaction)
		{
			S1F101_SVSTATUSREQUEST S1F101_SVSTATUSREQUEST = new S1F101_SVSTATUSREQUEST(transaction);
		}

		void SECSWrapper_OnS2F15_ECMEQPNEWCONSTANTREQUEST(string driverID, SECSTransaction transaction)
		{
			S2F15_ECMEQPNEWCONSTANTREQUEST S2F15_ECMEQPNEWCONSTANTREQUEST = new S2F15_ECMEQPNEWCONSTANTREQUEST(transaction);
		}

		void SECSWrapper_OnS2F18_ONLINEDATETIMEDATA(string driverID, SECSTransaction transaction)
		{
			S2F18_ONLINEDATETIMEDATA S2F18_ONLINEDATETIMEDATA = new S2F18_ONLINEDATETIMEDATA(transaction);
		}

		void SECSWrapper_OnS2F23_FDCTRACEINITREQUEST(string driverID, SECSTransaction transaction)
		{
			S2F23_FDCTRACEINITREQUEST S2F23_FDCTRACEINITREQUEST = new S2F23_FDCTRACEINITREQUEST(transaction);
		}

		void SECSWrapper_OnS2F29_ECMEQPCONSTANTNAMELISTREQUEST(string driverID, SECSTransaction transaction)
		{
			S2F29_ECMEQPCONSTANTNAMELISTREQUEST S2F29_ECMEQPCONSTANTNAMELISTREQUEST = new S2F29_ECMEQPCONSTANTNAMELISTREQUEST(transaction);
		}

		void SECSWrapper_OnS2F41_GLASSCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_GLASSCOMMAND S2F41_GLASSCOMMAND = new S2F41_GLASSCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F41_iEQPCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_iEQPCOMMAND S2F41_iEQPCOMMAND = new S2F41_iEQPCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F41_iGLASSCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_iGLASSCOMMAND S2F41_iGLASSCOMMAND = new S2F41_iGLASSCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F41_iJOBPROCESSCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_iJOBPROCESSCOMMAND S2F41_iJOBPROCESSCOMMAND = new S2F41_iJOBPROCESSCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F41_iSPECIFICAREARWCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_iSPECIFICAREARWCOMMAND S2F41_iSPECIFICAREARWCOMMAND = new S2F41_iSPECIFICAREARWCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F41_JOBPROCESSCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_JOBPROCESSCOMMAND S2F41_JOBPROCESSCOMMAND = new S2F41_JOBPROCESSCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F41_PORTCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_PORTCOMMAND S2F41_PORTCOMMAND = new S2F41_PORTCOMMAND(transaction);
		}

		void SECSWrapper_OnS3F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S3F0_ABORTTRANSACTION S3F0_ABORTTRANSACTION = new S3F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS2F41_UNITCOMMAND(string driverID, SECSTransaction transaction)
		{
			S2F41_UNITCOMMAND S2F41_UNITCOMMAND = new S2F41_UNITCOMMAND(transaction);
		}

		void SECSWrapper_OnS2F101_OPERATORCALLSEND(string driverID, SECSTransaction transaction)
		{
			S2F101_OPERATORCALLSEND S2F101_OPERATORCALLSEND = new S2F101_OPERATORCALLSEND(transaction);
		}

		void SECSWrapper_OnS2F103_EOIDEQPPARAMETERCHANGEREQUEST(string driverID, SECSTransaction transaction)
		{
			S2F103_EOIDEQPPARAMETERCHANGEREQUEST S2F103_EOIDEQPPARAMETERCHANGEREQUEST = new S2F103_EOIDEQPPARAMETERCHANGEREQUEST(transaction);
		}

		void SECSWrapper_OnS3F1_MASKINFORMAIONREQEUST(string driverID, SECSTransaction transaction)
		{
			S3F1_MASKINFORMAIONREQEUST S3F1_MASKINFORMAIONREQEUST = new S3F1_MASKINFORMAIONREQEUST(transaction);
		}

		void SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE1(string driverID, SECSTransaction transaction)
		{
			S3F101_CASSETTEINFORMATIONSEND_TYPE1 S3F101_CASSETTEINFORMATIONSEND_TYPE1 = new S3F101_CASSETTEINFORMATIONSEND_TYPE1(transaction);
		}

		void SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE2(string driverID, SECSTransaction transaction)
		{
			S3F101_CASSETTEINFORMATIONSEND_TYPE2 S3F101_CASSETTEINFORMATIONSEND_TYPE2 = new S3F101_CASSETTEINFORMATIONSEND_TYPE2(transaction);
		}

		void SECSWrapper_OnS5F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S5F0_ABORTTRANSACTION S5F0_ABORTTRANSACTION = new S5F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE3(string driverID, SECSTransaction transaction)
		{
			S3F101_CASSETTEINFORMATIONSEND_TYPE3 S3F101_CASSETTEINFORMATIONSEND_TYPE3 = new S3F101_CASSETTEINFORMATIONSEND_TYPE3(transaction);
		}

		void SECSWrapper_OnS3F101_CASSETTEINFORMATIONSEND_TYPE4(string driverID, SECSTransaction transaction)
		{
			S3F101_CASSETTEINFORMATIONSEND_TYPE4 S3F101_CASSETTEINFORMATIONSEND_TYPE4 = new S3F101_CASSETTEINFORMATIONSEND_TYPE4(transaction);
		}

		void SECSWrapper_OnS3F101_iCASSETTEINFORMATIONSEND(string driverID, SECSTransaction transaction)
		{
			S3F101_iCASSETTEINFORMATIONSEND S3F101_iCASSETTEINFORMATIONSEND = new S3F101_iCASSETTEINFORMATIONSEND(transaction);
		}

		void SECSWrapper_OnS5F2_ALARMREPORTREPLY(string driverID, SECSTransaction transaction)
		{
			S5F2_ALARMREPORTREPLY S5F2_ALARMREPORTREPLY = new S5F2_ALARMREPORTREPLY(transaction);
		}

		void SECSWrapper_OnS6F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S6F0_ABORTTRANSACTION S6F0_ABORTTRANSACTION = new S6F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS5F101_ALARMLISTREQUEST(string driverID, SECSTransaction transaction)
		{
			S5F101_ALARMLISTREQUEST S5F101_ALARMLISTREQUEST = new S5F101_ALARMLISTREQUEST(transaction);
		}

		void SECSWrapper_OnS5F101_NOUSE(string driverID, SECSTransaction transaction)
		{
			S5F101_NOUSE S5F101_NOUSE = new S5F101_NOUSE(transaction);
		}

		void SECSWrapper_OnS5F103_ALARMRESETREQEUST(string driverID, SECSTransaction transaction)
		{
			S5F103_ALARMRESETREQEUST S5F103_ALARMRESETREQEUST = new S5F103_ALARMRESETREQEUST(transaction);
		}

		void SECSWrapper_OnS6F2_FDCTRACEDATAREPLY(string driverID, SECSTransaction transaction)
		{
			S6F2_FDCTRACEDATAREPLY S6F2_FDCTRACEDATAREPLY = new S6F2_FDCTRACEDATAREPLY(transaction);
		}

		void SECSWrapper_OnS6F2_AUTOREPLY(string driverID, SECSTransaction transaction)
		{
			S6F2_AUTOREPLY S6F2_AUTOREPLY = new S6F2_AUTOREPLY(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY S6F12_AUTOREPLY = new S6F12_AUTOREPLY(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_1(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_1 S6F12_AUTOREPLY_1 = new S6F12_AUTOREPLY_1(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_2(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_2 S6F12_AUTOREPLY_2 = new S6F12_AUTOREPLY_2(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_3(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_3 S6F12_AUTOREPLY_3 = new S6F12_AUTOREPLY_3(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_4(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_4 S6F12_AUTOREPLY_4 = new S6F12_AUTOREPLY_4(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_5(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_5 S6F12_AUTOREPLY_5 = new S6F12_AUTOREPLY_5(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_6(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_6 S6F12_AUTOREPLY_6 = new S6F12_AUTOREPLY_6(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_7(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_7 S6F12_AUTOREPLY_7 = new S6F12_AUTOREPLY_7(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_8(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_8 S6F12_AUTOREPLY_8 = new S6F12_AUTOREPLY_8(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_9(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_9 S6F12_AUTOREPLY_9 = new S6F12_AUTOREPLY_9(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_10(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_10 S6F12_AUTOREPLY_10 = new S6F12_AUTOREPLY_10(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_11(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_11 S6F12_AUTOREPLY_11 = new S6F12_AUTOREPLY_11(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_12(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_12 S6F12_AUTOREPLY_12 = new S6F12_AUTOREPLY_12(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_13(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_13 S6F12_AUTOREPLY_13 = new S6F12_AUTOREPLY_13(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_14(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_14 S6F12_AUTOREPLY_14 = new S6F12_AUTOREPLY_14(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_15(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_15 S6F12_AUTOREPLY_15 = new S6F12_AUTOREPLY_15(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_17(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_17 S6F12_AUTOREPLY_17 = new S6F12_AUTOREPLY_17(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_18(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_18 S6F12_AUTOREPLY_18 = new S6F12_AUTOREPLY_18(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_19(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_19 S6F12_AUTOREPLY_19 = new S6F12_AUTOREPLY_19(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_16(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_16 S6F12_AUTOREPLY_16 = new S6F12_AUTOREPLY_16(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_20(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_20 S6F12_AUTOREPLY_20 = new S6F12_AUTOREPLY_20(transaction);
		}

		void SECSWrapper_OnS6F12_AUTOREPLY_21(string driverID, SECSTransaction transaction)
		{
			S6F12_AUTOREPLY_21 S6F12_AUTOREPLY_21 = new S6F12_AUTOREPLY_21(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY S6F14_AUTOREPLY = new S6F14_AUTOREPLY(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_1(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_1 S6F14_AUTOREPLY_1 = new S6F14_AUTOREPLY_1(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_2(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_2 S6F14_AUTOREPLY_2 = new S6F14_AUTOREPLY_2(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_3(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_3 S6F14_AUTOREPLY_3 = new S6F14_AUTOREPLY_3(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_4(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_4 S6F14_AUTOREPLY_4 = new S6F14_AUTOREPLY_4(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_5(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_5 S6F14_AUTOREPLY_5 = new S6F14_AUTOREPLY_5(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_6(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_6 S6F14_AUTOREPLY_6 = new S6F14_AUTOREPLY_6(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_8(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_8 S6F14_AUTOREPLY_8 = new S6F14_AUTOREPLY_8(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_9(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_9 S6F14_AUTOREPLY_9 = new S6F14_AUTOREPLY_9(transaction);
		}

		void SECSWrapper_OnS7F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S7F0_ABORTTRANSACTION S7F0_ABORTTRANSACTION = new S7F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_9_1(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_9_1 S6F14_AUTOREPLY_9_1 = new S6F14_AUTOREPLY_9_1(transaction);
		}

		void SECSWrapper_OnS6F14_AUTOREPLY_7(string driverID, SECSTransaction transaction)
		{
			S6F14_AUTOREPLY_7 S6F14_AUTOREPLY_7 = new S6F14_AUTOREPLY_7(transaction);
		}

		void SECSWrapper_OnS6F102_LOTLISTREPLY(string driverID, SECSTransaction transaction)
		{
			S6F102_LOTLISTREPLY S6F102_LOTLISTREPLY = new S6F102_LOTLISTREPLY(transaction);
		}

		void SECSWrapper_OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A(string driverID, SECSTransaction transaction)
		{
			S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A = new S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_A(transaction);
		}

		void SECSWrapper_OnS7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B(string driverID, SECSTransaction transaction)
		{
			S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B = new S7F23_RMSFORMATTEDPPIDCHANGEREQUEST_B(transaction);
		}

		void SECSWrapper_OnS7F25_RMSFORMATTEDPPIDDATAREQUEST(string driverID, SECSTransaction transaction)
		{
			S7F25_RMSFORMATTEDPPIDDATAREQUEST S7F25_RMSFORMATTEDPPIDDATAREQUEST = new S7F25_RMSFORMATTEDPPIDDATAREQUEST(transaction);
		}

		void SECSWrapper_OnS7F25_NOUSE(string driverID, SECSTransaction transaction)
		{
			S7F25_NOUSE S7F25_NOUSE = new S7F25_NOUSE(transaction);
		}

		void SECSWrapper_OnS9F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S9F0_ABORTTRANSACTION S9F0_ABORTTRANSACTION = new S9F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS7F101_RMSCURRENTPPIDREQUEST(string driverID, SECSTransaction transaction)
		{
			S7F101_RMSCURRENTPPIDREQUEST S7F101_RMSCURRENTPPIDREQUEST = new S7F101_RMSCURRENTPPIDREQUEST(transaction);
		}

		void SECSWrapper_OnS7F103_RMSPPIDEXISTENCEREQUEST(string driverID, SECSTransaction transaction)
		{
			S7F103_RMSPPIDEXISTENCEREQUEST S7F103_RMSPPIDEXISTENCEREQUEST = new S7F103_RMSPPIDEXISTENCEREQUEST(transaction);
		}

		void SECSWrapper_OnS7F105_RMSPPIDCHANGETIMEREQUEST(string driverID, SECSTransaction transaction)
		{
			S7F105_RMSPPIDCHANGETIMEREQUEST S7F105_RMSPPIDCHANGETIMEREQUEST = new S7F105_RMSPPIDCHANGETIMEREQUEST(transaction);
		}

		void SECSWrapper_OnS10F0_ABORTTRANSACTION(string driverID, SECSTransaction transaction)
		{
			S10F0_ABORTTRANSACTION S10F0_ABORTTRANSACTION = new S10F0_ABORTTRANSACTION(transaction);
		}

		void SECSWrapper_OnS10F2_AUTOREPLY(string driverID, SECSTransaction transaction)
		{
			S10F2_AUTOREPLY S10F2_AUTOREPLY = new S10F2_AUTOREPLY(transaction);
		}

		void SECSWrapper_OnS10F3_TERMINALDISPLAYSEND(string driverID, SECSTransaction transaction)
		{
			S10F3_TERMINALDISPLAYSEND S10F3_TERMINALDISPLAYSEND = new S10F3_TERMINALDISPLAYSEND(transaction);
		}

		void SECSWrapper_OnS10F9_BROADCASTSEND(string driverID, SECSTransaction transaction)
		{
			S10F9_BROADCASTSEND S10F9_BROADCASTSEND = new S10F9_BROADCASTSEND(transaction);
		}

		void SECSWrapper_OnS6F12(string driverID, SECSTransaction transaction)
		{
			S6F12 S6F12 = new S6F12(transaction);
		}

		void SECSWrapper_OnS6F14(string driverID, SECSTransaction transaction)
		{
			S6F14 S6F14 = new S6F14(transaction);
		}

		void SECSWrapper_OnS7F101_F1PSH01(string driverID, SECSTransaction transaction)
		{
			S7F101_F1PSH01 S7F101_F1PSH01 = new S7F101_F1PSH01(transaction);
		}

		void SECSWrapper_OnS7F25_NOUSE_1(string driverID, SECSTransaction transaction)
		{
			S7F25_NOUSE_1 S7F25_NOUSE_1 = new S7F25_NOUSE_1(transaction);
		}

		void SECSWrapper_OnS6F12_1(string driverID, SECSTransaction transaction)
		{
			S6F12_1 S6F12_1 = new S6F12_1(transaction);
		}


        #endregion


        public void onIllegal(string driverID, SECSTransaction transaction, string illegalMessage)
        {
            
        }

        public void onSendFailed(string driverID, SECSTransaction transaction)
        {
           
        }

        public void Dispose()
        {
            Terminate();
        }
    }
}
