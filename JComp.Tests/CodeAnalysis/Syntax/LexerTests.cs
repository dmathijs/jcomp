using JComp.CodeAnalysis.Syntax;

namespace JComp.Tests.CodeAnalysis.Syntax
{
	public class LexerTests
	{
		[Theory]
		[MemberData(nameof(GetTokensData))]
		public void Lexer_Lexes_Token(SyntaxKind kind, string text)
		{
			var tokens = SyntaxTree.ParseTokens(text).ToArray();
			Assert.Single(tokens);
			Assert.Equal(kind, tokens[0].Kind);
			Assert.Equal(text, tokens[0].Text);
		}

		[Theory]
		[MemberData(nameof(GetTokenPairsData))]
		public void Lexer_Lexes_TokenPairs(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)
		{
			var text = t1Text + t2Text;
			var tokens = SyntaxTree.ParseTokens(text).ToArray();
			Assert.Equal(2, tokens.Length);
			Assert.Equal(t1Kind, tokens[0].Kind);
			Assert.Equal(t1Text, tokens[0].Text);
			Assert.Equal(t2Kind, tokens[1].Kind);
			Assert.Equal(t2Text, tokens[1].Text);
		}

		[Theory]
		[MemberData(nameof(GetTokenPairsWithSeparatorData))]
		public void Lexer_Lexes_TokenPairsWithSeparator(SyntaxKind t1Kind, string t1Text, SyntaxKind separatorKind, string separatorText, SyntaxKind t2Kind, string t2Text)
		{
			var text = t1Text + separatorText + t2Text;
			var tokens = SyntaxTree.ParseTokens(text).ToArray();
			Assert.Equal(3, tokens.Length);
			Assert.Equal(t1Kind, tokens[0].Kind);
			Assert.Equal(t1Text, tokens[0].Text);
			Assert.Equal(separatorKind, tokens[1].Kind);
			Assert.Equal(separatorText, tokens[1].Text);
			Assert.Equal(t2Kind, tokens[2].Kind);
			Assert.Equal(t2Text, tokens[2].Text);
		}

		public static IEnumerable<object[]> GetTokensData()
		{
			return GetTokens()
				.Select(t => new object[] { t.kind, t.text });
		}

		public static IEnumerable<object[]> GetTokenPairsData()
		{
			return GetTokenPairs()
				.Select(t => new object[] { t.t1Kind, t.t1Text, t.t2Kind, t.t2Text });
		}

		public static IEnumerable<object[]> GetTokenPairsWithSeparatorData()
		{
			return GetTokenPairsWithSeparator()
				.Select(t => new object[] { t.t1Kind, t.t1Text, t.separatorKind, t.separatorText, t.t2Kind, t.t2Text });
		}

		private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
		{
			return new[]
			{
				(SyntaxKind.PlusToken, "+"),
				(SyntaxKind.MinusToken, "-"),
				(SyntaxKind.StarToken, "*"),
				(SyntaxKind.SlashToken, "/"),
				(SyntaxKind.BangToken, "!"),
				(SyntaxKind.EqualsToken, "="),
				(SyntaxKind.AmpersandAmpersandToken, "&&"),
				(SyntaxKind.PipePipeToken, "||"),
				(SyntaxKind.EqualsEqualsToken, "=="),
				(SyntaxKind.BangEqualsToken, "!="),
				(SyntaxKind.OpenParenthesisToken, "("),
				(SyntaxKind.CloseParenthesisToken, ")"),
				(SyntaxKind.FalseKeyword, "false"),
				(SyntaxKind.TrueKeyword, "true"),
				(SyntaxKind.IdentifierToken, "a"),
				(SyntaxKind.NumberToken, "1")
			};
		}

		private static IEnumerable<(SyntaxKind kind, string text)> GetSeparators()
		{
			return new[]
			{
				(SyntaxKind.WhitespaceToken, " "),
				(SyntaxKind.WhitespaceToken, "  "),
				(SyntaxKind.WhitespaceToken, "\r"),
				(SyntaxKind.WhitespaceToken, "\n"),
				(SyntaxKind.WhitespaceToken, "\r\n"),
				(SyntaxKind.WhitespaceToken, "\t")
			};
		}

		private static bool RequiresSeparator(SyntaxKind t1Kind, SyntaxKind t2Kind)
		{
			var t1IsKeyword = t1Kind.ToString().EndsWith("Keyword");
			var t2IsKeyword = t2Kind.ToString().EndsWith("Keyword");

			if (t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.IdentifierToken)
				return true;

			if (t1IsKeyword && t2IsKeyword)
				return true;

			if (t1IsKeyword && t2Kind == SyntaxKind.IdentifierToken)
				return true;

			if (t1Kind == SyntaxKind.IdentifierToken && t2IsKeyword)
				return true;

			if (t1Kind == SyntaxKind.CloseParenthesisToken && t2IsKeyword)
				return true;

			if (t1IsKeyword && t2Kind == SyntaxKind.OpenParenthesisToken)
				return true;

			if (t1Kind == SyntaxKind.TrueKeyword && t2Kind == SyntaxKind.OpenParenthesisToken)
				return true;

			if (t1Kind == SyntaxKind.FalseKeyword && t2Kind == SyntaxKind.OpenParenthesisToken)
				return true;

			if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.OpenParenthesisToken)
				return true;

			if (t1Kind == SyntaxKind.CloseParenthesisToken && t2Kind == SyntaxKind.OpenParenthesisToken)
				return true;

			if (t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.OpenParenthesisToken)
				return true;

			if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.IdentifierToken)
				return true;

			if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.IdentifierToken)
				return true;

			if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.IdentifierToken)
				return true;

			if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.IdentifierToken)
				return true;

			if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualsToken)
				return true;

			if (t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualsEqualsToken)
				return true;

			if (t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualsEqualsToken)
				return true;

			if (t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualsToken)
				return true;

			if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.NumberToken)
				return true;

			return false;
		}

		private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)> GetTokenPairs()
		{
			foreach (var t1 in GetTokens())
			{
				foreach (var t2 in GetTokens())
				{
					if (!RequiresSeparator(t1.kind, t2.kind))
						yield return (t1.kind, t1.text, t2.kind, t2.text);
				}
			}
		}

		private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind separatorKind, string separatorText, SyntaxKind t2Kind, string t2Text)> GetTokenPairsWithSeparator()
		{
			foreach (var t1 in GetTokens())
			{
				foreach (var t2 in GetTokens())
				{
					if (RequiresSeparator(t1.kind, t2.kind))
					{
						foreach (var separator in GetSeparators())
							yield return (t1.kind, t1.text, separator.kind, separator.text, t2.kind, t2.text);
					}
				}
			}
		}
	}
}