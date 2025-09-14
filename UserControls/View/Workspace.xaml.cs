using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

namespace CodingAssistantBox.UserControls.View
{
    /// <summary>
    /// Interaction logic for Workspace.xaml
    /// </summary>
    public partial class Workspace : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<string> workspaceFiles;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> WorkspaceFiles
        {
            get { return workspaceFiles; }
            private set 
            { 
                workspaceFiles = value;
            }
        }

        public Workspace()
        {
            DataContext = this;
            workspaceFiles = new ObservableCollection<string>();
            InitializeComponent();
        }

        private void fileBrowserBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            bool? success = openFolderDialog.ShowDialog();
            if (success == true)
            {
                string selectedFolderPath = openFolderDialog.FolderName;
                
                string[] files = Directory.GetFiles(selectedFolderPath, "*.*", SearchOption.AllDirectories);

                foreach (string file in files)
                    WorkspaceFiles.Add(System.IO.Path.GetFileName(file));
                MessageBox.Show(workspaceFiles.Count.ToString(), "Debug", MessageBoxButton.OK);
            }
        }
    }
}
