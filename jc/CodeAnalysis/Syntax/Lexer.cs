namespace JComp.CodeAnalysis.Syntax
{
	internal class Lexer
	{
		private readonly string _text;
		private int _position;
		private List<string> _diagnostics = new List<string>();

		public Lexer(string text)
		{
			this._text = text;
		}

		public IEnumerable<string> Diagonistics => _diagnostics;

		public SyntaxToken Lex()
		{
			if (_position >= _text.Length)
			{
				return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
			}

			if (char.IsDigit(Current))
			{
				var start = _position;

				while (char.IsDigit(Current))
					Next();

				var length = _position - start;
				var text = _text.Substring(start, length);
				if (!int.TryParse(text, out var value))
				{
					_diagnostics.Add($"The number {text} is not a valid Int32");
				}

				return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
			}

			if (char.IsWhiteSpace(Current))
			{
				var start = _position;

				while (char.IsWhiteSpace(Current))
					Next();

				var length = _position - start;
				var text = _text.Substring(start, length);
				return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
			}

			if (char.IsLetter(Current))
			{
				var start = _position;

				while (char.IsLetter(Current))
					Next();

				var length = _position - start;
				var text = _text.Substring(start, length);
				var kind = SyntaxFacts.GetKeywordKind(text);
				return new SyntaxToken(kind, start, text, null);
			}

			switch (Current)
			{
				case '+':
					return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
				case '-':
					return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
				case '*':
					return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
				case '/':
					return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
				case '(':
					return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
				case ')':
					return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
			}

			_diagnostics.Add($"ERROR: bad character input: '{Current}'");
			return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
		}

		private char Current
		{
			get
			{
				if (_position >= _text.Length)
				{
					return '\0';
				}

				return _text[_position];
			}

		}

		private void Next()
		{
			_position++;
		}
	}
}