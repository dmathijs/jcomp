namespace JComp.CodeAnalysis.Syntax
{
	public enum SyntaxKind
	{
		// Tokens
		BadToken,
		EndOfFileToken,
		WhitespaceToken,
		NumberToken,
		CloseParenthesisToken,
		OpenParenthesisToken,
		SlashToken,
		StarToken,
		MinusToken,
		PlusToken,

		// Expressions
		LiteralExpression,
		BinaryExpression,
		ParenthesizedExpression,
		UnaryExpression,
	}
}