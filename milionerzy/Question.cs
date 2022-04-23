using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace milionerzy;

public class Question
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public string? Answer1 { get; set; }
    public string? Answer2 { get; set; }
    public string? Answer3 { get; set; }
    public string? Answer4 { get; set; }
    public int? CorrectAnswerId { get; set; }
    public int? Difficulty { get; set; }
}