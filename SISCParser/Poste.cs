using System;

namespace SISCParser
{
   public class Poste
   {
      public Poste(string poste, string palier, string fonction, string debut, string fin)
      {
         NomDePoste = poste;
         PalierDuPoste = new Palier(palier);
         FonctionDuPoste = new Fonction(fonction);
         if (debut.Trim().Length != 0)
            Debut = DateTime.ParseExact(debut, "yyyyMMdd", null);
         else
            Debut = null;
         if (fin.Trim().Length != 0)
            Fin = DateTime.ParseExact(fin, "yyyyMMdd", null);
         else
            Fin = null;
      }

      public string NomDePoste { get; set; }
      public Palier PalierDuPoste { get; set; }
      public Fonction FonctionDuPoste { get; set; }
      public DateTime? Debut { get; set; }
      public DateTime? Fin { get; set; }

      internal bool Actif()
      {
         if (Fin == null)
         {
            return true;
         }
         return false;
      }
   }
}