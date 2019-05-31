using System;

namespace SISCParser
{
   internal class Membre
   {
      public string Nom;
      public string Prenom;
      public string Sexe;
      public DateTime Naissance;
      public DateTime Inscription;

      public override string ToString()
      {
         return Prenom + " " + Nom;
      }
   }
}