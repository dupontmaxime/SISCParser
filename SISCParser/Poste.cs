using System;
using System.Collections.Generic;
using System.Linq;

namespace SISCParser
{
    public class Postes : List<Poste>
    {
        public void AjoutePoste(string[] fields)
        {
            Add(new Poste(fields.ElementAt(EntreeNommee.GetFieldIndex("poste")),
                        fields.ElementAt(EntreeNommee.GetFieldIndex("palier")),
                        fields.ElementAt(EntreeNommee.GetFieldIndex("fonction")),
                        fields.ElementAt(EntreeNommee.GetFieldIndex("debut")),
                        fields.ElementAt(EntreeNommee.GetFieldIndex("fin"))));
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