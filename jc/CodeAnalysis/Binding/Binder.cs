using JComp.CodeAnalysis.Syntax;

namespace JComp.CodeAnalysis.Binding
{

	internal sealed class Binder
	{
		private readonly List<string> _diagnostics = new List<string>();

		public IEnumerable<string> Diagnostics => _diagnostics;

		public BoundExpression BindExpression(ExpressionSyntax syntax)
		{
			switch (syntax.Kind)
			{
				case SyntaxKind.LiteralExpression:
					return BindLiteralExpression((LiteralExpressionSyntax)syntax);
				case SyntaxKind.UnaryExpression:
					return BindUnaryExpression((UnaryExpressionSyntax)syntax);
				case SyntaxKind.BinaryExpression:
					return BindBinaryExpression((BinaryExpressionSyntax)syntax);
				default:
					throw new Exception($"Unexpected syntax {syntax.Kind}");
			}
		}

		private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
		{
			var boundLeft = BindExpression(syntax.Left);
			var boundRight = BindExpression(syntax.Right);
			var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

			if (boundOperatorKind == null)
			{
				_diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types '{boundLeft.Type}' and '{boundRight.Type}'");
				return boundLeft;
			}

			return new BoundBinaryExpression(boundLeft, boundOperatorKind.Value, boundRight);
		}

		private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
		{
			if (leftType == typeof(int) && rightType == typeof(int))
			{
				switch (kind)
				{
					case SyntaxKind.PlusToken:
						return BoundBinaryOperatorKind.Addition;
					case SyntaxKind.MinusToken:
						return BoundBinaryOperatorKind.Subtraction;
					case SyntaxKind.StarToken:
						return BoundBinaryOperatorKind.Multiplication;
					case SyntaxKind.SlashToken:
						return BoundBinaryOperatorKind.Division;
				}
			}
			else if (leftType == typeof(bool) && rightType == typeof(bool))
			{
				switch (kind)
				{
					case SyntaxKind.AmpersandToken:
						return BoundBinaryOperatorKind.LogicalAnd;
					case SyntaxKind.PipeToken:
						return BoundBinaryOperatorKind.LogialOr;
				}
			}
			return null;

		}

		private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
		{
			var boundOperand = BindExpression(syntax.Operand);
			var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

			if (boundOperatorKind == null)
			{
				_diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type '{boundOperand.Type}'");
				return boundOperand;
			}

			return new BoundUnaryExpression(boundOperatorKind.Value, boundOperand);
		}

		private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
		{
			if (operandType == typeof(int))
			{
				switch (kind)
				{
					case SyntaxKind.PlusToken:
						return BoundUnaryOperatorKind.Identity;
					case SyntaxKind.MinusToken:
						return BoundUnaryOperatorKind.Negation;
				}
			}
			else if (operandType == typeof(bool))
			{
				switch (kind)
				{
					case SyntaxKind.BangToken:
						return BoundUnaryOperatorKind.LogicalNegation;
				}
			}

			return null;
		}

		private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
		{
			var value = syntax.Value ?? 0;
			return new BoundLiteralExpression(value);
		}
	}
}