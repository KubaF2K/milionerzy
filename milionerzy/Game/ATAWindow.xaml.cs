using System.Collections.Generic;
using System.Windows;

namespace milionerzy.Game;

public partial class ATAWindow : Window
{
    public ATAWindow(IReadOnlyList<float> chances)
    {
        InitializeComponent();
        RectangleAnswer1.Width = chances[0] * 5;
        LabelAnswer1.Content = chances[0] + " %";
        RectangleAnswer2.Width = chances[1] * 5;
        LabelAnswer2.Content = chances[1] + " %";
        RectangleAnswer3.Width = chances[2] * 5;
        LabelAnswer3.Content = chances[2] + " %";
        RectangleAnswer4.Width = chances[3] * 5;
        LabelAnswer4.Content = chances[3] + " %";
    }
}