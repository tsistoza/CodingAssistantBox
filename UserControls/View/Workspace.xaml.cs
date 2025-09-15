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
    public partial class Workspace : UserControl
    {

        public class TreeViewParentItem
        {
            public string Name { get; set; } = string.Empty;
            public string FilePath { get; set; } = string.Empty;
            public ObservableCollection<TreeViewParentItem> Children { get; set; }

            public TreeViewParentItem(string filePath, bool isFile)
            {
                this.Children = new ObservableCollection<TreeViewParentItem>();
                if (isFile) Name = System.IO.Path.GetFileName(filePath)!;
                else Name = System.IO.Path.GetFileName(filePath)!;
                FilePath = filePath;
            }
        }

        private ObservableCollection<TreeViewParentItem> rootItems;

        public ObservableCollection<TreeViewParentItem> RootItems
        {
            get { return rootItems; }
            private set { rootItems = value; }
        }

        public Workspace()
        {
            DataContext = this;
            rootItems = new ObservableCollection<TreeViewParentItem>();
            InitializeComponent();
        }

        private void AddFileToContext(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            TreeViewParentItem item = (TreeViewParentItem)menuItem.DataContext;
            string filePath = item.FilePath;
            CodingAssistantBox.MainWindow.ContextItem contextItem = new CodingAssistantBox.MainWindow.ContextItem(filePath);
            MainWindow parentWindow = (Window.GetWindow(this) as MainWindow)!;
            parentWindow.ContextFiles.Add(contextItem);
        }

        private void fileBrowserBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            bool? success = openFolderDialog.ShowDialog();
            if (success == true)
            {
                string selectedFolderPath = openFolderDialog.FolderName;

                string[] files = Directory.GetFiles(selectedFolderPath, "*.*", SearchOption.TopDirectoryOnly);
                string[] folders = Directory.GetDirectories(selectedFolderPath);

                foreach (string folder in folders)
                {
                    TreeViewParentItem tree = new TreeViewParentItem(folder, false);
                    GetFilesInSubDir(folder, ref tree);
                    RootItems.Add(tree);
                }

                foreach (string file in files)
                    RootItems.Add(new TreeViewParentItem(file, true));
            }
        }

        private void GetFilesInSubDir(string folderPath, ref TreeViewParentItem tree)
        {
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
            string[] folders = Directory.GetDirectories(folderPath);

            foreach (string folder in folders)
            {
                TreeViewParentItem subTree2 = new TreeViewParentItem(folder, false);
                GetFilesInSubDir(folder, ref subTree2);
                tree.Children.Add(subTree2);
            }

            foreach (string file in files)
            {
                TreeViewParentItem subTree1 = new TreeViewParentItem(file, true);
                tree.Children.Add(subTree1);
            }

            return;
        }
    }
}
