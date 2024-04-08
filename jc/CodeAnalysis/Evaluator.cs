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
				var operand = (int)EvaluateExpression(u.Operand);

				switch (u.OperatorKind)
				{
					case BoundUnaryOperatorKind.Identity:
						return operand;
					case BoundUnaryOperatorKind.Negation:
						return -operand;
					default:
						throw new Exception($"Unexpected unary operator {u.OperatorKind}");
				}
			}

			if (node is BoundBinaryExpression b)
			{
				var left = (int)EvaluateExpression(b.Left);
				var right = (int)EvaluateExpression(b.Right);

				if (b.OperatorKind == BoundBinaryOperatorKind.Addition)
					return left + right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.Subtraction)
					return left - right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.Multiplication)
					return left * right;
				else if (b.OperatorKind == BoundBinaryOperatorKind.Division)
					return left / right;
				else
					throw new Exception($"Unexpected binary operator {b.OperatorKind}");
			}

			throw new Exception($"Unexpected node {node}");
		}
	}
}