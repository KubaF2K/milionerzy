using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace milionerzy.Game;

internal class Stage
{
    private readonly Brush _activeColor = new SolidColorBrush(Colors.Chartreuse);
    private readonly Brush _inactiveColor = new SolidColorBrush(Colors.Black);
    private readonly Brush _guaranteedColor = new SolidColorBrush(Colors.Blue);
    public bool IsGuaranteed { get; }

    public int Value { get; }
    public Question Question { get; set; }
    
    public TextBlock StageTextBlock { get; }

    public bool IsCurrent
    {
        set
        {
            if (value)
                StageTextBlock.Foreground = _activeColor;
            else if (IsGuaranteed)
                StageTextBlock.Foreground = _guaranteedColor;
            else
                StageTextBlock.Foreground = _inactiveColor;
        }
    }

    public Stage(int id, int value, bool isGuaranteed, Question question)
    {
        Value = value;
        Question = question;
        IsGuaranteed = isGuaranteed;
        StageTextBlock = new TextBlock
        {
            Text = id + ". " + value + " złotych"
        };
        IsCurrent = false;
    }
}
public partial class GameWindow
{
    
    private GameSelectWindow.GameType _gameType;

    private readonly List<Question> _questions;
    private Stage[] _stages;
    private bool[] _lines = { true, true, true, false };
    private bool[] _currActiveAnswers = { true, true, true, true };

    private int _currentStage;
    private int CurrentStageId
    {
        get => _currentStage;
        set
        {
            _stages[CurrentStageId].IsCurrent = false;
            _currentStage = value;
            _stages[CurrentStageId].IsCurrent = true;
        }
    }

    private Stage CurrentStage => _stages[CurrentStageId];
    private Question CurrentQuestion => CurrentStage.Question;

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private Question? GetQuestion(int diff)
    {
        var possibleQuestions = _questions.Where(question => question.Difficulty == diff);
        for (var idiff = diff - 1; !possibleQuestions.Any(); idiff--)
        {
            if (idiff < 0) return null;
            var idiff1 = idiff;
            possibleQuestions = _questions.Where(question => question.Difficulty == idiff1);
        }

        return possibleQuestions.ElementAt(new Random().Next(possibleQuestions.Count()));
    }
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private Question? GetQuestion(Question except)
    {
        if (except.Difficulty == null)
            throw new NullReferenceException();
        var diff = (int) except.Difficulty;
        var possibleQuestions = _questions.Where(question => question.Difficulty == diff && question.QuestionId != except.QuestionId);
        for (var idiff = diff - 1; !possibleQuestions.Any(); idiff--)
        {
            if (idiff < 0) return null;
            var idiff1 = idiff;
            possibleQuestions = _questions.Where(question => question.Difficulty == idiff1);
        }

        return possibleQuestions.ElementAt(new Random().Next(possibleQuestions.Count()));
    }

    private void DrawQuestionInfo()
    {
        TextQuestion.Text = CurrentQuestion.QuestionText;
        ButtonAnswer1.Content = _currActiveAnswers[0] ? "A: " + CurrentQuestion.Answer1 : "";
        ButtonAnswer2.Content = _currActiveAnswers[1] ? "B: " + CurrentQuestion.Answer2 : "";
        ButtonAnswer3.Content = _currActiveAnswers[2] ? "C: " + CurrentQuestion.Answer3 : "";
        ButtonAnswer4.Content = _currActiveAnswers[3] ? "D: " + CurrentQuestion.Answer4 : "";
        
        if (_currActiveAnswers[0])
        {
            ButtonAnswer1.Background = new SolidColorBrush
            {
                Color = Colors.White
            };
        }
        else
        {
            ButtonAnswer1.Background = new SolidColorBrush
            {
                Color = Colors.Gray
            };
        }
        if (_currActiveAnswers[1])
        {
            ButtonAnswer2.Background = new SolidColorBrush
            {
                Color = Colors.White
            };
        }
        else
        {
            ButtonAnswer2.Background = new SolidColorBrush
            {
                Color = Colors.Gray
            };
        }
        if (_currActiveAnswers[2])
        {
            ButtonAnswer3.Background = new SolidColorBrush
            {
                Color = Colors.White
            };
        }
        else
        {
            ButtonAnswer3.Background = new SolidColorBrush
            {
                Color = Colors.Gray
            };
        }
        if (_currActiveAnswers[3])
        {
            ButtonAnswer4.Background = new SolidColorBrush
            {
                Color = Colors.White
            };
        }
        else
        {
            ButtonAnswer4.Background = new SolidColorBrush
            {
                Color = Colors.Gray
            };
        }
    }
    
