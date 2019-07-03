using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SISCParser
{
   internal class Membre : EntreeNommee
   {
      public Membre()
      {
         ListeDesPostes = new List<Poste>();
      }

      public Membre(string[] fields) : this()
      {
         CodePermanent = fields.ElementAt(GetFieldIndex("code_permanent"));
         Prenom = fields.ElementAt(GetFieldIndex("prenom"));
         Nom = fields.ElementAt(GetFieldIndex("nom"));
         Naissance = DateTime.ParseExact(fields.ElementAt(GetFieldIndex("naissance")), "yyyyMMdd", null);
         Vaj = new VAJ(fields.ElementAt(GetFieldIndex("vaj_remplie")),
                       fields.ElementAt(GetFieldIndex("vaj_effectuee")),
                       fields.ElementAt(GetFieldIndex("vaj_autorite")));
         string cpjDate = fields.ElementAt(GetFieldIndex("cpj_date"));
         PrioriteJeunesse = null;
         if(cpjDate.Trim().Length > 0)
         {
            PrioriteJeunesse = DateTime.ParseExact(cpjDate, "yyyyMMdd", null);
         }

         string ccaDate = fields.ElementAt(GetFieldIndex("cca_date"));
         CodeConduite = null;
         if(ccaDate.Trim().Length >0)
         {
            CodeConduite = DateTime.ParseExact(ccaDate, "yyyyMMdd", null);
         }

         ListeDesPostes.Clear();
         AjoutePoste(fields);
      }

      internal void AjoutePoste(string[] fields)
      {
         ListeDesPostes.Add(new Poste(fields.ElementAt(GetFieldIndex("poste")),
                                      fields.ElementAt(GetFieldIndex("palier")),
                                      fields.ElementAt(GetFieldIndex("fonction")),
                                      fields.ElementAt(GetFieldIndex("debut")),
                                      fields.ElementAt(GetFieldIndex("fin"))));
      }

      public string CodePermanent { get; set; }
      public string Nom { get; set; }
      public string Prenom { get; set; }
      public string Sexe { get; set; }
      public DateTime Naissance { get; set; }
      public string NaissanceStr
      {
         get
         {
            return Naissance.ToString("yyyy-MM-dd");
         }
      }
      public DateTime Inscription { get; set; }
      public VAJ Vaj { get; set; }
      public DateTime? PrioriteJeunesse { get; set; }
      public DateTime? CodeConduite { get; set; }

      public List<Poste> ListeDesPostes;
      public bool PosteDansGroupe(string numeroDeGroupe, bool actif=true)
      {
         Palier recherchePalier = new Palier(numeroDeGroupe);
         if (recherchePalier.Groupe == "000" && ListeDesPostes.Exists(p => p.Actif() == actif))
            return true;
         if (ListeDesPostes.Exists(p => (p.PalierDuPoste.Groupe == recherchePalier.Groupe) && (p.Actif()==actif)))
            return true;
         return false;
      }
      public string Paliers
      {
         get
         {
            StringBuilder sbPaliers = new StringBuilder();
            int nbPoste = ListeDesPostes.Where(p => p.Actif()).Count();

            sbPaliers.Append("[" + nbPoste + "]");

            foreach (Poste poste in ListeDesPostes)
            {
               if (poste.Actif())
               {
                  if (poste.PalierDuPoste.BrancheUnite != Palier.Branche.Aucune)
                  {
                     sbPaliers.Append(Enum.GetName(typeof(Palier.Branche), poste.PalierDuPoste.BrancheUnite) + " ");
                  }
                  // TODO identifier les postes autres que dans une branche
               }
            }
            return sbPaliers.ToString();
         }
      }

      public string Postes
      {
         get
         {
            StringBuilder sbPaliers = new StringBuilder();

            sbPaliers.Append("[" + NbPostes + "]");

            foreach (Poste poste in ListeDesPostes)
            {
               if (poste.Actif())
               {
                  if (poste.PalierDuPoste.BrancheUnite != Palier.Branche.Aucune)
                  {
                     sbPaliers.Append(poste.FonctionDuPoste.TitreFonction() + " " + Enum.GetName(typeof(Palier.Branche), poste.PalierDuPoste.BrancheUnite) + " ");
                  }
                  else if (poste.PalierDuPoste.Groupe != string.Empty)
                  {
                     sbPaliers.Append(poste.FonctionDuPoste.TitreFonction() + " " + poste.PalierDuPoste.Groupe + " ");
                  }
                  // TODO identifier les postes autres que dans une branche
               }
            }
            return sbPaliers.ToString();
         }
      }

      public int NbPostes
      {
         get { return ListeDesPostes.Where(p => p.Actif()).Count(); }
      }

      public int NbPostesSup
      {
         get { return ListeDesPostes.Where(p => p.Actif() && p.FonctionDuPoste.EstSuperviseur()).Count(); }
      }

      public string NbPostesStr
      {
         get { return NbPostesSup.ToString() + "/" + NbPostes.ToString(); }
      }

      public string CPJ
      {
         get { return PrioriteJeunesse?.ToString("dd-MM-yyyy"); }
      }
      public string CCA
      {
         get { return CodeConduite?.ToString("dd-MM-yyyy"); }
      }

      public string[] GroupeArray()
      {
         StringBuilder sbPaliers = new StringBuilder();
         Dictionary<string, int> groupes = new Dictionary<string, int>();
         foreach (Poste poste in ListeDesPostes)
         {
            if (poste.Actif()   &&
               poste.PalierDuPoste.Groupe != string.Empty   &&
               !groupes.ContainsKey(poste.PalierDuPoste.Groupe)   )
            {
               groupes.Add(poste.PalierDuPoste.Groupe, int.Parse(poste.PalierDuPoste.Groupe));
            }
         }
         return groupes.Keys.ToArray();
      }

      public string Groupe
      {
         get
         {
            return string.Join(",", GroupeArray());
         }
      }

      public override string ToString()
      {
         return Prenom + " " + Nom + " " + Naissance.ToString("yyyy-MM-dd");
      }
   }
}