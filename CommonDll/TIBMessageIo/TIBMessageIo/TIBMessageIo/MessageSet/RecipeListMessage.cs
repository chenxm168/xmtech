using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
     [XmlRoot(ElementName = "Message")]
   public class RecipeListMessage:AbstractMessage
    {

         [XmlElement]
         public RecipeListBody Body;

         public static RecipeListMessage getRecipeListResponseMessage()
         {
             RecipeListMessage msg = new RecipeListMessage();
             msg.Header.MESSAGENAME = "RecipeListResponse";
             msg.Body = new RecipeListBody();
             msg.Init();
             msg.Body.MACHINENAME = StaticVarible.MachineID;
             return msg;
         }

    }
}
