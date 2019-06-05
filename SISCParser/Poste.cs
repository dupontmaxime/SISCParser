using System;

namespace SISCParser
{
   public class Poste
   {
      public Poste(string poste, string palier, string fonction, string debut, string fin)
      {
         NomDePoste = poste;
         Palier = palier;
         Fonction = fonction;
         if(debut.Trim().Length != 0)
            Debut = DateTime.ParseExact(debut, "yyyyMMdd", null);
         if (fin.Trim().Length != 0)
            Fin = DateTime.ParseExact(fin, "yyyyMMdd", null);
      }

      string NomDePoste { get; set; }
      string Palier { get; set; }
      string Fonction { get; set; }
      DateTime Debut { get; set; }
      DateTime Fin { get; set; }
   }
}