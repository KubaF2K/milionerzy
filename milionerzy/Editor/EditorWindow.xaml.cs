using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;

namespace milionerzy.Editor;

public partial class EditorWindow
{
    private readonly QuestionDbContext _context = new();
    public EditorWindow()
    {
        InitializeComponent();
        
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "ID", Binding = new Binding("QuestionId")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Pytanie", Binding = new Binding("QuestionText")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Odpowiedź 1", Binding = new Binding("Answer1")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Odpowiedź 2", Binding = new Binding("Answer2")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Odpowiedź 3", Binding = new Binding("Answer3")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Odpowiedź 4", Binding = new Binding("Answer4")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Poprawna odpowiedź", Binding = new Binding("CorrectAnswerId")});
        QuestionDg.Columns.Add(new DataGridTextColumn {Header = "Trudność", Binding = new Binding("Difficulty")});
        QuestionDg.AutoGenerateColumns = false;
        _context.Questions.Load();
        QuestionDg.ItemsSource = _context.Questions.Local.ToObservableCollection();
    }

    private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new AddWindow();
        if (dialog.ShowDialog() != true) return;
        _context.Questions.Add(dialog.Question);
        _context.SaveChanges();
        QuestionDg.Items.Refresh();
    }

    private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new AddWindow((Question)QuestionDg.SelectedItem);
        if (dialog.ShowDialog() != true) return;
        _context.SaveChanges();
        QuestionDg.Items.Refresh();
    }

    private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
    {
        _context.Questions.Remove((Question)QuestionDg.SelectedItem);
        _context.SaveChanges();
        QuestionDg.Items.Refresh();
    }
}