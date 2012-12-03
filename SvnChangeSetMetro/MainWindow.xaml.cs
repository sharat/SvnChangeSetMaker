using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;

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
    }
}
