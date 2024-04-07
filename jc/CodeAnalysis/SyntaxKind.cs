namespace JComp.CodeAnalysis
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