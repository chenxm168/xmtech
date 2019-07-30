using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace WinSECS.callback
{
    [ComVisible(false)]
    public class Received : AbstractCallback
    {
        public Received(object object_Renamed)
            : base(object_Renamed)
        {
        }

        public override int getType()
        {
            return 0;
        }
    }
}
