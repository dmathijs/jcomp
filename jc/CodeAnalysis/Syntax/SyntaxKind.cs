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
		NumberExpression,
		BinaryExpression,
		ParenthesizedExpression,
		UnaryExpression
	}
}