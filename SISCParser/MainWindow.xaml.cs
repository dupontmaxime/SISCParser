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

namespace SISCParser
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public string memberfilename;
      public string groupefilename;

      Dictionary<string, Membre> listeDesMembres = new Dictionary<string, Membre>();

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

         foreach (KeyValuePair<string, Membre> entreeMembre in listeDesMembres)
         {
            LVMembres.Items.Add(entreeMembre);
         }
      }

      public class IdentifiantGroupe
      {
         public string Name { get; set; }
         public string Value { get; set; }
      }

      public void ParseGroupes(string filename)
      {
         bool bChampLus = false;
         List<IdentifiantGroupe> listeGroupe = new List<IdentifiantGroupe>();
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
         List<KeyValuePair<string, Membre>> membresDuGroupe = listeDesMembres.Where(m => m.Value.PosteDansGroupe(comboGroupe.SelectedValue.ToString())).ToList();

         LVMembres.Items.Clear();
         foreach (KeyValuePair<string, Membre> membre in membresDuGroupe)
         {
            LVMembres.Items.Add(membre);
         }
      }
   }
}
