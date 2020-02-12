using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace INDAABIN.DI.AIFDIA.Aplicación
{
    public class MetodosGenerales
    {
        public ListItemCollection OpcionNula(ListItemCollection ItemCollection)
        {
            ListItem Item = new ListItem(" Seleccionar ", "0");
            if (!ItemCollection.Contains(Item))
                ItemCollection.Insert(0, Item);
            return ItemCollection;
        }
    }
}