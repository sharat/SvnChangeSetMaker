using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System;
using MahApps.Metro;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using LibSvnChangeSet;

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
            public System.Windows.Visibility ShowProgress { get; set; }
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

        private void buttonAddRepo_Click(object sender, RoutedEventArgs e)
        {
            if (!SvnChangeSetMaker.IsWorkingCopy(textBoxRepoPath.Text))
            {
                MessageBox.Show("The given directory is not a SVN repository...", "Invalid Repository", MessageBoxButton.OK, MessageBoxImage.Error);            
                return;
            }

            if (!repositoryInfo.Any((repo) => repo.Path == textBoxRepoPath.Text))
            {
                repositoryInfo.Add(new RepoInfo()
                {
                    Path = textBoxRepoPath.Text,
                    Name = "Testing",
                    Description = "Sample Description",
                    ShowProgress = Visibility.Hidden
                });
                listViewRepos.Items.Refresh();
                textBoxRepoPath.Text = string.Empty;
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
                                          Description = (string)repo.Element("Description"),
                                          ShowProgress = Visibility.Hidden
                                      }).ToList();
                    listViewRepos.ItemsSource = repositoryInfo;
                    listViewRepos.Items.Refresh();
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

        void modifiedListProgress(object sender, ProgressEventArgs e)
        {
            this.modifiedFileInfo.Add(new ChangedFilesInfo() { Path = e.FileName, Status = e.FileStatus.ToString(), Selected = true });
            listViewChanges.Items.Refresh();
            showMessage("Checking for changes...", false);
        }

        void modifiedListCompleted(object sender, CompletedEventArgs e)
        {
            listViewChanges.ItemsSource = this.modifiedFileInfo;
            RepoInfo v = (RepoInfo)(from info in repositoryInfo 
                    where info.Path == selectedRepoPath
                    select info).First();
            if (v != null)
            {
                v.ShowProgress = System.Windows.Visibility.Hidden;
                listViewRepos.Items.Refresh();
            }

            listViewChanges.Items.Refresh();
            RepoInfo selectedItem = (RepoInfo)listViewRepos.SelectedItem;
            selectedItem.ShowProgress = System.Windows.Visibility.Hidden;
            if (!string.IsNullOrEmpty(e.ErrorMessage))
                showMessage(e.ErrorMessage, true);
            else if (this.modifiedFileInfo.Count > 0)
                showMessage("Found " + modifiedFileInfo.Count.ToString() + " changes...", false);
            else
                showMessage("No changes found...", true);
        }
        
        private void listViewRepos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RepoInfo selectedItem = (RepoInfo)listViewRepos.SelectedItem;
            if (selectedItem != null && this.selectedRepoPath == selectedItem.Path)
                return;
            else
            {
                if (this.modifiedFileInfo != null)
                    this.modifiedFileInfo.Clear();
                else
                    this.modifiedFileInfo = new List<ChangedFilesInfo>();

                listViewChanges.ItemsSource = this.modifiedFileInfo;
                try
                {
                    showMessage(string.Empty, true);
                    LibSvnChangeSet.SvnChangeSetMaker changeset = new LibSvnChangeSet.SvnChangeSetMaker();
                    this.selectedRepoPath = selectedItem.Path;
                    selectedItem.ShowProgress = System.Windows.Visibility.Visible;
                    changeset.getModifiedFilesAsync(
                        selectedItem.Path,
                        new EventHandler<ProgressEventArgs>(modifiedListProgress),
                        new EventHandler<CompletedEventArgs>(modifiedListCompleted));
                }
                catch (Exception ex)
                {
                    showMessage("Error finding modifications for the selected archive", true);
                    this.selectedRepoPath = string.Empty;
                }
                listViewRepos.Items.Refresh();
            }
        }

        private void buttonSaveChangeList_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if( folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                !string.IsNullOrEmpty(folderBrowser.SelectedPath))
            {
                List<string> filetoSave = (from change in this.modifiedFileInfo
                                           where change.Selected
                                           select change.Path).ToList();

                if (filetoSave.Count > 0)
                {
                    LibSvnChangeSet.SvnChangeSetMaker changeset = new LibSvnChangeSet.SvnChangeSetMaker();
                    changeset.createChangeList(filetoSave, selectedRepoPath, folderBrowser.SelectedPath);
                    if (checkboxSaveInZip.IsChecked == true)
                        if(SvnChangeSetHelper.zipChangeSetDir(selectedRepoPath, folderBrowser.SelectedPath+"\\changeset.zip"))
                            MessageBox.Show("Failed to create zip file at - " + folderBrowser.SelectedPath);

                    MessageBox.Show("Saved change set to " + folderBrowser.SelectedPath);
                }
            }
        }

        #region context menu handlers
        private void listViewRepos_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            listViewRepos.ContextMenu.IsOpen = true;
        }

        private void MenuItem_OpenInExpolorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RepoInfo rp = (RepoInfo)listViewRepos.SelectedItem;
                Process.Start(rp.Path);
            }
            catch 
            {
                MessageBox.Show("Error opening the given path");
            }
        }

        private void MenuItem_delete_Click(object sender, RoutedEventArgs e)
        {
            RepoInfo rp = (RepoInfo )listViewRepos.SelectedItem;
            textBoxRepoPath.Text = rp.Path;
            this.repositoryInfo.Remove(rp);
            listViewRepos.Items.Refresh();
        }

        private void showMessage(string message, bool bShowOnlyMessageArea)
        {
            labelErrorChangeList.Content = message;
            controlbarSelectDeselect.Visibility = bShowOnlyMessageArea ? Visibility.Hidden : Visibility.Visible;
            controlbarSave.Visibility = bShowOnlyMessageArea ? Visibility.Hidden : Visibility.Visible;
        }
        #endregion

    }
}
