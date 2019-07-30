using System;
using System.Collections.Generic;
using System.Text;
using WinSECS.structure;

namespace WinSECS
{
    public class S1F6_EQPLOCALSTATUSREPLY_TOOL_COUNT
    {
        private ListFormat ownerList = new ListFormat();

		private String toolid= "";
		private String mcmd= "";
		private String eqst= "";
		private String eqstate= "";
		private String ppid= "";
		private String glasssize= "";
		private String direction= "";
		private String lotkind= "";
		private String tact= "";
		private String stdcell= "";
		private String glasstype= "";
		private String cutrule= "";

        public S1F6_EQPLOCALSTATUSREPLY_TOOL_COUNT(String toolid, String mcmd, String eqst, String eqstate, String ppid, String glasssize, String direction, String lotkind, String tact, String stdcell, String glasstype, String cutrule)
        {
			this.toolid = toolid;
			this.mcmd = mcmd;
			this.eqst = eqst;
			this.eqstate = eqstate;
			this.ppid = ppid;
			this.glasssize = glasssize;
			this.direction = direction;
			this.lotkind = lotkind;
			this.tact = tact;
			this.stdcell = stdcell;
			this.glasstype = glasstype;
			this.cutrule = cutrule;

        }

        public ListFormat getMessage(bool isNoPadding)
        {
            ownerList.Length = 2;

			if (isNoPadding)
				ownerList.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(toolid).Length, "TOOLID", toolid);
			else
				ownerList.add(AsciiFormat.TYPE, 9, "TOOLID", toolid);
			ListFormat listNode_0 = ownerList.add(ListFormat.TYPE, 11, "", "") as ListFormat;
			String[] sArray =  mcmd.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "MCMD", mcmd);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "MCMD", mcmd);
			sArray =  eqst.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "EQST", eqst);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "EQST", eqst);
			sArray =  eqstate.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint1Format.TYPE, sArray.Length, "EQSTATE", eqstate);
			else
				listNode_0.add(Uint1Format.TYPE, 1, "EQSTATE", eqstate);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(ppid).Length, "PPID", ppid);
			else
				listNode_0.add(AsciiFormat.TYPE, 20, "PPID", ppid);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasssize).Length, "GLASSSIZE", glasssize);
			else
				listNode_0.add(AsciiFormat.TYPE, 2, "GLASSSIZE", glasssize);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(direction).Length, "DIRECTION", direction);
			else
				listNode_0.add(AsciiFormat.TYPE, 16, "DIRECTION", direction);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(lotkind).Length, "LOTKIND", lotkind);
			else
				listNode_0.add(AsciiFormat.TYPE, 16, "LOTKIND", lotkind);
			sArray =  tact.Split(' ');
			if (isNoPadding)
				listNode_0.add(Uint2Format.TYPE, sArray.Length, "TACT", tact);
			else
				listNode_0.add(Uint2Format.TYPE, 1, "TACT", tact);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(stdcell).Length, "STDCELL", stdcell);
			else
				listNode_0.add(AsciiFormat.TYPE, 2, "STDCELL", stdcell);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(glasstype).Length, "GLASSTYPE", glasstype);
			else
				listNode_0.add(AsciiFormat.TYPE, 2, "GLASSTYPE", glasstype);
			if (isNoPadding)
				listNode_0.add(AsciiFormat.TYPE, Encoding.GetEncoding("ks_c_5601-1987").GetBytes(cutrule).Length, "CUTRULE", cutrule);
			else
				listNode_0.add(AsciiFormat.TYPE, 16, "CUTRULE", cutrule);

            return ownerList;
        }

    }
}