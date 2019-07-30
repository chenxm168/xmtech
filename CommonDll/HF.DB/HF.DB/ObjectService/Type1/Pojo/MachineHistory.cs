using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HF.DB;

namespace HF.DB.ObjectService.Type1.Pojo
{
  public  class MachineHistory
    {
      [ColummAttribute(PrimaryKey=true)]
      public string ObjectNo
      {
          get;
          set;
      }

      public string EventName
      {
          get;
          set;
      }

      public string HistoryTime
      {
          get;
          set;
      }

      public string EquipmentName
      {
          get;
          set;
      }

      public string MachineName
      {
          get;
          set;
      }

      public int Capacity
      {
          get;
          set;
      }

      public string CimMode
      {
          get;
          set;
      }

      public int CommType
      {
          get;
          set;
      }

      public string CurrentStatusTime
      {
          get;
          set;
      }

      public int HighLimit
      {
          get;
          set;
      }

      public int LocalNo
      {
          get;
          set;
      }

      public int LowLimit
      {
          get;
          set;
      }

      public string MachineAlive
      {
          get;
          set;
      }

      public string MachineAutoMode
      {
          get;
          set;
      }

      public string MachineMode
      {
          get;
          set;
      }

      public string MachineStatus
      {
          get;
          set;
      }

      public string MachineType
      {
          get;
          set;
      }

      public string OldMachineStatus
      {
          get;
          set;
      }

      public string OnlineControlStatus
      {
          get;
          set;
      }

      public string PMCode
      {
          get;
          set;
      }

      public int RecipeFigureNumber
      {
          get;
          set;
      }

      public string RecipeName
      {
          get;
          set;
      }

      public string StatusCode
      {
          get;
          set;
      }

      public string Description
      {
          get;
          set;
      }
    }
}
