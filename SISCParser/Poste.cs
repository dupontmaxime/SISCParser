using System;
using System.Collections.Generic;
using System.Linq;

namespace SISCParser
{
   public class Postes: List<Poste>
   {
      public void AjoutePoste(string[] fields)
      {
         Add(new Poste(fields.ElementAt(Membre.GetFieldIndex("poste")),
                     fields.ElementAt(Membre.GetFieldIndex("palier")),
                     fields.ElementAt(Membre.GetFieldIndex("fonction")),
                     fields.ElementAt(Membre.GetFieldIndex("debut")),
                     fields.ElementAt(Membre.GetFieldIndex("fin"))));
      }
   }

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