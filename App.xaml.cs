using OllamaSharp;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Net.Http;

namespace CodingAssistantBox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private OllamaApiClient? ollama;
        private Uri? uri;

        public OllamaApiClient? Model 
        {
            get { return ollama; }
            private set { ollama = value; } 
        }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            uri = new Uri("http://localhost:11434");
            ollama = new OllamaApiClient(uri);
            ollama.SelectedModel = "deepseek-coder:6.7b"; // Default

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                } catch
                {
                    MessageBox.Show("START OLLAMA, EXITING...", "Error", MessageBoxButton.OK);
                    Application.Current.Shutdown();
                }
            }
        }
    }

}
