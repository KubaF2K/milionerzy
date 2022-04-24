using System.Windows;
using milionerzy.Editor;
using milionerzy.Game;

namespace milionerzy;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonGame_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new GameSelectWindow();
        if (dialog.ShowDialog() != true) return;
        new GameWindow(dialog.Type).Show();
    }

    private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
    {
        new EditorWindow().Show();
    }
        
    private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}