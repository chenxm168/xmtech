using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIBMessageIo.ConstantDef
{
   public class ConstrantType1
    {
       public const string PORT_TRANSFER_STATE_LR = "LR";
       public const string PORT_TRANSFER_STATE_LC = "LC";
       public const string PORT_TRANSFER_STATE_UR = "UR";
       //public const string PORT_TRANSFER_STATE_LR = "UC";

       public const string PORT_ACCESSMODE_AUTO = "UC";



    }

  public  enum PORT_TRANSER_STATE
  {
      LR,LC,UR,UC,PL
  }

  public enum MACHINE_CONTROL_STATE
  {
      OFFLINE, REMOTE, LOCAL
  }

  public enum PORT_TYPE
  {
      PB,PL,PU,PP,BL,BU,BB
  }

  public enum PORT_USE_TYPE
  {
      TFT,CF,DM,QC,NG,RW,RP,A,B
  }

  public enum PORT_ENABLE_MODE
  {
      ENABLE, DISABLE
  }

  public enum PORT_STATE
  {
      UP, DOWN
  }

  public enum PORT_ACCESS_MODE
  {
      AUTO, MANUAL
  }

  public enum MACHINE_STATE
  {
      IDLE,RUN,DOWN,MANUAL
  }

}
