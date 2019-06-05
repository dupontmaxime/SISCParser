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

      Dictionary<string, Membre> listeDesMembres = new Dictionary<string, Membre>();
      List<string> nomDesChamps;

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

      private void BtnParseMembers_Click(object sender, RoutedEventArgs e)
      {
         ParseMembers(memberfilename);
      }

      public int GetFieldIndex(string fieldName) => nomDesChamps.FindIndex(x=>x.ToLower() == fieldName.ToLower());

      public void ParseMembers(string filename)
      {
         Task parseListeMembres = Task.Run(() =>
         {

            bool bChampLus = false;
            using (TextFieldParser parser = new TextFieldParser(filename, Encoding.Default))
            {
               parser.TextFieldType = FieldType.Delimited;
               parser.SetDelimiters(";");
               while (!parser.EndOfData)
               {
                  //Process row
                  string[] fields = parser.ReadFields();
                  if (!bChampLus)
                  {
                     nomDesChamps = fields.ToList();
                     bChampLus = true;
                  }
                  else
                  {
                     Membre membrecourant = null;
                     string codepermanent = fields.ElementAt(GetFieldIndex("code_permanent"));
                     if (!listeDesMembres.ContainsKey(codepermanent))
                     {
                        membrecourant = new Membre();
                        membrecourant.Prenom = fields.ElementAt(GetFieldIndex("prenom"));
                        membrecourant.Nom = fields.ElementAt(GetFieldIndex("nom"));
                        membrecourant.Naissance = DateTime.ParseExact(fields.ElementAt(GetFieldIndex("naissance")), "yyyyMMdd", null);
                        listeDesMembres.Add(codepermanent, membrecourant);
                     }
                  }
               }

               //StringBuilder prettyDict = new StringBuilder();
               //foreach (var pair in listeDesMembres)
               //{
               //   prettyDict.Append(String.Format(" {0} {1} \n ", pair.Key.ToUpper(), pair.Value));
               //}
               //txtEditor.Text = prettyDict.ToString();
            }
         });
      }
   }
}
