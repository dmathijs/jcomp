using JComp.CodeAnalysis;
using JComp.CodeAnalysis.Syntax;

namespace JComp.Tests.CodeAnalysis;

public class EvaluatorTests
{
	[Theory]
	[InlineData("1", 1)]
	[InlineData("+1", 1)]
	[InlineData("-1", -1)]
	[InlineData("1 + 2", 3)]
	[InlineData("1 - 2", -1)]
	[InlineData("4 / 2", 2)]
	[InlineData("2 * 3", 6)]
	[InlineData("2 == 3", false)]
	[InlineData("2 != 3", true)]
	[InlineData("(10)", 10)]
	[InlineData("true", true)]
	[InlineData("!true", false)]
	[InlineData("false", false)]
	[InlineData("!false", true)]
	[InlineData("false && true", false)]
	[InlineData("false || true", true)]
	[InlineData("false == true", false)]
	[InlineData("false != true", true)]
	[InlineData("(a = 10) * a", 100)]
	public void SyntaxFact_GetText_RoundTrips(string text, object expectedResult)
	{
		var expression = SyntaxTree.Parse(text);
		var compilation = new Compilation(expression);

		var variables = new Dictionary<VariableSymbol, object?>();
		var result = compilation.Evaluate(variables);

		Assert.Empty(result.Diagnostics);
		Assert.Equal(expectedResult, result.Value);
	}
}