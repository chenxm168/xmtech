using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
   public class RecipeListBody
    {
       [XmlElement]
       public string MACHINENAME;


    }

    public class MachineRecipeList
    {
        [XmlElement]
        public MachineRecipeInfo[] MACHINERECIPE;
    }

   public class MachineRecipeInfo
   {
       [XmlElement]
       public string MACHINERECIPENAME;

       [XmlElement]
       public UnitRecipeList UNITLIST;
   }

    public class UnitRecipeList
    {
        [XmlElement]
        public UnitRecipe[] UNIT;
    }

    public class UnitRecipe
    {
        [XmlElement]
        public string UNITNAME;

        [XmlElement]
        public string LOCALRECIPENAME;
    }
}
