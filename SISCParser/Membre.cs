﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SISCParser
{
   internal class Membre
   {
      public static List<string> nomDesChamps { get;  set; }
      public Membre()
      {
         ListeDesPostes = new List<Poste>();
      }

      public Membre(string[] fields) : this()
      {
         Prenom = fields.ElementAt(GetFieldIndex("prenom"));
         Nom = fields.ElementAt(GetFieldIndex("nom"));
         Naissance = DateTime.ParseExact(fields.ElementAt(GetFieldIndex("naissance")), "yyyyMMdd", null);
         Vaj = new VAJ(fields.ElementAt(GetFieldIndex("vaj_remplie")),
                       fields.ElementAt(GetFieldIndex("vaj_effectuee")),
                       fields.ElementAt(GetFieldIndex("vaj_autorite")));
         ListeDesPostes.Clear();
         AjoutePoste(fields);
      }

      public static int GetFieldIndex(string fieldName) => nomDesChamps.FindIndex(x => x.ToLower() == fieldName.ToLower());

      internal void AjoutePoste(string[] fields)
      {
         ListeDesPostes.Add(new Poste(fields.ElementAt(GetFieldIndex("poste")),
                                      fields.ElementAt(GetFieldIndex("palier")),
                                      fields.ElementAt(GetFieldIndex("fonction")),
                                      fields.ElementAt(GetFieldIndex("debut")),
                                      fields.ElementAt(GetFieldIndex("fin"))));
      }

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

      public List<Poste> ListeDesPostes;
      public VAJ Vaj { get; set; }
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
            int nbPoste = ListeDesPostes.Where(p => p.Actif()).Count();

            sbPaliers.Append("[" + nbPoste + "]");

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