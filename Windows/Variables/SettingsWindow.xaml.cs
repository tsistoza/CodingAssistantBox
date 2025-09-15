using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace CodingAssistantBox.Windows.Variables
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public int ChatHistoryNum
        {
            get
            {
                MainWindow mainWindow = (Application.Current.MainWindow as MainWindow)!;
                return mainWindow.numMessagesInChat;
            }
            set
            {
                MainWindow mainWindow = (Application.Current.MainWindow as MainWindow)!;
                mainWindow.numMessagesInChat = value;
                OnPropertyChanged(nameof(ChatHistoryNum));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SettingsWindow()
        {
            DataContext = this;
            InitializeComponent();
        }


        private void IncreaseChat_Click(object sender, RoutedEventArgs e)
        {
            if (ChatHistoryNum > 9) return;
            ChatHistoryNum++;
        }

        private void DecreaseChat_Click(object sender, RoutedEventArgs e)
        {
            if (ChatHistoryNum == 1) return;
            ChatHistoryNum--;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
