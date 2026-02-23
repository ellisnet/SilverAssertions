using System.Xml.Linq;

namespace SilverAssertions.Equivalency.Steps;

public class XAttributeEquivalencyStep : EquivalencyStep<XAttribute>
{
    protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        var subject = (XAttribute)comparands.Subject;
        var expectation = (XAttribute)comparands.Expectation;

        subject.Should().Be(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return EquivalencyResult.AssertionCompleted;
    }
}
