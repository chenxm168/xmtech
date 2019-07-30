using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB.ObjectService.Type1.Pojo;

namespace HF.DB.ObjectService.Type1.Service
{
   public interface IEquipmentService
    {
       Equipment FindByEQName(string eq_name);
       Equipment[] FindAll();
       int UpdateEQStatus(string eq_name, string status);
       int UpdateEQControlStatus(string eq_name, string status);
    }
}
