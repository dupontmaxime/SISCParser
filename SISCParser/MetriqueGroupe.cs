﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SISCParser
{
   class DictMetriques : Dictionary<String, MetriqueGroupe>
   {
      public DictMetriques() { }

      public void ClasserMetriques()
      {
         OrdonnerMetrique("AnimateurActif");
         OrdonnerMetrique("MultiplePoste");
         OrdonnerMetrique("MultiplePosteSup");
         OrdonnerMetrique("VAJIncomplete");
         OrdonnerMetrique("PJIncomplete");
         OrdonnerMetrique("CCIncomplete");
         OrdonnerMetriquePerCapita("MultiplePoste");
         OrdonnerMetriquePerCapita("MultiplePosteSup");
         OrdonnerMetriquePerCapita("VAJIncomplete");
         OrdonnerMetriquePerCapita("PJIncomplete");
         OrdonnerMetriquePerCapita("CCIncomplete");
      }

      private void OrdonnerMetrique(string stringMetrique)
      {
         List<KeyValuePair<string, MetriqueGroupe>> valMetriqueOrdonnee;

         MetriqueGroupe firstMetrique = this.First().Value;
         bool inverse = ((ValeurMetrique)firstMetrique.GetType().GetProperty(stringMetrique).GetValue(firstMetrique)).Inverse;

         valMetriqueOrdonnee = this.OrderBy(m => ((ValeurMetrique)m.Value.GetType().GetProperty(stringMetrique).GetValue(m.Value)).Valeur).ToList();
         valMetriqueOrdonnee = valMetriqueOrdonnee.Where(m => m.Value.IdGroupe.Value != "d10-000").ToList();
         if (inverse)
            valMetriqueOrdonnee.Reverse();
         int lastIndex = 0;
         int lastValue = -1;
         foreach (KeyValuePair<string, MetriqueGroupe> rang in valMetriqueOrdonnee)
         {
            int curMetrique = ((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value)).Valeur;
            if (curMetrique != lastValue)
            {
               lastValue = curMetrique;
               lastIndex += 1;
            }
            ((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value)).Rang = lastIndex;
         }
      }

      //List<KeyValuePair<string, MetriqueGroupe>> rangMultiplePosteSup = this.OrderBy(m => m.Value.MultiplePosteSup.Valeur).ToList();
      //int lastIndex = 0;
      //int lastValue = -1;
      //foreach (KeyValuePair<string, MetriqueGroupe> rang in rangMultiplePosteSup)
      //{
      //   if (rang.Value.MultiplePosteSup.Valeur != lastValue)
      //   {
      //      lastValue = rang.Value.MultiplePosteSup.Valeur;
      //      lastIndex += 1;
      //   }
      //   rang.Value.MultiplePosteSup.Rang = lastIndex;
      //}

      private void OrdonnerMetriquePerCapita(string stringMetrique)
      {
         List<KeyValuePair<string, MetriqueGroupe>> valMetriqueOrdonnee;

         bool inverse = ((ValeurMetrique)this.First().Value.GetType().GetProperty(stringMetrique).GetValue(this.First().Value)).Inverse;

         valMetriqueOrdonnee = this.OrderBy(m => m.Value.PerCapita((ValeurMetrique)m.Value.GetType().GetProperty(stringMetrique).GetValue(m.Value))).ToList();
         valMetriqueOrdonnee = valMetriqueOrdonnee.Where(m => m.Value.IdGroupe.Value != "d10-000").ToList();
         if (inverse)
            valMetriqueOrdonnee.Reverse();

         int lastIndex = 0;
         double lastValue = Double.MinValue;
         foreach (KeyValuePair<string, MetriqueGroupe> rang in valMetriqueOrdonnee)
         {
            double metriquePerCapita = rang.Value.PerCapita((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value));
            if (metriquePerCapita != lastValue)
            {
               lastValue = metriquePerCapita;
               lastIndex += 1;
            }
            ((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value)).RangPerCapita = lastIndex;
         }
      }

      public void Exporter(string file)
      {
         try
         {
            using (TextWriter stream = new StreamWriter(file, false, Encoding.UTF8))
            {
               if (stream != null)
               {
                  int propTitreCount = typeof(MetriqueGroupe).GetProperties().Count();
                  int iTitreProp = 0;
                  foreach (PropertyInfo prop in typeof(MetriqueGroupe).GetProperties())
                  {
                     if(prop.PropertyType == typeof(IdentifiantGroupe))
                     {
                        stream.Write("GroupeName, GroupeID");
                     }
                     else if (prop.PropertyType == typeof(ValeurMetrique))
                     {
                        string baseName = prop.Name;
                        stream.Write(baseName+"Valeur, "+baseName+"Rang");
                     }
                     iTitreProp++;

                     if (iTitreProp < propTitreCount)
                     {
                        stream.Write(", ");
                     }
                     else
                     {
                        stream.Write("\n");
                     }
                  }


                  int propCount = typeof(MetriqueGroupe).GetProperties().Count();
                  foreach (MetriqueGroupe metGroupe in this.Values)
                  {
                     int iProp = 0;
                     foreach (PropertyInfo prop in metGroupe.GetType().GetProperties())
                     {
                        if (prop.PropertyType == typeof(IdentifiantGroupe))
                        {
                           IdentifiantGroupe idGroupe = (IdentifiantGroupe)prop.GetValue(metGroupe);
                           stream.Write(idGroupe.Name+", "+idGroupe.Value);
                        }
                        else if (prop.PropertyType == typeof(ValeurMetrique))
                        {
                           ValeurMetrique valMetrique = (ValeurMetrique)prop.GetValue(metGroupe);
                           stream.Write(valMetrique.Valeur + ", " + valMetrique.Rang);
                        }
                        iProp++;

                        if (iProp < propCount)
                        {
                           stream.Write(", ");
                        }
                        else
                        {
                           stream.Write("\n");
                        }
                     }
                  }
               }
            }
         }
         catch (ArgumentNullException)
         {
            MessageBox.Show("Pas de fichier choisi pour la sauvegarde", "Pas de fichier choisi", MessageBoxButton.OK, MessageBoxImage.Warning);
         }
      }
   }

   class ValeurMetrique
   {
      public ValeurMetrique(string nom, bool absolu=false, bool inverse=false)
      {
         Nom = nom;
         Absolu = absolu;
         Inverse = inverse;
      }
      public string Nom { get; set; }
      public int Valeur { get; set; }
      public int Rang { get; set; }
      public int RangPerCapita { get; set; }
      public bool Absolu { get; set; }
      public bool Inverse { get; set; }
   }

   class MetriqueGroupe
   {
      public MetriqueGroupe(IdentifiantGroupe groupe, List<KeyValuePair<string, Membre>> membres)
      {
         AnimateurActif = new ValeurMetrique("Animateurs Actifs", true, true);
         MultiplePosteSup = new ValeurMetrique("Membres avec plusieurs postes de supervision");
         MultiplePoste = new ValeurMetrique("Membres avec plusieurs postes");
         VAJIncomplete = new ValeurMetrique("VAJ incomplète");
         PJIncomplete = new ValeurMetrique("Priorité Jeunesse incomplète");
         CCIncomplete = new ValeurMetrique("Code de conduite incomplet");

         EvaluerGroupe(groupe, membres);
      }

      public IdentifiantGroupe IdGroupe { get; set; }
      public ValeurMetrique AnimateurActif { get; set; }
      public ValeurMetrique MultiplePosteSup { get; set; }
      public ValeurMetrique MultiplePoste { get; set; }
      public ValeurMetrique VAJIncomplete { get; set; }
      public ValeurMetrique PJIncomplete { get; set; }
      public ValeurMetrique CCIncomplete { get; set; }

      public double PerCapita(ValeurMetrique valeurMetrique)
      {
         if (valeurMetrique.Absolu)
            return valeurMetrique.Valeur;
         else
            return valeurMetrique.Valeur / (double)AnimateurActif.Valeur;
      }

      public void EvaluerGroupe(IdentifiantGroupe groupe, List<KeyValuePair<string, Membre>> membres)
      {
         IdGroupe = groupe;
         AnimateurActif.Valeur = membres.Where(m => m.Value.NbPostes > 0).Count();
         MultiplePosteSup.Valeur = membres.Where(m => m.Value.NbPostesSup > 1).Count();
         MultiplePoste.Valeur = membres.Where(m => m.Value.NbPostes > 1).Count();
         VAJIncomplete.Valeur = membres.Where(m => (m.Value.Vaj.Statut == VAJ.VAJStatut.NON_REMPLIE) || (m.Value.Vaj.Statut == VAJ.VAJStatut.INCOMPLETE)).Count();
         PJIncomplete.Valeur = membres.Where(m => m.Value.PrioriteJeunesse == null).Count();
         CCIncomplete.Valeur = membres.Where(m => m.Value.CodeConduite == null).Count();
      }

   }
}
