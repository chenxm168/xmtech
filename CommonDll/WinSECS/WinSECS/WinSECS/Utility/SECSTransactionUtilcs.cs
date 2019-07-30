using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSECS.global;
using WinSECS.structure;

namespace WinSECS.Utility
{
    internal class SECSTransactionUtilcs
    {
        public static List<SECS1Block> ToSECS1BlockList(SECSTransaction trx, bool isHost)
        {
            SECS1Block block;
            byte[] buffer;
            if ((trx.Header == null) || (trx.Header.Length != 10))
            {
                throw new Exception("Header is NULL or Invalid Length.");
            }
            List<SECS1Block> list = new List<SECS1Block>();
            if ((trx.Body == null) || (trx.Body.Length == 0))
            {
                block = new SECS1Block();
                buffer = new byte[10];
                Array.Copy(trx.Header, buffer, 10);
                block.Header = buffer;
                if (!isHost)
                {
                    block.Header[0] = (byte)(block.Header[0] | 0x80);
                }
                block.Header[4] = 0x80;
                block.Header[5] = 1;
                block.Text = new byte[0];
                list.Add(block);
            }
            else
            {
                ushort num = 1;
                int sourceIndex = 0;
                while (sourceIndex < trx.Body.Length)
                {
                    byte[] buffer3;
                    block = new SECS1Block();
                    buffer = new byte[10];
                    Array.Copy(trx.Header, buffer, 10);
                    block.Header = buffer;
                    if (!isHost)
                    {
                        block.Header[0] = (byte)(block.Header[0] | 0x80);
                    }
                    byte[] bytes = BigEndianBitConverter.GetBytes(num);
                    block.Header[4] = bytes[0];
                    block.Header[5] = bytes[1];
                    if ((sourceIndex + 0xea) < trx.Body.Length)
                    {
                        buffer3 = new byte[0xea];
                        Array.Copy(trx.Body, sourceIndex, buffer3, 0, buffer3.Length);
                        block.Text = buffer3;
                        sourceIndex += buffer3.Length;
                    }
                    else
                    {
                        buffer3 = new byte[trx.Body.Length - sourceIndex];
                        Array.Copy(trx.Body, sourceIndex, buffer3, 0, buffer3.Length);
                        block.Text = buffer3;
                        sourceIndex += buffer3.Length;
                    }
                    list.Add(block);
                    num = (ushort)(num + 1);
                }
                list[list.Count - 1].Header[4] = (byte)(list[list.Count - 1].Header[4] | 0x80);
            }
            foreach (SECS1Block block2 in list)
            {
                block2.CheckSum = BigEndianBitConverter.GetBytes(block2.MakeCheckSum());
            }
            return list;
        }
    }
}
