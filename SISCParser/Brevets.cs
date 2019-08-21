using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISCParser
{
   class Brevets : ListeChamps
   {
      public bool Secourisme()
      {
         if (ContainsKey("saj") ||
             ContainsKey("sar") ||
             ContainsKey("scr") ||
             ContainsKey("sdr") ||
             ContainsKey("sec") ||
             ContainsKey("ser") ||
             FormateurSecourisme())
            return true;
         else
            return false;
      }

      public bool FormateurSecourisme()
      {
         if (ContainsKey("maj") ||
             ContainsKey("mcr") ||
             ContainsKey("mec") ||
             ContainsKey("mfm"))
            return true;
         else
            return false;
      }

      public bool ActiviteHiver() { return ContainsKey("ch1"); }
      public bool HiverLourd() { return ContainsKey("ch2"); }
      public bool HiverLeger() { return ContainsKey("ch3"); }
      public bool Gilwell() { return ContainsKey("an1"); }
      public bool Grege() { return ContainsKey("an2"); }
      public bool BadgeBois() { return ContainsKey("an3"); }
      public bool CabestanBleu() { return ContainsKey("ge1"); }
      public bool CabestanVert() { return ContainsKey("ge2"); }
      public bool CabestanViolet () { return ContainsKey("ge3"); }
   }
}
