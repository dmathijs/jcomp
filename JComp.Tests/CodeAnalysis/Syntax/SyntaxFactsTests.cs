using JComp.CodeAnalysis.Syntax;

namespace JComp.Tests.CodeAnalysis.Syntax
{
	public class SyntaxFactsTests
	{
		[Fact]
		public void SyntaxFact_GetText_RoundTrips()
		{
			foreach (SyntaxKind kind in Enum.GetValues(typeof(SyntaxKind)))
			{
				var text = SyntaxFacts.GetText(kind);
				if (kind.ToString() == "BadToken")
					continue;
				if (text == null)
					continue;
				var tokens = SyntaxTree.ParseTokens(text);
				var token = Assert.Single(tokens);

				Assert.Equal(kind, token.Kind);
				Assert.Equal(text, token.Text);
			}
		}

		[Fact]
		public void SyntaxFact_GetBinaryOperator_RoundTrips()
		{
			foreach (SyntaxKind kind in Enum.GetValues(typeof(SyntaxKind)))
			{
				if (kind.ToString().EndsWith("Keyword") ||
					kind.ToString().EndsWith("Token") ||
					kind.ToString().EndsWith("Literal"))
					continue;

				var text = SyntaxFacts.GetText(kind);
				if (text == null)
					continue;

				var parsedKind = SyntaxFacts.GetKeywordKind(text);
				Assert.Equal(kind, parsedKind);
			}
		}
	}
}