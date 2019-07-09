using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         List<KeyValuePair<string, MetriqueGroupe>> rangMultiplePosteSup;

         rangMultiplePosteSup = this.OrderBy(m => ((ValeurMetrique)m.Value.GetType().GetField(stringMetrique).GetValue(m.Value)).Valeur).ToList();
         int lastIndex = 0;
         int lastValue = -1;
         foreach (KeyValuePair<string, MetriqueGroupe> rang in rangMultiplePosteSup)
         {
            int curMetrique = ((ValeurMetrique)rang.Value.GetType().GetField(stringMetrique).GetValue(rang.Value)).Valeur;
            if (curMetrique != lastValue)
            {
               lastValue = curMetrique;
               lastIndex += 1;
            }
            ((ValeurMetrique)rang.Value.GetType().GetField(stringMetrique).GetValue(rang.Value)).Rang = lastIndex;
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
         List<KeyValuePair<string, MetriqueGroupe>> rangMultiplePosteSup;

         rangMultiplePosteSup = this.OrderBy(m => m.Value.PerCapita((ValeurMetrique)m.Value.GetType().GetField(stringMetrique).GetValue(m.Value))).ToList();
         int lastIndex = 0;
         double lastValue = Double.MinValue;
         foreach (KeyValuePair<string, MetriqueGroupe> rang in rangMultiplePosteSup)
         {
            double metriquePerCapita = rang.Value.PerCapita((ValeurMetrique)rang.Value.GetType().GetField(stringMetrique).GetValue(rang.Value));
            if (metriquePerCapita > lastValue)
            {
               lastValue = metriquePerCapita;
               lastIndex += 1;
            }
            ((ValeurMetrique)rang.Value.GetType().GetField(stringMetrique).GetValue(rang.Value)).RangPerCapita = lastIndex;
         }
      }
   }

   class ValeurMetrique
   {
      public int Valeur { get; set; }
      public int Rang { get; set; }
      public int RangPerCapita { get; set; }
   }

   class MetriqueGroupe
   {
      public MetriqueGroupe(IdentifiantGroupe groupe, List<KeyValuePair<string, Membre>> membres)
      {
         AnimateurActif = new ValeurMetrique();
         MultiplePosteSup = new ValeurMetrique();
         MultiplePoste = new ValeurMetrique();
         VAJIncomplete = new ValeurMetrique();
         PJIncomplete = new ValeurMetrique();
         CCIncomplete = new ValeurMetrique();

         EvaluerGroupe(groupe, membres);
      }

      public IdentifiantGroupe IdGroupe { get; set; }
      public ValeurMetrique AnimateurActif;
      public ValeurMetrique MultiplePosteSup;
      public ValeurMetrique MultiplePoste;
      public ValeurMetrique VAJIncomplete;
      public ValeurMetrique PJIncomplete;
      public ValeurMetrique CCIncomplete;

      public double PerCapita(ValeurMetrique valeurMetrique)
      {
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
