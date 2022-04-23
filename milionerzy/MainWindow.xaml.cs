using System;
using System.Windows;
using milionerzy.Editor;

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
        throw new NotImplementedException();
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