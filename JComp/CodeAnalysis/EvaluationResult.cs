using System.Collections.Generic;

namespace JComp.CodeAnalysis
{
	public sealed class EvaluationResult
	{
		public EvaluationResult(IEnumerable<Diagnostic> diagnostics, object? value)
		{
			Diagnostics = diagnostics.ToArray();
			Value = value;
		}

		public IReadOnlyList<Diagnostic> Diagnostics { get; }
		public object? Value { get; }
	}
}