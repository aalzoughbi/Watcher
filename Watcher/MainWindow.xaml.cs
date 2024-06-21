using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Watcher;

namespace Watcher
{
    public sealed partial class MainWindow : Window
    {
        private FolderMonitor _folderMonitor;
        private LoggingService _loggingService;
        private EmailService _emailService;
        private FileManagementService _fileManagementService;
        private AppSettings _settings;

        public MainWindow()
        {
            this.InitializeComponent();

            _loggingService = new LoggingService();
            LogsListView.ItemsSource = _loggingService.Logs;

            LoadSettings();
        }

        private void LoadSettings()
        {
            _settings = AppSettings.Load();

            if (string.IsNullOrWhiteSpace(_settings.SourceFolder) ||
                string.IsNullOrWhiteSpace(_settings.ProcessedFolder) ||
                string.IsNullOrWhiteSpace(_settings.EmailFrom) ||
                string.IsNullOrWhiteSpace(_settings.EmailTo) ||
                string.IsNullOrWhiteSpace(_settings.SmtpServer) ||
                string.IsNullOrWhiteSpace(_settings.SmtpUser) ||
                string.IsNullOrWhiteSpace(_settings.SmtpPass))
            {
                _loggingService.Log("One or more settings are not set. Please configure the settings.");
                return;
            }

            _emailService = new EmailService(
                _settings.EmailFrom,
                _settings.EmailTo,
                _settings.SmtpServer,
                _settings.SmtpPort,
                _settings.SmtpUser,
                _settings.SmtpPass,
                _settings.UseSsl
            );

            _fileManagementService = new FileManagementService(_settings.ProcessedFolder);

            _folderMonitor = new FolderMonitor(_settings.SourceFolder, OnNewFileDetected);
        }


        private async void OnNewFileDetected(string filePath)
        {
            _loggingService.Log($"New file detected: {filePath}");

            try
            {
                _emailService.SendEmail("New File", "A new file has been added.", filePath);
                _loggingService.Log($"Email sent for file: {filePath}");

                _fileManagementService.MoveFile(filePath);
                _loggingService.Log($"File moved to processed folder: {filePath}");
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Error: {ex.Message}");
                await ShowDialogAsync("Error", $"An error occurred: {ex.Message}");
            }
        }


        private async void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsPage = new SettingsPage();
            ContentDialog settingsDialog = new ContentDialog
            {
                Title = "Settings",
                Content = settingsPage,
                CloseButtonText = "Close",
                XamlRoot = MainGrid.XamlRoot // Use the XamlRoot from MainGrid
            };

            await settingsDialog.ShowAsync();

            // Reload settings after the dialog is closed to apply any changes
            LoadSettings();
        }


        private async Task ShowDialogAsync(string title, string content)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = MainGrid.XamlRoot
            };
            await dialog.ShowAsync();
        }


    }
}
