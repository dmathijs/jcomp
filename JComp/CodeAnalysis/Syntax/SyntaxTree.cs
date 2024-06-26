namespace JComp.CodeAnalysis.Syntax
{
	public sealed class SyntaxTree
	{
		public SyntaxTree(DiagnosticBag diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
		{
			Diagnostics = diagnostics;
			Root = root;
			EndOfFileToken = endOfFileToken;
		}

		public DiagnosticBag Diagnostics { get; }
		public ExpressionSyntax Root { get; }
		public SyntaxToken EndOfFileToken { get; }

		public static SyntaxTree Parse(string text)
		{
			var parser = new Parser(text);
			return parser.Parse();
		}

		public static IEnumerable<SyntaxToken> ParseTokens(string text)
		{
			var lexer = new Lexer(text);
			while (true)
			{
				var token = lexer.Lex();
				if (token.Kind == SyntaxKind.EndOfFileToken)
					break;

				yield return token;
			}
		}
	}
}