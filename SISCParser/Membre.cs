using System;

namespace SISCParser
{
   internal class Membre
   {
      public string Nom { get; set; }
      public string Prenom { get; set; }
      public string Sexe { get; set; }
      public DateTime Naissance { get; set; }
      public string NaissanceStr {
         get { return Naissance.ToString("yyyy-MM-dd"); }
         }
      public DateTime Inscription { get; set; }

      public override string ToString()
      {
         return Prenom + " " + Nom + " " + Naissance.ToString("yyyy-MM-dd");
      }
   }
}