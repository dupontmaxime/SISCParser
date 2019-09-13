using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

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
         OrdonnerMetrique("Secouriste");
         OrdonnerMetrique("ActiviteHiver");
         OrdonnerMetrique("HiverLourd");
         OrdonnerMetrique("HiverLeger");
         OrdonnerMetrique("Gilwell");
         OrdonnerMetrique("Grege");
         OrdonnerMetrique("BadgeBois");
         OrdonnerMetrique("CabestanBleu");
         OrdonnerMetrique("CabestanVert");
         OrdonnerMetrique("CabestanViolet");
         OrdonnerMetrique("FormationBaseCompletee");
         OrdonnerMetriquePerCapita("MultiplePoste");
         OrdonnerMetriquePerCapita("MultiplePosteSup");
         OrdonnerMetriquePerCapita("VAJIncomplete");
         OrdonnerMetriquePerCapita("PJIncomplete");
         OrdonnerMetriquePerCapita("CCIncomplete");
         OrdonnerMetriquePerCapita("Secouriste");
         OrdonnerMetriquePerCapita("ActiviteHiver");
         OrdonnerMetriquePerCapita("HiverLourd");
         OrdonnerMetriquePerCapita("HiverLeger");
         OrdonnerMetriquePerCapita("Gilwell");
         OrdonnerMetriquePerCapita("Grege");
         OrdonnerMetriquePerCapita("BadgeBois");
         OrdonnerMetriquePerCapita("CabestanBleu");
         OrdonnerMetriquePerCapita("CabestanVert");
         OrdonnerMetriquePerCapita("CabestanViolet");
         OrdonnerMetriquePerCapita("FormationBaseCompletee");
      }

      private void OrdonnerMetrique(string stringMetrique)
      {
         List<KeyValuePair<string, MetriqueGroupe>> valMetriqueOrdonnee;

         MetriqueGroupe firstMetrique = this.First().Value;
         bool inverse = ((ValeurMetrique)firstMetrique.GetType().GetProperty(stringMetrique).GetValue(firstMetrique)).Inverse;

         valMetriqueOrdonnee = this.OrderBy(m => ((ValeurMetrique)m.Value.GetType().GetProperty(stringMetrique).GetValue(m.Value)).Valeur).ToList();
         valMetriqueOrdonnee = valMetriqueOrdonnee.Where(m => m.Value.IdGroupe.Value != "d10-000").ToList();
         valMetriqueOrdonnee = valMetriqueOrdonnee.Where(m => m.Value.AnimateurActif.Valeur > 0).ToList();
         if (inverse)
            valMetriqueOrdonnee.Reverse();
         int lastIndex = 0;
         int lastValue = -1;
         int curRang = 0;
         foreach (KeyValuePair<string, MetriqueGroupe> rang in valMetriqueOrdonnee)
         {
            curRang++;
            int curMetrique = ((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value)).Valeur;
            if (curMetrique != lastValue)
            {
               lastValue = curMetrique;
               lastIndex = curRang;
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
         valMetriqueOrdonnee = valMetriqueOrdonnee.Where(m => m.Value.AnimateurActif.Valeur > 0).ToList();
         if (inverse)
            valMetriqueOrdonnee.Reverse();

         int lastIndex = 0;
         int curRang = 0;
         double lastValue = Double.MinValue;
         foreach (KeyValuePair<string, MetriqueGroupe> rang in valMetriqueOrdonnee)
         {
            curRang++;
            double metriquePerCapita = rang.Value.PerCapita((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value));
            if (metriquePerCapita != lastValue)
            {
               lastValue = metriquePerCapita;
               lastIndex = curRang;
            }
            ((ValeurMetrique)rang.Value.GetType().GetProperty(stringMetrique).GetValue(rang.Value)).RangPerCapita = lastIndex;
         }
      }

        public void Exporter(string file)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = oXL.Workbooks.Add(Missing.Value);
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                int propTitreCount = typeof(MetriqueGroupe).GetProperties().Count();
                int cellColumn = 1;
                foreach (PropertyInfo prop in typeof(MetriqueGroupe).GetProperties().OrderBy(x => x.Name))
                {
                    if (prop.PropertyType == typeof(IdentifiantGroupe))
                    {
                        oSheet.Cells[1, cellColumn] = "GroupeName";
                        cellColumn++;
                        oSheet.Cells[1, cellColumn] = "GroupeId";
                    }
                    else if (prop.PropertyType == typeof(ValeurMetriqueAbsolu))
                    {
                        string baseName = prop.Name;
                        oSheet.Cells[1, cellColumn] = baseName + "Valeur";
                        cellColumn++;
                        oSheet.Cells[1, cellColumn] = baseName + "Rang";
                    }   
                    else if (prop.PropertyType == typeof(ValeurMetrique))
                    {
                        string baseName = prop.Name;
                        oSheet.Cells[1, cellColumn] = baseName + "Valeur";
                        cellColumn++;
                        oSheet.Cells[1, cellColumn] = baseName + "Rang";
                        cellColumn++;
                        oSheet.Cells[1, cellColumn] = baseName + "ValeurPerCapita";
                        cellColumn++;
                        oSheet.Cells[1, cellColumn] = baseName + "RangPerCapita";
                    }
                    cellColumn++;
                }
                oSheet.get_Range("A1", "AZ1").Font.Bold = true;
                oSheet.get_Range("A1", "AZ1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                int propCount = typeof(MetriqueGroupe).GetProperties().Count();
                var cellRow = 2;
                foreach (MetriqueGroupe metGroupe in Values)
                {
                    if (metGroupe.AnimateurActif.Valeur <= 0)
                        continue;

                    cellColumn = 1;
                    foreach (PropertyInfo prop in metGroupe.GetType().GetProperties().OrderBy(x => x.Name))
                    {
                        if (prop.PropertyType == typeof(IdentifiantGroupe))
                        {
                            IdentifiantGroupe idGroupe = (IdentifiantGroupe)prop.GetValue(metGroupe);
                            oSheet.Cells[cellRow, cellColumn] = idGroupe.Name;
                            cellColumn++;
                            oSheet.Cells[cellRow, cellColumn] = idGroupe.Value;
                            cellColumn++;
                        }
                        else if (prop.PropertyType == typeof(ValeurMetriqueAbsolu))
                        {
                            ValeurMetrique valMetrique = (ValeurMetrique)prop.GetValue(metGroupe);
                            oSheet.Cells[cellRow, cellColumn] = valMetrique.Valeur;
                            cellColumn++;
                            oSheet.Cells[cellRow, cellColumn] = valMetrique.Rang;
                            cellColumn++;
                        }
                        else if (prop.PropertyType == typeof(ValeurMetrique))
                        {
                            ValeurMetrique valMetrique = (ValeurMetrique)prop.GetValue(metGroupe);
                            oSheet.Cells[cellRow, cellColumn] = valMetrique.Valeur;
                            cellColumn++;
                            oSheet.Cells[cellRow, cellColumn] = valMetrique.Rang;
                            cellColumn++;
                            oSheet.Cells[cellRow, cellColumn] = metGroupe.PerCapita(valMetrique).ToString("P0");
                            cellColumn++;
                            oSheet.Cells[cellRow, cellColumn] = valMetrique.RangPerCapita;
                            cellColumn++;
                        }
                    }
                    cellRow++;
                }

                oSheet.get_Range("A1", "AZ1").EntireColumn.AutoFit();

                oWB.SaveAs(file,
                            Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value,
                            false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                string errorMessage;
                errorMessage = "Error: ";
                errorMessage = string.Concat(errorMessage, theException.Message);
                errorMessage = string.Concat(errorMessage, " Line: ");
                errorMessage = string.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
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

   class ValeurMetriqueAbsolu: ValeurMetrique
   {
      public ValeurMetriqueAbsolu(string nom, bool inverse=false)
         : base(nom, true, inverse)
      {
      }
   }

   class MetriqueGroupe
   {
      public MetriqueGroupe(IdentifiantGroupe groupe, List<KeyValuePair<string, Membre>> membres)
      {
         AnimateurActif = new ValeurMetriqueAbsolu("Animateurs Actifs", true);
         MultiplePosteSup = new ValeurMetrique("Membres avec plusieurs postes de supervision");
         MultiplePoste = new ValeurMetrique("Membres avec plusieurs postes");
         VAJIncomplete = new ValeurMetrique("VAJ incomplète");
         PJIncomplete = new ValeurMetrique("Priorité Jeunesse incomplète");
         CCIncomplete = new ValeurMetrique("Code de conduite incomplet");
         Secouriste = new ValeurMetrique("Secouriste", false, true);
         ActiviteHiver = new ValeurMetrique("Activité Hiver", false, true);
         HiverLourd = new ValeurMetrique("Camping Hiver lourd", false, true);
         HiverLeger = new ValeurMetrique("Camping Hiver léger", false, true);
         Gilwell = new ValeurMetrique("Noeud de Gilwell", false, true);
         Grege = new ValeurMetrique("Foulard Grège", false, true);
         BadgeBois = new ValeurMetrique("Badge de bois", false, true);
         CabestanBleu = new ValeurMetrique("Cabestan Bleu", false, true);
         CabestanVert = new ValeurMetrique("Cabestan Vert", false, true);
         CabestanViolet = new ValeurMetrique("Cabestan Violet", false, true);
         FormationBaseCompletee = new ValeurMetrique("Formation de base complétées", false, true);

         EvaluerGroupe(groupe, membres);
      }

      public IdentifiantGroupe IdGroupe { get; set; }
      public ValeurMetriqueAbsolu AnimateurActif { get; set; }
      public ValeurMetrique MultiplePosteSup { get; set; }
      public ValeurMetrique MultiplePoste { get; set; }
      public ValeurMetrique VAJIncomplete { get; set; }
      public ValeurMetrique PJIncomplete { get; set; }
      public ValeurMetrique CCIncomplete { get; set; }
      public ValeurMetrique Secouriste { get; set; }
      public ValeurMetrique ActiviteHiver { get; set; }
      public ValeurMetrique HiverLourd { get; set; }
      public ValeurMetrique HiverLeger { get; set; }
      public ValeurMetrique Gilwell { get; set; }
      public ValeurMetrique Grege { get; set; }
      public ValeurMetrique BadgeBois { get; set; }
      public ValeurMetrique CabestanBleu { get; set; }
      public ValeurMetrique CabestanVert { get; set; }
      public ValeurMetrique CabestanViolet { get; set; }
      public ValeurMetrique FormationBaseCompletee { get; set; }

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
         Secouriste.Valeur = membres.Where(m => m.Value.ListeDesBrevets.Secourisme()).Count();
         ActiviteHiver.Valeur = membres.Where(m => m.Value.ListeDesBrevets.ActiviteHiver()).Count();
         HiverLourd.Valeur = membres.Where(m => m.Value.ListeDesBrevets.HiverLourd()).Count();
         HiverLeger.Valeur = membres.Where(m => m.Value.ListeDesBrevets.HiverLeger()).Count();
         Gilwell.Valeur = membres.Where(m => m.Value.ListeDesBrevets.Gilwell()).Count();
         Grege.Valeur = membres.Where(m => m.Value.ListeDesBrevets.Grege()).Count();
         BadgeBois.Valeur = membres.Where(m => m.Value.ListeDesBrevets.BadgeBois()).Count();
         CabestanBleu.Valeur = membres.Where(m => m.Value.ListeDesBrevets.CabestanBleu()).Count();
         CabestanVert.Valeur = membres.Where(m => m.Value.ListeDesBrevets.CabestanVert()).Count();
         CabestanViolet.Valeur = membres.Where(m => m.Value.ListeDesBrevets.CabestanViolet()).Count();
         FormationBaseCompletee.Valeur = membres.Where(m => m.Value.ListeDesFormations.BaseComplete()).Count();
      }

   }
}
