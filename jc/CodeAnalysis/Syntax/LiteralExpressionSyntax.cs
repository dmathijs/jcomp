namespace JComp.CodeAnalysis.Syntax
{
	public sealed class LiteralExpressionSyntax : ExpressionSyntax
	{
		public LiteralExpressionSyntax(SyntaxToken literalToken)
#pragma warning disable CS8604 // Possible null reference argument.
		: this(literalToken, literalToken.Value)
#pragma warning restore CS8604 // Possible null reference argument.
		{
			LiteralToken = literalToken;
		}

		public LiteralExpressionSyntax(SyntaxToken literalToken, Object value)
		{
			LiteralToken = literalToken;
			Value = value;
		}

		public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
		public SyntaxToken LiteralToken { get; }
		public object Value { get; }

		public override IEnumerable<SyntaxNode> GetChildren()
		{
			yield return LiteralToken;
		}
	}
}