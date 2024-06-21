using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using System;
using System.Threading.Tasks;
using WinRT.Interop;
using Watcher;

namespace Watcher
{
    public sealed partial class SettingsPage : UserControl
    {
        private AppSettings _settings;

        public SettingsPage()
        {
            this.InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            _settings = AppSettings.Load();

            SourceFolderTextBox.Text = _settings.SourceFolder;
            ProcessedFolderTextBox.Text = _settings.ProcessedFolder;
            EmailFromTextBox.Text = _settings.EmailFrom;
            EmailToTextBox.Text = _settings.EmailTo;
            SmtpServerTextBox.Text = _settings.SmtpServer;
            SmtpPortTextBox.Text = _settings.SmtpPort.ToString();
            SmtpUserTextBox.Text = _settings.SmtpUser;
            SmtpPasswordTextBox.Text = _settings.SmtpPass;
            UseSslCheckBox.IsChecked = _settings.UseSsl;

            SettingsFilePathTextBlock.Text = $"Settings file path: {AppSettings.SettingsFilePath}";
        }

        private async void BrowseSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            var folder = await PickFolderAsync();
            if (folder != null)
            {
                SourceFolderTextBox.Text = folder.Path;
            }
        }

        private async void BrowseProcessedFolder_Click(object sender, RoutedEventArgs e)
        {
            var folder = await PickFolderAsync();
            if (folder != null)
            {
                ProcessedFolderTextBox.Text = folder.Path;
            }
        }

        private async Task<Windows.Storage.StorageFolder> PickFolderAsync()
        {
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(folderPicker, hwnd);

            return await folderPicker.PickSingleFolderAsync();
        }

        private async void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            _settings.SourceFolder = SourceFolderTextBox.Text;
            _settings.ProcessedFolder = ProcessedFolderTextBox.Text;
            _settings.EmailFrom = EmailFromTextBox.Text;
            _settings.EmailTo = EmailToTextBox.Text;
            _settings.SmtpServer = SmtpServerTextBox.Text;
            _settings.SmtpPort = int.Parse(SmtpPortTextBox.Text);
            _settings.SmtpUser = SmtpUserTextBox.Text;
            _settings.SmtpPass = SmtpPasswordTextBox.Text;
            _settings.UseSsl = UseSslCheckBox.IsChecked ?? false;

            if (string.IsNullOrWhiteSpace(_settings.SourceFolder) ||
                string.IsNullOrWhiteSpace(_settings.ProcessedFolder) ||
                string.IsNullOrWhiteSpace(_settings.EmailFrom) ||
                string.IsNullOrWhiteSpace(_settings.EmailTo) ||
                string.IsNullOrWhiteSpace(_settings.SmtpServer) ||
                string.IsNullOrWhiteSpace(_settings.SmtpUser) ||
                string.IsNullOrWhiteSpace(_settings.SmtpPass))
            {
                await ShowDialogAsync("Invalid Settings", "All settings fields must be filled out.");
                return;
            }

            _settings.Save();
            await ShowDialogAsync("Settings Saved", "Your settings have been saved successfully.");
        }

        private async Task ShowDialogAsync(string title, string content)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
