using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Castle.Core.Internal;

namespace milionerzy.Game;

internal class Stage
{
    private readonly Brush _activeColor = new SolidColorBrush(Colors.Chartreuse);
    private readonly Brush _inactiveColor = new SolidColorBrush(Colors.Black);
    private readonly Brush _guaranteedColor = new SolidColorBrush(Colors.Blue);
    private readonly bool _isGuaranteed;
    public Question Question { get; set; }
    
    public TextBlock StageTextBlock { get; }

    public bool IsCurrent
    {
        set
        {
            if (value)
                StageTextBlock.Foreground = _activeColor;
            else if (_isGuaranteed)
                StageTextBlock.Foreground = _guaranteedColor;
            else
                StageTextBlock.Foreground = _inactiveColor;
        }
    }

    public Stage(int id, int value, bool isGuaranteed, Question question)
    {
        Question = question;
        _isGuaranteed = isGuaranteed;
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

    private void DrawQuestionInfo()
    {
        TextQuestion.Text = CurrentQuestion.QuestionText;
        ButtonAnswer1.Content = CurrentQuestion.Answer1;
        ButtonAnswer2.Content = CurrentQuestion.Answer2;
        ButtonAnswer3.Content = CurrentQuestion.Answer3;
        ButtonAnswer4.Content = CurrentQuestion.Answer4;
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
                new Stage(1, 100, false, GetQuestion(0) ?? throw new ArgumentException()),
                new Stage(2, 200, false, GetQuestion(1) ?? throw new ArgumentException()),
                new Stage(3, 300, false, GetQuestion(2) ?? throw new ArgumentException()),
                new Stage(4, 500, false, GetQuestion(3) ?? throw new ArgumentException()),
                new Stage(5, 1000, true, GetQuestion(4) ?? throw new ArgumentException()),
                new Stage(6, 2000, false, GetQuestion(5) ?? throw new ArgumentException()),
                new Stage(7, 4000, false, GetQuestion(6) ?? throw new ArgumentException()),
                new Stage(8, 8000, false, GetQuestion(7) ?? throw new ArgumentException()),
                new Stage(9, 16000, false, GetQuestion(8) ?? throw new ArgumentException()),
                new Stage(10, 32000, true, GetQuestion(9) ?? throw new ArgumentException()),
                new Stage(11, 64000, false, GetQuestion(10) ?? throw new ArgumentException()),
                new Stage(12, 125000, false, GetQuestion(11) ?? throw new ArgumentException()),
                new Stage(13, 250000, false, GetQuestion(12) ?? throw new ArgumentException()),
                new Stage(14, 500000, false, GetQuestion(13) ?? throw new ArgumentException()),
                new Stage(15, 1000000, true, GetQuestion(14) ?? throw new ArgumentException())
            },
            GameSelectWindow.GameType.Standard => new[]
            {
                new Stage(1, 500, false, GetQuestion(3) ?? throw new ArgumentException()),
                new Stage(2, 1000, true, GetQuestion(4) ?? throw new ArgumentException()),
                new Stage(3, 2000, false, GetQuestion(5) ?? throw new ArgumentException()),
                new Stage(4, 5000, false, GetQuestion(6) ?? throw new ArgumentException()),
                new Stage(5, 10000, false, GetQuestion(7) ?? throw new ArgumentException()),
                new Stage(6, 20000, false, GetQuestion(8) ?? throw new ArgumentException()),
                new Stage(7, 40000, true, GetQuestion(9) ?? throw new ArgumentException()),
                new Stage(8, 75000, false, GetQuestion(10) ?? throw new ArgumentException()),
                new Stage(9, 125000, false, GetQuestion(11) ?? throw new ArgumentException()),
                new Stage(10, 250000, false, GetQuestion(12) ?? throw new ArgumentException()),
                new Stage(11, 500000, false, GetQuestion(13) ?? throw new ArgumentException()),
                new Stage(12, 1000000, true, GetQuestion(14) ?? throw new ArgumentException())
            },
            GameSelectWindow.GameType.FourLines => new[]
            {
                new Stage(1, 500, false, GetQuestion(3) ?? throw new ArgumentException()),
                new Stage(2, 1000, true, GetQuestion(4) ?? throw new ArgumentException()),
                new Stage(3, 2000, false, GetQuestion(5) ?? throw new ArgumentException()),
                new Stage(4, 5000, false, GetQuestion(6) ?? throw new ArgumentException()),
                new Stage(5, 10000, false, GetQuestion(7) ?? throw new ArgumentException()),
                new Stage(6, 20000, false, GetQuestion(8) ?? throw new ArgumentException()),
                new Stage(7, 40000, false, GetQuestion(9) ?? throw new ArgumentException()),
                new Stage(8, 75000, false, GetQuestion(10) ?? throw new ArgumentException()),
                new Stage(9, 125000, false, GetQuestion(11) ?? throw new ArgumentException()),
                new Stage(10, 250000, false, GetQuestion(12) ?? throw new ArgumentException()),
                new Stage(11, 500000, false, GetQuestion(13) ?? throw new ArgumentException()),
                new Stage(12, 1000000, true, GetQuestion(14) ?? throw new ArgumentException())
            },
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };

        CurrentStageId = 0;
        
        InitializeComponent();
        
        foreach (var stage in _stages)
            StackStages.Children.Add(stage.StageTextBlock);
        
        if (gameType != GameSelectWindow.GameType.FourLines)
            ButtonLineChange.Visibility = Visibility.Collapsed;
        
        DrawQuestionInfo();
    }
}