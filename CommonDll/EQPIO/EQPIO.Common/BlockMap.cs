
namespace EQPIO.Common
{
    using System;
    using System.Collections.Generic;

    public class BlockMap
    {
        private List<EQPIO.Common.Block> block = new List<EQPIO.Common.Block>();

        public void Add(EQPIO.Common.Block newBlock)
        {
            this.block.Add(newBlock);
        }

        public List<EQPIO.Common.Block> Block
        {
            get
            {
                return this.block;
            }
            set
            {
                this.block = value;
            }
        }
    }
}
