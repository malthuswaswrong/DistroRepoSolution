using DistroRepo;
using System.Collections.Generic;

namespace DistroRepoTest;

public class EvaluationTests
{
    Evaluation cut;
    public EvaluationTests()
    {
        cut = new Evaluation();
    }
    
    [Theory]
    [InlineData(EvaluationType.Equals, "1", "1", true, true)]
    [InlineData(EvaluationType.Equals, "1", "1", false, true)]
    [InlineData(EvaluationType.Equals, "one", "one", true, true)]
    [InlineData(EvaluationType.Equals, "one", "One", false, true)]
    [InlineData(EvaluationType.Equals, "one", "One", true, false)]
    [InlineData(EvaluationType.Equals, "1001-01-01", "01/01/1001", false, true)]
    [InlineData(EvaluationType.Equals, "1", "01", false, true)]
    [InlineData(EvaluationType.Equals, "1", "1.0", false, true)]
    [InlineData(EvaluationType.Equals, "1", "1.1", false, false)]
    
    [InlineData(EvaluationType.NotEquals, "1", "1.1", false, true)]
    [InlineData(EvaluationType.NotEquals, "one", "One", true, true)]
    
    [InlineData(EvaluationType.Contains, "one", "OneTwoThree", false, true)]
    [InlineData(EvaluationType.Contains, "one", "OneTwoThree", true, false)]
    [InlineData(EvaluationType.Contains, "1", "111", true, true)]
    [InlineData(EvaluationType.Contains, "01", "111", true, false)]

    [InlineData(EvaluationType.NotContains, "one", "OneTwoThree", false, false)]
    [InlineData(EvaluationType.NotContains, "one", "OneTwoThree", true, true)]
    [InlineData(EvaluationType.NotContains, "1", "111", true, false)]
    [InlineData(EvaluationType.NotContains, "01", "111", true, true)]

    [InlineData(EvaluationType.StartsWith, "one", "OneTwoThree", false, true)]
    [InlineData(EvaluationType.StartsWith, "one", "OneTwoThree", true, false)]
    [InlineData(EvaluationType.StartsWith, "1", "111", true, true)]
    [InlineData(EvaluationType.StartsWith, "01", "111", true, false)]
    
    [InlineData(EvaluationType.EndsWith, "three", "OneTwoThree", false, true)]
    [InlineData(EvaluationType.EndsWith, "three", "OneTwoThree", true, false)]
    [InlineData(EvaluationType.EndsWith, "1", "111", true, true)]
    [InlineData(EvaluationType.EndsWith, "01", "111", true, false)]

    [InlineData(EvaluationType.GreaterThan, "1", "1", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "0", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "2", false, true)]
    [InlineData(EvaluationType.GreaterThan, "2", "10", false, true)]
    [InlineData(EvaluationType.GreaterThan, "1", "0.1", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "0.0", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "1.001", false, true)]
    [InlineData(EvaluationType.GreaterThan, "1901-01-01", "1901-01-02", false, true)]
    [InlineData(EvaluationType.GreaterThan, "1901-01-01", "1901-01-01", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1901-01-01", "01/01/1999", false, true)]
    
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "1", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "0", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "2", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "0.1", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "0.0", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "1.001", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "1.00", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1901-01-01", "1901-01-02", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1901-01-01", "1901-01-01", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1901-01-01", "01/01/1999", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1901-01-01", "01/01/1901", false, true)]

    [InlineData(EvaluationType.LessThan, "1", "1", false, false)]
    [InlineData(EvaluationType.LessThan, "1", "0", false, true)]
    [InlineData(EvaluationType.LessThan, "1", "2", false, false)]
    [InlineData(EvaluationType.LessThan, "1", "0.1", false, true)]
    [InlineData(EvaluationType.LessThan, "1901-01-01", "1901-01-02", false, false)]
    [InlineData(EvaluationType.LessThan, "1901-01-01", "1901-01-01", false, false)]
    [InlineData(EvaluationType.LessThan, "01/01/1999", "1901-01-01", false, true)]

