using System;

namespace SISCParser
{
   public class VAJ
   {
      public VAJ(string remplie, string effectuee, string autorisee)
      {
         if (remplie.Trim().Length > 0)
         {
            Remplie = DateTime.ParseExact(remplie, "yyyyMMdd", null);
         }
         else
         {
            Remplie = null;
         }

         if (effectuee.Trim().Length > 0)
         {
            Effectuee = DateTime.ParseExact(effectuee, "yyyyMMdd", null);
         }
         else
         {
            Effectuee = null;
         }

         Autorisee = (autorisee.Trim().Length > 0) ? true : false;

      }

      public override string ToString()
      {
         if (Autorisee)
         {
            if (Effectuee != null)
               return "Complétée le " + Effectuee.Value.ToString("dd - MM - yyyy");
            else
               return "Complétée";
         }
         else if (Effectuee != null)
         {
            return "Reçue incomplete";
         }
         else if (Remplie != null)
         {
            return "Remplie le " + Remplie.Value.ToString("dd - MM - yyyy");
         }
         else
         {
            return "Non remplie";
         }
      }

      public DateTime? Remplie { get; set; }
      public DateTime? Effectuee { get; set; }
      public bool Autorisee { get; set; }
   }
}