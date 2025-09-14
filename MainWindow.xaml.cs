using Microsoft.Extensions.AI;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CodingAssistantBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OllamaApiClient ollama;
        private Uri uri;
        private List<string> chatHistory = new List<string>();
        private List<OllamaSharp.Models.Model> models = new List<OllamaSharp.Models.Model>();

        public MainWindow()
        {
            ollama = ((App)Application.Current).Model!;
            uri = ((App)Application.Current).HostLink!;
            InitializeComponent();
            CheckConnection();
        }

        private async void CheckConnection()
        {
            userInput.IsEnabled = false;
            while (true)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(uri);
                        response.EnsureSuccessStatusCode();
                        userInput.IsEnabled = true;
                        userPlaceholdler.Text = "Chat here...";
                        break;
                    }
                    catch
                    {
                        userPlaceholdler.Text = "Model Not Connected... Start Ollama";
                    }
                }
            }

            // List Models
            foreach (var model in await ollama.ListLocalModelsAsync())
            {
                MenuItem selector = new MenuItem();
                selector.Header = model.Name;
                selector.Click += ModelSelector_Click;
                selector.Foreground = Brushes.Black;
                ModelSelector.Items.Add(selector);
            }

            // Set Model
            NameModel.Header = ollama.SelectedModel;
        }

        private async Task<string> GenerateMessage(string msg)
        {
            string response = "";
            Chat chat = new Chat(ollama);
            userPlaceholdler.Text = "Generating...";
            userInput.Text = null;
            userInput.IsEnabled = false;
            

            string context = "";
            foreach (string history in chatHistory)
                context += history;
            context += "User: " + msg;

            await foreach (var answerToken in chat.SendAsync(context))
                response += answerToken;

            if (chatHistory.Count > 5)
                chatHistory.RemoveAt(0);
            chatHistory.Add("User: " + msg + "\nResponse: " + response + "\n");

            return response;
        }

        private void AppendTextBlock(string text, bool isUser)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = 16,
                LineHeight = 15,
                TextWrapping = TextWrapping.Wrap,
                Foreground = (isUser) ? Brushes.LightGray : Brushes.White,
                Margin = new Thickness(20, 20, 20, 20)
            };
            DialogPanel.Children.Add(textBlock);
        }

        private void AppendCodeSnippet(string codeSnippet)
        {
            Border border = new Border
            {
                Margin = new Thickness(20, 20, 20, 20),
                Background = Brushes.DarkSlateGray,
            };
            TextBlock textBlock = new TextBlock
            {
                Background = Brushes.DarkSlateGray,
                Text = codeSnippet,
                FontSize = 13,
                FontFamily = new FontFamily("Consolas"),
                LineHeight = 15,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(20, 20, 20, 20)
            };

            border.Child = textBlock;
            DialogPanel.Children.Add(border);
        }

        // Event Handlers
        private void userInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(userInput.Text)) userPlaceholdler.Visibility = Visibility.Visible;
            else userPlaceholdler.Visibility = Visibility.Hidden;
        }

        private async void userInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AppendTextBlock("> " + userInput.Text, true);
                string response = await GenerateMessage(userInput.Text);

                string[] parts = Regex.Split(response, "```");
                bool isCodeSnippet = false;
                foreach (string block in parts)
                {
                    if (isCodeSnippet) AppendCodeSnippet(block);
                    else AppendTextBlock(block, false);
                    isCodeSnippet = !isCodeSnippet;
                }

                ScrollDialog.ScrollToEnd();
                userInput.IsEnabled = true;
                userPlaceholdler.Text = "Chat here...";

                userInput.Focus();
            }
        }

        private void clearDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogPanel.Children.Clear();
        }

        private void ModelSelector_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            ollama.SelectedModel = (string)item.Header;
            NameModel.Header = ollama.SelectedModel;
        }
    }
}