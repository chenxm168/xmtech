using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.LAN
{
    public delegate void  ReaderCallback(object sender, object[] ars);
   public interface IReader
    {
       void Connect(string ip, int port);
       void ConnectAsyn(string ip, int port);
       void Disconnection();
       //void StartRead(ReaderCallback callbakc);
       //void StopRead();
       //void ReadOnce(ReaderCallback callback);
       //void ReadUnitSuccessOnce(ReaderCallback callback);
    }
}
