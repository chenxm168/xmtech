using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinSECS.callback
{
    [ComVisible(false)]
    public class Connected : AbstractCallback
    {
        public Connected(object object_Renamed)
            : base(object_Renamed)
        {
        }

        public override int getType()
        {
            return 1;
        }
    }
}
