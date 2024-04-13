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
		IdentifierToken,
		BangToken,
		AmpersandToken,
		PipeToken,
		AmpersandAmpersandToken,
		PipePipeToken,
		EqualsEqualsToken,
		BangEqualsToken,
		EqualsToken,

		// Keywords
		FalseKeyword,
		TrueKeyword,

		// Expressions
		LiteralExpression,
		BinaryExpression,
		ParenthesizedExpression,
		UnaryExpression,
		NameExpression,
		AssignmentExpression,
	}
}