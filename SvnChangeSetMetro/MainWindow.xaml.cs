using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System;
using MahApps.Metro;
using System.Windows.Media.Imaging;

namespace SvnChangeSetMetro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Repository Information
        /// </summary>
        class RepoInfo
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public string Description { get; set; }
        }

        class ChangedFilesInfo
        {
            public string Path { get; set; }
            public bool Selected { get; set; }
            public string Status { get; set; }
        }

        private readonly string RepoConfigXml = "Repos.xml";
        private List<RepoInfo> repositoryInfo;
        private List<ChangedFilesInfo> modifiedFileInfo;

        private string selectedRepoPath = string.Empty;


        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.ChangeTheme(this, ThemeManager.DefaultAccents.First(a => a.Name == "Orange"), Theme.Light);

            InitializeRepoData();
            modifiedFileInfo = new List<ChangedFilesInfo>();
            listViewChanges.ItemsSource = modifiedFileInfo;
        }

        private void buttoncheckForModifications_Click(object sender, RoutedEventArgs e)
        {
           // activityProgressbar.IsActive = true;
            LibSvnChangeSet.SvnChangeSetMaker helper = new LibSvnChangeSet.SvnChangeSetMaker();
            string localPath = @"D:\dev\ExportTool\trunk";
            List<string> modifiedFiles = helper.getModifiedFiles(localPath);
        }

        private void buttonAddRepo_Click(object sender, RoutedEventArgs e)
        {
            if (!repositoryInfo.Any((repo) => repo.Path == textBoxRepoPath.Text))
            {
                repositoryInfo.Add(new RepoInfo()
                {
                    Path = textBoxRepoPath.Text,
                    Name = "Testing",
                    Description = "Sample Description"
                });
                listViewRepos.Items.Refresh();
            }
            else
                MessageBox.Show("Selected repository already exists");
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveCurrentRepos();
        }


        /// <summary>
        /// Initializes the repository data from the default XML file
        /// TODO: Replace with binding
        /// </summary>
        private void InitializeRepoData()
        {
            try
            {
                repositoryInfo = new List<RepoInfo>();
                if (File.Exists(RepoConfigXml))
                {
                    XDocument doc = XDocument.Load(RepoConfigXml);
                    repositoryInfo = (from repo in doc.Elements("Repositories").Elements("Repo")
                                      select new RepoInfo
                                      {
                                          Name = (string)repo.Element("Name"),
                                          Path = (string)repo.Element("Path"),
                                          Description = (string)repo.Element("Description")
                                      }).ToList();
                    listViewRepos.ItemsSource = repositoryInfo;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing configuration file");
            }
        }

        /// <summary>
        /// Saves the currently active repositories to the file
        /// </summary>
        private void SaveCurrentRepos()
        {
            try
            {
                XDocument doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Repository Information"),
                    new XElement("Repositories",
                        from repo in this.repositoryInfo
                        select new XElement("Repo",
                            new XElement("Name", repo.Name),
                            new XElement("Path", repo.Path),
                            new XElement("Description", repo.Description))));

                doc.Save(RepoConfigXml);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the current state.");
            }
        }

        private void listViewRepos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            RepoInfo selectedItem = (RepoInfo)listViewRepos.SelectedItem;
            if (this.selectedRepoPath == selectedItem.Path)
                return;
            else
            {

                try
                {
                    LibSvnChangeSet.SvnChangeSetMaker changeset = new LibSvnChangeSet.SvnChangeSetMaker();
                    List<string> modifiedFileList = changeset.getModifiedFiles(selectedItem.Path);
                    if (modifiedFileList == null)
                    {
                        selectedRepoPath = string.Empty;
                        changeListControlbar.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        selectedRepoPath = selectedItem.Path;

                        this.modifiedFileInfo = (from path in modifiedFileList
                                                 select new ChangedFilesInfo()
                                                 {
                                                     Path = path,
                                                     Selected = true,
                                                     Status = "Modified"
                                                 }).ToList();
                        listViewChanges.ItemsSource = modifiedFileInfo;
                        if(modifiedFileInfo.Count > 0)
                            changeListControlbar.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error getting changelist from the specified path");
                }
            }
        }

        private void buttonSaveChangeList_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            if( folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                !string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
            {
                List<string> filetoSave = (from change in this.modifiedFileInfo
                                           where change.Selected
                                           select change.Path).ToList();

                if (filetoSave.Count > 0)
                {
                    LibSvnChangeSet.SvnChangeSetMaker changeset = new LibSvnChangeSet.SvnChangeSetMaker();
                    changeset.createChangeList(filetoSave, selectedRepoPath, folderBrowserDialog1.SelectedPath);
                }
            }
        }
    }
}