    public GameWindow(GameSelectWindow.GameType gameType)
    {
        using (var context = new QuestionDbContext())
            _questions = context.Questions.ToList();
        
        _gameType = gameType;
        _stages = gameType switch
        {
            GameSelectWindow.GameType.Classic => new[]
            {
                new Stage(1, 100, false, GetQuestion(0) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(2, 200, false, GetQuestion(1) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(3, 300, false, GetQuestion(2) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(4, 500, false, GetQuestion(3) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(5, 1000, true, GetQuestion(4) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(6, 2000, false, GetQuestion(5) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(7, 4000, false, GetQuestion(6) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(8, 8000, false, GetQuestion(7) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(9, 16000, false, GetQuestion(8) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(10, 32000, true, GetQuestion(9) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(11, 64000, false, GetQuestion(10) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(12, 125000, false, GetQuestion(11) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(13, 250000, false, GetQuestion(12) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(14, 500000, false, GetQuestion(13) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(15, 1000000, true, GetQuestion(14) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!"))
            },
            GameSelectWindow.GameType.Standard => new[]
            {
                new Stage(1, 500, false, GetQuestion(3) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(2, 1000, true, GetQuestion(4) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(3, 2000, false, GetQuestion(5) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(4, 5000, false, GetQuestion(6) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(5, 10000, false, GetQuestion(7) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(6, 20000, false, GetQuestion(8) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(7, 40000, true, GetQuestion(9) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(8, 75000, false, GetQuestion(10) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(9, 125000, false, GetQuestion(11) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(10, 250000, false, GetQuestion(12) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(11, 500000, false, GetQuestion(13) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(12, 1000000, true, GetQuestion(14) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!"))
            },
            GameSelectWindow.GameType.FourLines => new[]
            {
                new Stage(1, 500, false, GetQuestion(3) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(2, 1000, true, GetQuestion(4) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(3, 2000, false, GetQuestion(5) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(4, 5000, false, GetQuestion(6) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(5, 10000, false, GetQuestion(7) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(6, 20000, false, GetQuestion(8) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(7, 40000, false, GetQuestion(9) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(8, 75000, false, GetQuestion(10) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(9, 125000, false, GetQuestion(11) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(10, 250000, false, GetQuestion(12) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(11, 500000, false, GetQuestion(13) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!")),
                new Stage(12, 1000000, true, GetQuestion(14) ?? throw new ArgumentException("Brak odpowiednich pytań w bazie!"))
            },
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };

        _lines[3] = gameType == GameSelectWindow.GameType.FourLines;
        
        CurrentStageId = 0;
        
        InitializeComponent();
        
        foreach (var stage in _stages)
            StackStages.Children.Add(stage.StageTextBlock);
        
        if (gameType != GameSelectWindow.GameType.FourLines)
            ButtonLineChange.Visibility = Visibility.Collapsed;
        
        DrawQuestionInfo();
    }

    private void CheckAnswer(int answerId)
    {
        if (!_currActiveAnswers[answerId - 1])
            return;
        if (CurrentQuestion.CorrectAnswerId == answerId)
        {
            MessageBox.Show("Dobree");
            if (CurrentStage.Value == 1000000)
            {
                MessageBox.Show("Wygrałeś 1000000 zł");
                Close();
            }
            else
            {
                CurrentStageId++;
                Array.Fill(_currActiveAnswers, true);
                DrawQuestionInfo();
            }
        }
        else
        {
            WrongAnswer();
        }
    }
    private void ButtonAnswer1_OnClick(object sender, RoutedEventArgs e)
    {
        CheckAnswer(1);
    }

    private void ButtonAnswer2_OnClick(object sender, RoutedEventArgs e)
    {
        CheckAnswer(2);
    }

    private void ButtonAnswer3_OnClick(object sender, RoutedEventArgs e)
    {
        CheckAnswer(3);
    }

    private void ButtonAnswer4_OnClick(object sender, RoutedEventArgs e)
    {
        CheckAnswer(4);
    }

    private void ButtonQuit_OnClick(object sender, RoutedEventArgs e)
    {
        if (CurrentStageId == 0)
        {
            MessageBox.Show("Odchodzisz z pustymi rękami!");
            Close();
            return;
        }

        MessageBox.Show("Wygrałeś " + _stages[CurrentStageId - 1].Value + " zł!");
        Close();
    }

    private void WrongAnswer()
    {
        for (var i = CurrentStageId - 1; i >= 0; i--)
        {
            if (!_stages[i].IsGuaranteed) continue;
            MessageBox.Show("Zła odpowiedź! Wygrałeś " + _stages[i].Value + " zł!");
            Close();
            return;
        }

        MessageBox.Show("Zła odpowiedź! Odchodzisz z pustymi rękami.");
        Close();
    }

    private void ButtonLineHalfHalf_OnClick(object sender, RoutedEventArgs e)
    {
        if(!_lines[0])
            return;
        
        for (var i = 0; i < 2; i++)
        {
            int buttonToHideId;
            var randomizer = new Random();
            
            do buttonToHideId = randomizer.Next(4); 
            while (buttonToHideId == CurrentQuestion.CorrectAnswerId-1 || !_currActiveAnswers[buttonToHideId]);
            
            _currActiveAnswers[buttonToHideId] = false;
        }
        DrawQuestionInfo();
        
        _lines[0] = false;
        ButtonLineHalfHalf.Background = new SolidColorBrush
        {
            Color = Colors.Gray
        };
    }

    private void ButtonLineAudience_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_lines[1])
            return;
        
        var randomizer = new Random();
        
        var chances = new int[4];
        var chanceTotal = 0;
        var correctAnswer = CurrentQuestion.CorrectAnswerId - 1;
        
        for (var i = 0; i < chances.Length; i++)
        {
            if (i == correctAnswer)
                chances[i] = randomizer.Next(40, 60);
            else
                chances[i] = randomizer.Next(10, 50);
            chanceTotal += chances[i];
        }

        var normalizedChances = new float[4];
        
        for (var i = 0; i < normalizedChances.Length; i++)
            normalizedChances[i] = (float) chances[i] / chanceTotal * 100.0f;
        
        new ATAWindow(normalizedChances).Show();
        
        _lines[1] = false;
        ButtonLineAudience.Background = new SolidColorBrush
        {
            Color = Colors.Gray
        };
    }

    private void ButtonLineFriend_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_lines[2])
            return;
        if (CurrentQuestion.CorrectAnswerId == null)
            throw new NullReferenceException();

        var randomizer = new Random();

        char[] answerNames = { 'A', 'B', 'C', 'D' };

        char answer;


        if (randomizer.Next(4) == 0)
        {
            var answers = new List<int>(new[] { 0, 1, 2, 3 });
            answers.Remove((int)(CurrentQuestion.CorrectAnswerId - 1));
            answer = answerNames[answers[randomizer.Next(3)]];
        }
        else
            answer = answerNames[(int)(CurrentQuestion.CorrectAnswerId - 1)];

        MessageBox.Show("Yyyy myślę że to będzie " + answer + ".");

        _lines[2] = false;
        ButtonLineFriend.Background = new SolidColorBrush
        {
            Color = Colors.Gray
        };
    }

    private void ButtonLineChange_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_lines[3])
            return;
        
        var newQuestion = GetQuestion(CurrentQuestion);
        if (newQuestion == null)
        {
            MessageBox.Show("Brak pytań do zamiany!");
            return;
        }

        CurrentStage.Question = newQuestion;
        Array.Fill(_currActiveAnswers, true);
        DrawQuestionInfo();

        _lines[3] = false;
        ButtonLineChange.Background = new SolidColorBrush
        {
            Color = Colors.Gray
        };
    }
}