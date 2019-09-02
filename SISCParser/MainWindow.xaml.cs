using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Reflection;
using SISCParser.Properties;

namespace SISCParser
{
    public partial class MainWindow : Window
    {
        #region Variable

        private string memberfilename;
        private string groupefilename;
        private string metriquefilename;

        List<IdentifiantGroupe> listeGroupe = new List<IdentifiantGroupe>();
        Dictionary<string, Membre> listeDesMembres = new Dictionary<string, Membre>();
        List<KeyValuePair<string, Membre>> membresDuGroupe;
        DictMetriques metriquedesgroupes = new DictMetriques();

        #endregion

        public MainWindow()
        {
            Resources["Membres"] = listeDesMembres;

            InitializeComponent();

            Metriquefilename = Settings.Default.MetriqueFilePath;
            Groupefilename = Settings.Default.GroupFilePath;
            Memberfilename = Settings.Default.MemberFilePath;
        }

        #region Private

        private void ParseMembers(string filename)
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

            UpdateGroupeInfo();
        }

        private void ParseGroupes(string filename)
        {
            listeGroupe.Clear();
            using (TextFieldParser parser = new TextFieldParser(filename, Encoding.Default))
            {
                listeGroupe.Add(new IdentifiantGroupe() { Name = "Tous", Value = "d10-000" });

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                //init field title
                Groupe.NomDesChamps(new string[] { "a", "b", "numero", "nom", "identifiant", "f",
                                                  "g", "h", "i", "j", "k", "l",
                                                  "m", "n", "o", "p", "q", "r",
                                                  "s", "t", "u", "v", "w"});
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    string nom = fields.ElementAt(Membre.GetFieldIndex("nom"));
                    string identifiant = fields.ElementAt(Membre.GetFieldIndex("identifiant"));

                    listeGroupe.Add(new IdentifiantGroupe() { Name = nom, Value = identifiant });
                }
            }

            comboGroupe.ItemsSource = listeGroupe;
            comboGroupe.SelectedIndex = 0;
            comboGroupe.DisplayMemberPath = "Name";
            comboGroupe.SelectedValuePath = "Value";
        }

        private string GetSelectedGroupe()
        {
            try
            {
                return comboGroupe.SelectedValue.ToString();
            }
            catch (NullReferenceException)
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
            foreach (IdentifiantGroupe groupe in listeGroupe)
            {
                List<KeyValuePair<string, Membre>> membresSelection = GetMembresDuGroupe(listeMembres, groupe.Value);
                if (!metriquedesgroupes.ContainsKey(groupe.Value))
                {
                    MetriqueGroupe metrique = new MetriqueGroupe(groupe, membresSelection);
                    metriquedesgroupes.Add(groupe.Value, metrique);
                }
            }
            metriquedesgroupes.ClasserMetriques();
        }

      private void UpdateGroupeInfo()
      {
         StringBuilder txtGroupeDetail = new StringBuilder();
         try
         {
            MetriqueGroupe metrique = metriquedesgroupes[GetSelectedGroupe()];
            PropertyInfo[] fieldsMetrique = metrique.GetType().GetProperties();
            foreach (PropertyInfo fieldMetrique in fieldsMetrique)
            {
               if (fieldMetrique.PropertyType == typeof(ValeurMetrique))
               {
                  ValeurMetrique fieldValue = (ValeurMetrique)fieldMetrique.GetValue(metrique);
                  txtGroupeDetail.Append(fieldValue.Nom + ": " + fieldValue.Valeur + " (" + fieldValue.Rang + " e)\n");
               }
               else
                  continue;
            }
         }
         catch (KeyNotFoundException)
         {
            txtGroupeDetail.Append("Pas de groupe sélectionné");
         }

            GroupeInfo.Text = txtGroupeDetail.ToString();
        }

        #endregion
        
        #region Events

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Memberfilename = openFileDialog.FileName;
        }

        private void btnOpenFileGroupe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Groupefilename = openFileDialog.FileName;
        }

        private void BtnOpenFileMetriquesGroupes_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xlsx";
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog.Title = "Choisir un fichier de métrique";
            if (openFileDialog.ShowDialog() == true)
                Metriquefilename = openFileDialog.FileName;
        }

        private void BtnGenerateStats_Click(object sender, RoutedEventArgs e)
        {
            EvaluerMetriques(listeDesMembres, listeGroupe);
            metriquedesgroupes.Exporter(Metriquefilename);
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

        #endregion

        #region Property

        public string Groupefilename
        {
            get
            {
                return groupefilename;
            }
            set
            {
                groupefilename = value;
                txtGroupeCSV.Text = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Properties.Settings.Default.GroupFilePath = value;
                    Properties.Settings.Default.Save();
                    ParseGroupes(value);
                }
            }
        }
        public string Metriquefilename
        {
            get
            {
                return metriquefilename;
            }
            set
            {
                metriquefilename = value;
                txtMetriquesGroupes.Text = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Properties.Settings.Default.MetriqueFilePath = value;
                    Properties.Settings.Default.Save();
                }
            }
        }
        public string Memberfilename
        {
            get
            {
                return memberfilename;
            }
            set
            {
                memberfilename = value;
                txtMembreCSV.Text = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Properties.Settings.Default.MemberFilePath = value;
                    Properties.Settings.Default.Save();
                    if (!string.IsNullOrEmpty(Groupefilename))
                        ParseMembers(value);
                }
            }
        }

        #endregion
    }
}
