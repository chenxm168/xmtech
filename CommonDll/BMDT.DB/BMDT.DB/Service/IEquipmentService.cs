using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMDT.DB.Pojo;

namespace BMDT.DB.Service
{
   public interface IEquipmentService
    {

        Equipment FindByEQName(string eq_name);
        Equipment FindOne();
        Equipment[] FindAll();
        int UpdateEQStatus(string eq_name, string status);
        int UpdateEQControlStatus(string eq_name, string status);

        int Update(Equipment eq);
    }
}
