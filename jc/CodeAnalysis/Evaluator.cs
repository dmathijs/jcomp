using JComp.CodeAnalysis.Binding;
using JComp.CodeAnalysis.Syntax;

namespace JComp.CodeAnalysis
{
	internal class Evaluator
	{
		private readonly BoundExpression _root;

		public Evaluator(BoundExpression root)
		{
			_root = root;
		}

		public object Evaluate()
		{
			return EvaluateExpression(_root);
		}

		private object EvaluateExpression(BoundExpression node)
		{
			if (node is BoundLiteralExpression n)
			{
				return n.Value;
			}

			if (node is BoundUnaryExpression u)
			{
				var operand = EvaluateExpression(u.Operand);

				switch (u.OperatorKind)
				{
					case BoundUnaryOperatorKind.Identity:
						return (int)operand;
					case BoundUnaryOperatorKind.Negation:
						return -(int)operand;
					case BoundUnaryOperatorKind.LogicalNegation:
						return !(bool)operand;
					default:
						throw new Exception($"Unexpected unary operator {u.OperatorKind}");
				}
			}

			if (node is BoundBinaryExpression b)
			{
				var left = EvaluateExpression(b.Left);
				var right = EvaluateExpression(b.Right);

				if (b.OperatorKind == BoundBinaryOperatorKind.Addition)
					return (int)left + (int)right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.Subtraction)
					return (int)left - (int)right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.Multiplication)
					return (int)left * (int)right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.Division)
					return (int)left / (int)right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.LogialOr)
					return (bool)left || (bool)right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.LogicalAnd)
					return (bool)left && (bool)right;
				else
					throw new Exception($"Unexpected binary operator {b.OperatorKind}");
			}

			throw new Exception($"Unexpected node {node}");
		}
	}
}