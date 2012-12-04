using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System;

namespace SvnChangeSetMetro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttoncheckForModifications_Click(object sender, RoutedEventArgs e)
        {
           // activityProgressbar.IsActive = true;
            LibSvnChangeSet.SvnChangeSetMaker helper = new LibSvnChangeSet.SvnChangeSetMaker();
            string localPath = @"D:\dev\ExportTool\trunk";
            List<string> modifiedFiles = helper.getModifiedFiles(localPath);

         //   listViewChanges.Items.Clear();
         //   foreach (string file in modifiedFiles)
         //       listViewChanges.Items.Add(file);
         ////   activityProgressbar.IsActive = false;
        }

        private void buttonAddRepo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XDocument doc = null;
                if(File.Exists("Repos.xml"))
                    doc = XDocument.Load("Repos.xml");
                else
                {
                    doc = new XDocument();
                }

                var x = from a in doc.Descendants("Path")
                        where a.Value.ToString() == "SignIn"
                        select a.Value;

                string path = string.Empty;
                if (x == null)
                    MessageBox.Show("The repository added already exists in the list.");
                else
                    doc.Add(new XNode("Repositories", 
                        new XNode("Repository",
                            new XElement("Name","Repo-name"),
                            new XElement("Path", path),
                            new XElement("Description", "sample description"))));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing configuration file");
            }
        }
    }
}
