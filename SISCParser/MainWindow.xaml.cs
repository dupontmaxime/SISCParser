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
   }
}
