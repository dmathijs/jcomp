using System.Collections;
using JComp.CodeAnalysis.Syntax;

namespace JComp.CodeAnalysis
{
	public sealed class DiagnosticBag : IEnumerable<Diagnostic>
	{
		private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

		private void Report(TextSpan span, string message)
		{
			var diagnostic = new Diagnostic(span, message);
			_diagnostics.Add(diagnostic);
		}

		public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		internal void AddRange(DiagnosticBag diagonistics)
		{
			_diagnostics.AddRange(diagonistics._diagnostics);
		}

		internal void ReportInvalidNumber(TextSpan span, string text, Type type)
		{
			var message = $"The number {text} isn't a valid {type}.";
			Report(span, message);
		}

		internal void ReportBadCharacter(int position, char current)
		{
			var message = $"bad character input: '{current}'.";
			Report(new TextSpan(position, 1), message);
		}

		internal void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
		{
			var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
			Report(span, message);
		}

		internal void ReportUndefinedUnaryOperator(TextSpan span, string? operatorText, Type operandType)
		{
			var message = $"Unary operator '{operatorText}' is not defined for type '{operandType}'.";
			Report(span, message);
		}

		internal void ReportUndefinedBinaryOperator(TextSpan span, string? text, Type leftType, Type rightType)
		{
			var message = $"Binary operator '{text}' is not defined for types '{leftType}' and '{rightType}'.";
			Report(span, message);
		}
	}
}
