using EQPIO.Common;

namespace EQPIO.Controller.Proxy
{
	public struct PLCMap
	{
		public EQPIO.Common.ConnectionInfo connectionInfo;

		public BlockMap blockMap;

		public DataGathering dataGathering;

		public Transaction transaction;

		public DataGathering vEQPdataGathering;

		public Transaction vEQPtransaction;

		public bool bDirectAccess;

		public PLCMap(EQPIO.Common.ConnectionInfo conn, BlockMap map, DataGathering data, Transaction trx, bool directAccess)
		{
			connectionInfo = conn;
			blockMap = map;
			dataGathering = data;
			transaction = trx;
			vEQPdataGathering = data;
			vEQPtransaction = trx;
			bDirectAccess = directAccess;
		}

		public PLCMap(EQPIO.Common.ConnectionInfo conn, BlockMap map, DataGathering data, Transaction trx, DataGathering vEQPGathering, Transaction vEQPTransaction, bool directAccess)
		{
			connectionInfo = conn;
			blockMap = map;
			dataGathering = data;
			transaction = trx;
			vEQPdataGathering = vEQPGathering;
			vEQPtransaction = vEQPTransaction;
			bDirectAccess = directAccess;
		}
	}
}
