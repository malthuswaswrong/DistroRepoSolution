using System.Text.RegularExpressions;

namespace DistroRepo;

public class Evaluation
{
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public EvaluationType EvaluationType { get; set; }

    /// <summary>
    /// The evaluation is valid if the attribute value matches the evaluation type. First tries to cast the attribute to a DateTime, then to a double, then fails over to a straight string comparison
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="caseSensitive"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public bool Evaluate(Dictionary<string, string> attributes, bool strictEvaluation = false)
    {
        if (attributes is null) return false;
        
        if (attributes.TryGetValue(AttributeName, out string? target))
        {
            if(target is null) return false;
            
            if(strictEvaluation) return EvaluateStrict(attributes, true);
            
            bool caseSensitive = false;
            return EvaluationType switch
            {
                EvaluationType.Equals => (DateTime.TryParse(target, out DateTime dtt) && DateTime.TryParse(AttributeValue, out DateTime dta)) ? dtt == dta : (double.TryParse(target, out double lt) && double.TryParse(AttributeValue, out double la)) ? lt == la : EvaluateStrict(attributes, caseSensitive),
                EvaluationType.NotEquals => (DateTime.TryParse(target, out DateTime dtt) && DateTime.TryParse(AttributeValue, out DateTime dta)) ? dtt != dta : (double.TryParse(target, out double lt) && double.TryParse(AttributeValue, out double la)) ? lt != la : EvaluateStrict(attributes, caseSensitive),
                EvaluationType.GreaterThan => (DateTime.TryParse(target, out DateTime dtt) && DateTime.TryParse(AttributeValue, out DateTime dta)) ? dtt > dta : (double.TryParse(target, out double lt) && double.TryParse(AttributeValue, out double la)) ? lt > la : EvaluateStrict(attributes, caseSensitive),
                EvaluationType.GreaterThanOrEqual => (DateTime.TryParse(target, out DateTime dtt) && DateTime.TryParse(AttributeValue, out DateTime dta)) ? dtt >= dta : (double.TryParse(target, out double lt) && double.TryParse(AttributeValue, out double la)) ? lt >= la : EvaluateStrict(attributes, caseSensitive),
                EvaluationType.LessThan => (DateTime.TryParse(target, out DateTime dtt) && DateTime.TryParse(AttributeValue, out DateTime dta)) ? dtt < dta : (double.TryParse(target, out double lt) && double.TryParse(AttributeValue, out double la)) ? lt < la : EvaluateStrict(attributes, caseSensitive),
                EvaluationType.LessThanOrEqual => (DateTime.TryParse(target, out DateTime dtt) && DateTime.TryParse(AttributeValue, out DateTime dta)) ? dtt <= dta : (double.TryParse(target, out double lt) && double.TryParse(AttributeValue, out double la)) ? lt <= la : EvaluateStrict(attributes, caseSensitive),
                _ => EvaluateStrict(attributes, caseSensitive)
            };
        }
        
        return false;
    }

    /// <summary>
    /// The evaluation is valid if the attribute value matches the evaluation type. Only does a straight string comparison.  Does not try to cast to DateTime or double.
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="caseSensitive"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private bool EvaluateStrict(Dictionary<string, string> attributes, bool caseSensitive = true)
    {
        if (attributes is null) return false;

        if (attributes.TryGetValue(AttributeName, out string? target))
        {
            if (target is null) return false;

            return EvaluationType switch
            {
                EvaluationType.Equals => caseSensitive ? target == AttributeValue : target.Equals(AttributeValue, StringComparison.InvariantCultureIgnoreCase),
                EvaluationType.NotEquals => caseSensitive ? target != AttributeValue : !target.Equals(AttributeValue, StringComparison.InvariantCultureIgnoreCase),
                EvaluationType.Contains => caseSensitive ? target.Contains(AttributeValue) : target.ToLower().Contains(AttributeValue.ToLower()),
                EvaluationType.NotContains => caseSensitive ? !target.Contains(AttributeValue) : !target.ToLower().Contains(AttributeValue.ToLower()),
                EvaluationType.StartsWith => caseSensitive ? target.StartsWith(AttributeValue) : target.ToLower().StartsWith(AttributeValue.ToLower()),
                EvaluationType.NotStartsWith => caseSensitive ? !target.StartsWith(AttributeValue) : !target.ToLower().StartsWith(AttributeValue.ToLower()),
                EvaluationType.EndsWith => caseSensitive ? target.EndsWith(AttributeValue) : target.ToLower().EndsWith(AttributeValue.ToLower()),
                EvaluationType.NotEndsWith => caseSensitive ? !target.EndsWith(AttributeValue) : !target.ToLower().EndsWith(AttributeValue.ToLower()),
                EvaluationType.GreaterThan => target.CompareTo(AttributeValue) > 0,
                EvaluationType.GreaterThanOrEqual => target.CompareTo(AttributeValue) >= 0,
                EvaluationType.LessThan => target.CompareTo(AttributeValue) < 0,
                EvaluationType.LessThanOrEqual => target.CompareTo(AttributeValue) <= 0,
                EvaluationType.Regex => Regex.IsMatch(target, AttributeValue),
                _ => throw new Exception("Unknown EvaluationType")
            };
        }

        return false;
    }
}
