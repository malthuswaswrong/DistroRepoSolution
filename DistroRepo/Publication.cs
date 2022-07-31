namespace DistroRepo;

public class Publication
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<EvaluationGroup>? EvaluationGroups { get; set; }
}
