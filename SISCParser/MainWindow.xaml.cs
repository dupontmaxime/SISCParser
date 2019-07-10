using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

namespace SISCParser
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public string memberfilename;
      public string groupefilename;

      List<IdentifiantGroupe> listeGroupe = new List<IdentifiantGroupe>();
      Dictionary<string, Membre> listeDesMembres = new Dictionary<string, Membre>();
      List<KeyValuePair<string, Membre>> membresDuGroupe;
      DictMetriques metriquedesgroupes = new DictMetriques();

      public MainWindow()
      {
         Resources["Membres"] = listeDesMembres;

         InitializeComponent();
      }

      private void btnOpenFile_Click(object sender, RoutedEventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         if (openFileDialog.ShowDialog() == true)
            memberfilename = openFileDialog.FileName;
         else
            memberfilename = null;

         txtMembreCSV.Text = memberfilename;
      }

      private void btnOpenFileGroupe_Click(object sender, RoutedEventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         if (openFileDialog.ShowDialog() == true)
            groupefilename = openFileDialog.FileName;
         else
            groupefilename = null;

         txtGroupeCSV.Text = groupefilename;
         ParseGroupes(groupefilename);
      }
      private void BtnParseMembers_Click(object sender, RoutedEventArgs e)
      {
         ParseMembers(memberfilename);
      }

      public void ParseMembers(string filename)
      {
         bool bChampLus = false;
         using (TextFieldParser parser = new TextFieldParser(filename, Encoding.Default))
         {
            LVMembres.Items.Clear();
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            while (!parser.EndOfData)
            {
               //Process row
               string[] fields = parser.ReadFields();
               if (!bChampLus)
               {
                  Membre.nomDesChamps = fields.ToList();
                  bChampLus = true;
               }
               else
               {
                  Membre membrecourant = null;
                  string codepermanent = fields.ElementAt(Membre.GetFieldIndex("code_permanent"));
                  if (!listeDesMembres.ContainsKey(codepermanent))
                  {
                     membrecourant = new Membre(fields);
                     listeDesMembres.Add(codepermanent, membrecourant);
                  }
                  else
                  {
                     listeDesMembres[codepermanent].AjoutePoste(fields);
                  }
               }
            }
         }

         membresDuGroupe = listeDesMembres.ToList();
         foreach (KeyValuePair<string, Membre> entreeMembre in listeDesMembres)
         {
            LVMembres.Items.Add(entreeMembre);
         }

         EvaluerMetriques(listeDesMembres, listeGroupe);

         UpdateGroupeInfo();
      }

      public void ParseGroupes(string filename)
      {
         bool bChampLus = false;
         listeGroupe.Clear();
         using (TextFieldParser parser = new TextFieldParser(filename, Encoding.Default))
         {
            listeGroupe.Add(new IdentifiantGroupe() { Name = "Tous", Value = "d10-000" });

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            while (!parser.EndOfData)
            {
               //init field title
               Groupe.NomDesChamps(new string[] { "a", "b", "numero", "nom", "identifiant", "f",
                                                  "g", "h", "i", "j", "k", "l",
                                                  "m", "n", "o", "p", "q", "r",
                                                  "s", "t", "u", "v", "w"});
               bChampLus = true;

               //Process row
               string[] fields = parser.ReadFields();
               if (!bChampLus)
               {
                  //Groupe.NomDesChamps(new string[] { "a", "b", "numero", "nom", "identifiant", "f",
                  //                                    "g", "h", "i", "j", "k", "l",
                  //                                    "m", "n", "o", "p", "q", "r",
                  //                                    "s", "t", "u", "v", "w"});
                  //bChampLus = true;
               }
               else
               {
                  string nom = fields.ElementAt(Membre.GetFieldIndex("nom"));
                  string identifiant = fields.ElementAt(Membre.GetFieldIndex("identifiant"));

                  listeGroupe.Add(new IdentifiantGroupe() { Name = nom, Value = identifiant });
               }
            }
         }

         comboGroupe.ItemsSource = listeGroupe;
         comboGroupe.SelectedIndex = 0;
         comboGroupe.DisplayMemberPath = "Name";
         comboGroupe.SelectedValuePath = "Value";
      }

      private void CtrlCCopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
      {
         var builder = new StringBuilder();
         foreach (ListViewItem item in LVMembres.SelectedItems)
            builder.AppendLine(item.ToString());

         Clipboard.SetText(builder.ToString());
      }

      private void CtrlCCopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
      {
         e.CanExecute = true;
      }

      private void ComboGroupe_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         membresDuGroupe = GetMembresDuGroupe(listeDesMembres, GetSelectedGroupe());

         LVMembres.Items.Clear();
         foreach (KeyValuePair<string, Membre> membre in membresDuGroupe)
         {
            LVMembres.Items.Add(membre);
         }
         UpdateGroupeInfo();
      }

      private string GetSelectedGroupe()
      {
         try
         {
            return comboGroupe.SelectedValue.ToString();
         }
         catch(NullReferenceException)
         {
            return "";
         }
      }

      private List<KeyValuePair<string, Membre>> GetMembresDuGroupe(Dictionary<string, Membre> listeMembres, string selectionGroupe)
      {
          return listeMembres.Where(m => m.Value.PosteDansGroupe(selectionGroupe)).ToList();
      }

      private void EvaluerMetriques(Dictionary<string, Membre> listeMembres, List<IdentifiantGroupe> listeGroupe)
      {
         foreach(IdentifiantGroupe groupe in listeGroupe)
         {
            List<KeyValuePair<string,Membre>> membresSelection = GetMembresDuGroupe(listeMembres, groupe.Value);
            if(!metriquedesgroupes.ContainsKey(groupe.Value))
            {
               MetriqueGroupe metrique = new MetriqueGroupe(groupe, membresSelection);
               metriquedesgroupes.Add(groupe.Value, metrique);
            }
         }
         metriquedesgroupes.ClasserMetriques();
      }

      public void UpdateGroupeInfo()
      {
         StringBuilder txtGroupeDetail = new StringBuilder();
         try
         {
            MetriqueGroupe metrique = metriquedesgroupes[GetSelectedGroupe()];
            FieldInfo[] fieldsMetrique = metrique.GetType().GetFields();
            foreach (FieldInfo fieldMetrique in fieldsMetrique)
            {
               ValeurMetrique fieldValue = (ValeurMetrique)fieldMetrique.GetValue(metrique);
               txtGroupeDetail.Append(fieldValue.Nom + ": " + fieldValue.Valeur + " (" + fieldValue.Rang + " e)\n");
            }
         }
         catch (KeyNotFoundException)
         {
            txtGroupeDetail.Append("Pas de groupe sélectionné");
         }

         GroupeInfo.Text = txtGroupeDetail.ToString();
      }
   }
}
