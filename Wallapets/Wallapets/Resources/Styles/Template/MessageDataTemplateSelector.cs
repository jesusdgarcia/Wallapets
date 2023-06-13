using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallapets.Resources.Styles.Template
{
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SenderMessageTemplate { get; set; }
        public DataTemplate ReceiverMessageTemplate { get; set; }

        /// <summary>
        /// Metodo que decide que template se va a utilizar, 
        /// si el uid del mensaje es igual al token del usuario 
        /// los chat del usuario se mostraran a la derecha y los 
        /// del otro usuario a la izquierda
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = (clsMensaje)item;

            if (message.Uid == Preferences.Get("UserToken",""))
                return ReceiverMessageTemplate;

            return SenderMessageTemplate;
        }
    }
}
