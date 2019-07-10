using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISCParser
{
   class Formations : Dictionary<String, DateTime>
   {
      public Formations()
      {
      }

      public Formations(string field)
      {
         RempliListeDeFormations(field);
      }

      public void RempliListeDeFormations(string field)
      {
         Clear();
         string[] arrayFormations = field.Split(' ');
         foreach (string formation in arrayFormations)
         {
            if (!ContainsKey(formation))
            {
               Add(formation, DateTime.MinValue);
            }
         }
      }

      public bool BaseComplete()
      {
         if (ContainsKey("DPF0001") &&
            ContainsKey("MVT0001") &&
            ContainsKey("MVT0002") &&
            ContainsKey("MVT0003"))
         {
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
