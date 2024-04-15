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
	}
}