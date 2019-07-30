using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSECS.structure;

namespace WinSECS.timeout
{
    public interface ISECSTimout
    {
        object Clone();
        string ToString();

        int Id { get; }

        ISECSTransaction Message { get; set; }

        string Type { get; }
    }
}
