namespace JComp.CodeAnalysis.Syntax
{

	internal class Parser
	{
		private readonly SyntaxToken[] _tokens;
		private int _position;
		private DiagnosticBag _diagnostics = new DiagnosticBag();

		public Parser(string text)
		{
			var tokens = new List<SyntaxToken>();
			var lexer = new Lexer(text);

			SyntaxToken token;
			do
			{
				token = lexer.Lex();

				if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken)
				{
					tokens.Add(token);
				}
			} while (token.Kind != SyntaxKind.EndOfFileToken);

			_tokens = tokens.ToArray();
			_diagnostics.AddRange(lexer.Diagonistics);
		}

		public DiagnosticBag Diagnostics => _diagnostics;

		public SyntaxTree Parse()
		{
			var expression = ParseExpression();
			var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
			return new SyntaxTree(_diagnostics, expression, endOfFileToken);
		}

		private ExpressionSyntax ParseExpression()
		{
			return ParseAssignmentExpression();
		}

		private ExpressionSyntax ParseAssignmentExpression()
		{
			if (Peek(0).Kind == SyntaxKind.IdentifierToken && Peek(1).Kind == SyntaxKind.EqualsToken)
			{
				var identifierToken = NextToken();
				var operatorToken = NextToken();
				var right = ParseAssignmentExpression();
				return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
			}

			return ParseBinaryExpression();
		}

		private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
		{
			ExpressionSyntax left;

			var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
			if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
			{
				var operatorToken = NextToken();
				var operand = ParseBinaryExpression(unaryOperatorPrecedence);
				left = new UnaryExpressionSyntax(operatorToken, operand);
			}
			else
			{
				left = ParsePrimaryExpression();
			}

			while (true)
			{
				var precedence = Current.Kind.GetBinaryOperatorPrecedence();
				if (precedence == 0 || precedence <= parentPrecedence)
					break;

				var operatorToken = NextToken();
				var right = ParseBinaryExpression(precedence);
				left = new BinaryExpressionSyntax(left, operatorToken, right);
			}

			return left;
		}

		private ExpressionSyntax ParsePrimaryExpression()
		{
			switch (Current.Kind)
			{
				case SyntaxKind.OpenParenthesisToken:
					{
						var left = NextToken();
						var expression = ParseExpression();
						var right = MatchToken(SyntaxKind.CloseParenthesisToken);
						return new ParenthesizedExpressionSyntax(left, expression, right);
					}

				case SyntaxKind.TrueKeyword:
				case SyntaxKind.FalseKeyword:
					{
						var keywordToken = NextToken();
						var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
						return new LiteralExpressionSyntax(keywordToken, value);
					}
				case SyntaxKind.IdentifierToken:
					{
						var identifierToken = NextToken();
						return new NameExpressionSyntax(identifierToken);
					}

				default:
					{
						var numberToken = MatchToken(SyntaxKind.NumberToken);
						return new LiteralExpressionSyntax(numberToken);
					}
			}
		}

		private SyntaxToken MatchToken(SyntaxKind kind)
		{
			if (Current.Kind == kind)
				return NextToken();

			_diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
			return new SyntaxToken(kind, Current.Position, null, null);
		}

		private SyntaxToken NextToken()
		{
			var current = Current;
			_position++;
			return current;
		}

		private SyntaxToken Peek(int offset)
		{
			var index = _position + offset;
			if (index >= _tokens.Length)
				return _tokens[_tokens.Length - 1];

			return _tokens[index];
		}

		private SyntaxToken Current => Peek(0);
	}
}