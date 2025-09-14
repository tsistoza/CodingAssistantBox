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
        public Uri? HostLink
        {
            get { return uri; }
            private set { uri = value; }
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            uri = new Uri("http://localhost:11434");
            ollama = new OllamaApiClient(uri);
            ollama.SelectedModel = "deepseek-coder:6.7b"; // Default
        }
    }

}
