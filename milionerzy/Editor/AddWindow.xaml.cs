using System;
using System.Windows;

namespace milionerzy.Editor;

public partial class AddWindow
{
    public readonly Question Question;
    public AddWindow(Question? question = null)
    {
        InitializeComponent();

        if (question == null)
        {
            RadioAnswer1.IsChecked = true;
            Question = new Question();
        }
        else
        {
            TextQuestion.Text = question.QuestionText;
            TextAnswer1.Text = question.Answer1;
            TextAnswer2.Text = question.Answer2;
            TextAnswer3.Text = question.Answer3;
            TextAnswer4.Text = question.Answer4;
            switch (question.CorrectAnswerId)
            {
                case 1:
                    RadioAnswer1.IsChecked = true;
                    break;
                case 2:
                    RadioAnswer2.IsChecked = true;
                    break;
                case 3:
                    RadioAnswer3.IsChecked = true;
                    break;
                case 4:
                    RadioAnswer4.IsChecked = true;
                    break;
            }

            TextDiff.Text = question.Difficulty.ToString();

            Question = question;
        }
    }

    private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
    {
        int diff;
        try
        {
            diff = Convert.ToInt32(TextDiff.Text);
            if (diff is < 0 or > 11) throw new FormatException();
        }
        catch (FormatException)
        {
            MessageBox.Show("Podano błędny poziom trudności! Podaj wartość od 0 do 11");
            return;
        }

        Question.QuestionText = TextQuestion.Text;
        Question.Answer1 = TextAnswer1.Text;
        Question.Answer2 = TextAnswer2.Text;
        Question.Answer3 = TextAnswer3.Text;
        Question.Answer4 = TextAnswer4.Text;
        
        if (RadioAnswer1.IsChecked == true)
        {
            Question.CorrectAnswerId = 1;
        }
        else if (RadioAnswer2.IsChecked == true)
        {
            Question.CorrectAnswerId = 2;
        }
        else if (RadioAnswer3.IsChecked == true)
        {
            Question.CorrectAnswerId = 3;
        }
        else if (RadioAnswer4.IsChecked == true)
        {
            Question.CorrectAnswerId = 4;
        }

        Question.Difficulty = diff;

        DialogResult = true;
    }

    private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}