
namespace HF.BC.Tool.EIPDriver.Driver.Compiler
{
    using HF.BC.Tool.EIPDriver;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class EIPMapCompiler
    {
        private BlockCompiler blockCompiler = new BlockCompiler();
        private DataGatherCompiler dataGatherCompiler = new DataGatherCompiler();
        private XmlDocument document;
        private ItemGroupCompiler itemGroupCompiler = new ItemGroupCompiler();
        private TagCompiler tagCompiler = new TagCompiler();
        private TrxCompiler trxCompiler = new TrxCompiler();

        public void CheckAll()
        {
            this.Clean();
            this.itemGroupCompiler.CheckItemGroupUnique();
            this.itemGroupCompiler.CheckItemUnique();
            this.itemGroupCompiler.CheckAllItemAttributes();
            this.blockCompiler.CheckItemAttributes();
            this.blockCompiler.CheckItemUnique();
            this.blockCompiler.CheckBlockUnique();
            this.blockCompiler.CheckItemGroupUndefine();
            this.tagCompiler.CheckBlockAttributes();
            this.tagCompiler.CheckBlockUnique();
            this.tagCompiler.CheckTagUnique();
            this.tagCompiler.CheckBlockUndefine();
            this.dataGatherCompiler.CheckMultiTagUnique();
            this.dataGatherCompiler.CheckMultiTagAttribute();
            this.dataGatherCompiler.CheckTagUndefine();
            this.trxCompiler.CheckTrxUnique();
            this.trxCompiler.CheckTrxAttributes();
            this.trxCompiler.CheckTrxKey();
            this.trxCompiler.CheckTrxUniqueKey();
            this.trxCompiler.CheckTagUnique();
            this.trxCompiler.CheckBlockUndefine();
        }

        public void Clean()
        {
            this.itemGroupCompiler.Clean();
            this.blockCompiler.Clean();
            this.tagCompiler.Clean();
            this.dataGatherCompiler.Clean();
            this.trxCompiler.Clean();
        }

        private int CompareCompileResult(CompileResult result1, CompileResult result2)
        {
            int num = result1.Severity.CompareTo(result2.Severity);
            if (num == 0)
            {
                string str = result1.Resource.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                string strB = result2.Resource.Attributes[EIPConst.ATTRIBUTE_NAME].Value;
                return str.CompareTo(strB);
            }
            return num;
        }

        public void Load(string filename)
        {
            this.document = new XmlDocument();
            this.document.Load(filename);
            this.Load(this.document);
        }

        public void Load(XmlDocument document)
        {
            this.document = document;
            XmlNode documentElement = this.document.DocumentElement;
            this.itemGroupCompiler.Configure(documentElement, documentElement.SelectSingleNode(EIPConst.XPATH_ITEMGROUPCOLLECTION));
            this.blockCompiler.Configure(documentElement, documentElement.SelectSingleNode(EIPConst.XPATH_BLOCKMAP));
            this.tagCompiler.Configure(documentElement, documentElement.SelectSingleNode(EIPConst.XPATH_TAGMAP));
            this.dataGatherCompiler.Configure(documentElement, documentElement.SelectSingleNode(EIPConst.XPATH_DATAGATHERING));
            this.trxCompiler.Configure(documentElement, documentElement.SelectSingleNode(EIPConst.XPATH_TRX));
            this.Clean();
        }

        public IEnumerable<CompileResult> CompileResultCollection
        {
            get
            {
                List<CompileResult> list = new List<CompileResult>();
                list.AddRange(this.itemGroupCompiler.CompileResultCollection);
                list.AddRange(this.blockCompiler.CompileResultCollection);
                list.AddRange(this.tagCompiler.CompileResultCollection);
                list.AddRange(this.dataGatherCompiler.CompileResultCollection);
                list.AddRange(this.trxCompiler.CompileResultCollection);
                list.Sort(new Comparison<CompileResult>(this.CompareCompileResult));
                return list;
            }
        }

        public int ErrorCount
        {
            get
            {
                return ((((this.itemGroupCompiler.ErrorCount + this.blockCompiler.ErrorCount) + this.tagCompiler.ErrorCount) + this.dataGatherCompiler.ErrorCount) + this.trxCompiler.ErrorCount);
            }
        }

        public int WarnCount
        {
            get
            {
                return ((((this.itemGroupCompiler.WarnCount + this.blockCompiler.WarnCount) + this.tagCompiler.WarnCount) + this.dataGatherCompiler.WarnCount) + this.trxCompiler.WarnCount);
            }
        }
    }
}
