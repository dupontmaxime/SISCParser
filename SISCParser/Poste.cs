using System;

namespace SISCParser
{
   public class Poste
   {
      public Poste(string poste, string palier, string fonction, string debut, string fin)
      {
         NomDePoste = poste;
         PalierDuPoste = new Palier(palier);
         Fonction = fonction;
         if(debut.Trim().Length != 0)
            Debut = DateTime.ParseExact(debut, "yyyyMMdd", null);
         if (fin.Trim().Length != 0)
            Fin = DateTime.ParseExact(fin, "yyyyMMdd", null);
      }

      public string NomDePoste { get; set; }
      public Palier PalierDuPoste { get; set; }
      public string Fonction { get; set; }
      public DateTime Debut { get; set; }
      public DateTime Fin { get; set; }
   }
}