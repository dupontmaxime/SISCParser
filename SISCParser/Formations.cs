using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISCParser
{
   class Formations : ListeChamps
   {
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
