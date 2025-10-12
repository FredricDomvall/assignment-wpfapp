namespace Infrastructure.Models;
public class AnswerOutcome<T>
{
    public bool Statement { get; set; }
    public string? Answer { get; set; }
    public T? Outcome { get; set; }
}