    [InlineData(EvaluationType.LessThanOrEqual, "1", "1", false, true)]
    [InlineData(EvaluationType.LessThanOrEqual, "1", "0", false, true)]
    [InlineData(EvaluationType.LessThanOrEqual, "1", "2", false, false)]
    [InlineData(EvaluationType.LessThanOrEqual, "1", "0.1", false, true)]
    [InlineData(EvaluationType.LessThanOrEqual, "1901-01-01", "1901-01-02", false, false)]
    [InlineData(EvaluationType.LessThanOrEqual, "1901-01-01", "1901-01-01", false, true)]
    [InlineData(EvaluationType.LessThanOrEqual, "01/01/1999", "1901-01-01", false, true)]

    [InlineData(EvaluationType.Regex, @"[\d]", "1", true, true)]
    [InlineData(EvaluationType.Regex, @"[\d]", "one", true, false)]
    [InlineData(EvaluationType.Regex, @"[0$]", "10", true, true)]
    [InlineData(EvaluationType.Regex, @"[^1]", "10", true, true)]
    [InlineData(EvaluationType.Regex, @"(\d)(0)(\d)", "101", true, true)]
    [InlineData(EvaluationType.Regex, @"(\d)(1)(\d)", "101", true, false)]

    public void AllTestsLoseEvaluation(EvaluationType type, string source, string target, bool caseSensitive, bool expected)
    {
        Dictionary<string, string> attributes = new Dictionary<string, string>(){
            {"test", target}
        };
        cut.AttributeName = "test";
        cut.AttributeValue = source;
        cut.EvaluationType = type;
        Assert.Equal(expected, cut.Evaluate(attributes, caseSensitive));
    }

    [Theory]
    [InlineData(EvaluationType.Equals, "1001-01-01", "01/01/1001", false, false)]
    [InlineData(EvaluationType.Equals, "1", "01", false, false)]
    [InlineData(EvaluationType.Equals, "1", "1.0", false, false)]
    [InlineData(EvaluationType.Equals, "1", "1.1", false, false)]

    [InlineData(EvaluationType.GreaterThan, "1", "1", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "0", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "2", false, true)]
    [InlineData(EvaluationType.GreaterThan, "2", "10", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "0.1", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "0.0", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1", "1.001", false, true)]
    [InlineData(EvaluationType.GreaterThan, "1901-01-01", "1901-01-02", false, true)]
    [InlineData(EvaluationType.GreaterThan, "1901-01-01", "1901-01-01", false, false)]
    [InlineData(EvaluationType.GreaterThan, "1901-01-01", "01/01/1999", false, false)]

    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "1", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "0", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "2", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "2", "10", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "0.1", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "0.0", false, false)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "1.001", false, true)]
    [InlineData(EvaluationType.GreaterThanOrEqual, "1", "1.00", false, true)]

    [InlineData(EvaluationType.LessThan, "1", "1", false, false)]
    [InlineData(EvaluationType.LessThan, "1", "0", false, true)]
    [InlineData(EvaluationType.LessThan, "1", "2", false, false)]
    [InlineData(EvaluationType.LessThan, "1", "20", false, false)]
    [InlineData(EvaluationType.LessThan, "20", "1", false, true)]

    [InlineData(EvaluationType.LessThanOrEqual, "1", "1", false, true)]
    [InlineData(EvaluationType.LessThanOrEqual, "1", "0", false, true)]
    [InlineData(EvaluationType.LessThanOrEqual, "1", "2", false, false)]
    [InlineData(EvaluationType.LessThanOrEqual, "1", "20", false, false)]
    [InlineData(EvaluationType.LessThanOrEqual, "20", "1", false, true)]

    public void AllTestsStrictEvaluation(EvaluationType type, string source, string target, bool caseSensitive, bool expected)
    {
        Dictionary<string, string> attributes = new Dictionary<string, string>(){
            {"test", target}
        };
        cut.AttributeName = "test";
        cut.AttributeValue = source;
        cut.EvaluationType = type;
        Assert.Equal(expected, cut.EvaluateStrict(attributes, caseSensitive));
    }
}