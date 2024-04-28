public class LeetCodeQuestion
{
    // The ID of the LeetCode question
    public string _id { get; set; }

    // The ID of the LeetCode question
    public string questionId { get; set; }

    // The title of the LeetCode question
    public string title { get; set; }

    // The difficulty of the LeetCode question
    public string difficulty { get; set; }

    // The question content
    public string content { get; set; }

    // The hints of the question
    public string[] hints { get; set; }

    // The current index of the hint array
    private int currentHintIndex = 0;

    /// <summary>
    /// This method retrieves the next hint in the array of hints
    /// </summary>
    /// <returns>The next hint for the LeetCode question</returns>
    public string GetNextHint()
    {
        if (hints == null || hints.Length == 0)
        {
            return "No hints available!";
        }
        
        if (currentHintIndex < hints.Length)
        {
            return hints[currentHintIndex++];
        }
        else
        {
            return "No more hints available!";
        }
    }

    public override string ToString()
    {
        return "Question ID: " + questionId + "\nTitle: " + title + "\nDifficulty: " + difficulty + "\nContent: " + content + "\nHints: " + string.Join(", ", hints);
    }
}

public class RootObject
{
    public LeetCodeQuestion question { get; set; }
}