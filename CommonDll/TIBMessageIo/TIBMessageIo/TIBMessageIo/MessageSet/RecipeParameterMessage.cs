using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TIBMessageIo.MessageSet
{
    [XmlRoot(ElementName = "Message")]
   public class RecipeParameterMessage:AbstractMessage
    {
        [XmlElement]
        public RecipeParameterBody Body;

        private RecipeParameterMessage getInstance(string messageName)
        {
            RecipeParameterMessage msg = new RecipeParameterMessage();
            msg.Body = new RecipeParameterBody();
            msg.Init();
            msg.Header.MESSAGENAME = messageName;
            return msg;
        }

        public RecipeParameterMessage getRecipeParameterChangeRequestMessage()
        {
            return getInstance("RecipeParameterChangeRequest");
        }
    }
}
