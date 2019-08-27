using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISCParser
{
   class ListeChamps : Dictionary<String, DateTime>
   {
      public ListeChamps()
      {
      }

      public ListeChamps(string field)
      {
         RemplirListe(field);
      }

      public void RemplirListe(string field)
      {
         Clear();
         string[] listeSepare = field.Split(' ');
         foreach (string entiteSepare in listeSepare)
         {
            if (!ContainsKey(entiteSepare))
            {
               Add(entiteSepare, DateTime.MinValue);
            }
         }
      }
   }
}
