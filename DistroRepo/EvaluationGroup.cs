namespace DistroRepo;

public class EvaluationGroup
{
    List<List<Evaluation>>? Evaluations { get; set; }
    /// <summary>
    /// Outer List is an "OR" condition.  Inner list is an "AND" condition
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="caseSensitive"></param>
    /// <returns></returns>
    public bool Evaluate(Dictionary<string, string> attributes, bool caseSensitive = false)
    {
        if (Evaluations == null) return false;

        
        var result = Evaluations
            .Where(outer => outer != null)
            .Any(outer =>
                outer.Where(inner => inner != null)
                    .All(inner => inner.Evaluate(attributes, caseSensitive))
            );
        
        
        return result;
    }
}
