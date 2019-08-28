using System;

namespace SISCParser
{
   public class VAJ
   {
      public enum VAJStatut { COMPLETEE, COMPLETEE_SANS_DATE, INCOMPLETE, REMPLIE, NON_REMPLIE };

      public VAJ(string remplie, string effectuee, string autorisee)
      {
         DateTime dateRemplie = new DateTime();
         if (remplie.Trim().Length > 0 &&
            DateTime.TryParseExact(remplie, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dateRemplie))
         {
            Remplie = dateRemplie;
         }
         else
         {
            Remplie = null;
         }

         DateTime dateEffectuee = new DateTime();
         if (effectuee.Trim().Length > 0 &&
            DateTime.TryParseExact(effectuee, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dateEffectuee))
         {
            Effectuee = dateEffectuee;
         }
         else
         {
            Effectuee = null;
         }

         Autorisee = (autorisee.Trim().Length > 0) ? true : false;

      }

      public override string ToString()
      {
         switch(Statut)
         {
            case VAJStatut.COMPLETEE:
               return "Complétée le " + Effectuee.Value.ToString("dd - MM - yyyy");
            case VAJStatut.COMPLETEE_SANS_DATE:
               return "Complétée";
            case VAJStatut.INCOMPLETE:
               return "Reçue incomplete";
            case VAJStatut.REMPLIE:
               return "Remplie le " + Remplie.Value.ToString("dd - MM - yyyy");
            case VAJStatut.NON_REMPLIE:
            default:
               return "Non remplie"; 
         }
      }

      public DateTime? Remplie { get; set; }
      public DateTime? Effectuee { get; set; }
      public bool Autorisee { get; set; }
      public VAJStatut Statut {
         get
         {
            if (Autorisee)
               if (Effectuee != null)
                  return VAJStatut.COMPLETEE;
               else
                  return VAJStatut.COMPLETEE_SANS_DATE;
            else if (Effectuee != null)
               return VAJStatut.INCOMPLETE;
            else if (Remplie != null)
               return VAJStatut.REMPLIE;
            else
               return VAJStatut.NON_REMPLIE;
         }
      }
   }
}