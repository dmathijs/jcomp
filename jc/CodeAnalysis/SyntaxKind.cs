namespace JComp.CodeAnalysis
{
	public enum SyntaxKind
	{
		NumberToken,
		WhitespaceToken,
		CloseParenthesisToken,
		OpenParenthesisToken,
		SlashToken,
		StarToken,
		MinusToken,
		PlusToken,
		BadToken,
		EndOfFileToken,
		NumberExpression,
		BinaryExpression,
		ParenthesizedExpression
	}
}