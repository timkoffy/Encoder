using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace Encoder;

public class MainWindow : Window
{
    private string? _fileContent;
    private string? _key;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = GetTopLevel(this);
        
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            
            _fileContent = await streamReader.ReadToEndAsync();
            var textBlock = this.FindControl<TextBlock>("FileNameTextBlock");
            textBlock.Text = files[0].Name;
            var button = this.FindControl<Button>("OpenFileButton");
            button.IsEnabled = false;
        }
    }

    private void EncodeButton_Clicked(object sender, RoutedEventArgs args)
    {
        if (string.IsNullOrEmpty(_fileContent))
        {
            var textBlock = this.FindControl<TextBlock>("FileNameTextBlock");
            textBlock.Text = "Open file first!";
            return;
        }
        
        var textBox = this.FindControl<TextBox>("KeyBox");
        _key = textBox.Text;
        
        if (string.IsNullOrEmpty(_key))
        {
            textBox.Watermark = "Enter key here!";
            return;
        }
        
        SaveFile(Program.EncodeText(_fileContent, _key));
    }
    
    private async void SaveFile(string text)
    {
        var topLevel = GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File"
        });

        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            
            await streamWriter.WriteLineAsync(text);
        }
    }
}
