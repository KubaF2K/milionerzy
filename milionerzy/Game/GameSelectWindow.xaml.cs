using System.Linq;
using System.Windows;

namespace milionerzy.Game;

public partial class GameSelectWindow
{
    private int _questionCount;
    public enum GameType
    {
        Classic, Standard, FourLines
    }

    public GameType Type;
    
    public GameSelectWindow()
    {
        using (var context = new QuestionDbContext())
            _questionCount = context.Questions.Count();
        
        InitializeComponent();
    }

    private void ButtonClassic_OnClick(object sender, RoutedEventArgs e)
    {
        if (_questionCount < 15)
        {
            MessageBox.Show("Za mało pytań!");
            DialogResult = false;
            return;
        }
        Type = GameType.Classic;
        DialogResult = true;
    }

    private void ButtonStandard_OnClick(object sender, RoutedEventArgs e)
    {
        if (_questionCount < 12)
        {
            MessageBox.Show("Za mało pytań!");
            DialogResult = false;
            return;
        }
        Type = GameType.Standard;
        DialogResult = true;
    }

    private void Button4lifelines_OnClick(object sender, RoutedEventArgs e)
    {
        if (_questionCount < 12)
        {
            MessageBox.Show("Za mało pytań!");
            DialogResult = false;
            return;
        }
        Type = GameType.FourLines;
        DialogResult = true;
    }
}