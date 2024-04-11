using JComp.CodeAnalysis.Binding;
using JComp.CodeAnalysis.Syntax;

namespace JComp.CodeAnalysis
{
	public sealed class Compilation
	{
		public Compilation(SyntaxTree syntax)
		{
			Syntax = syntax;
		}

		public SyntaxTree Syntax { get; set; }

		public EvaluationResult Evaluate()
		{
			var binder = new Binder();
			var boundExpression = binder.BindExpression(Syntax.Root);

			var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
			if (diagnostics.Any())
			{
				return new EvaluationResult(diagnostics, null);
			}

			var evaluator = new Evaluator(boundExpression);
			return new EvaluationResult(Array.Empty<Diagnostic>(), evaluator.Evaluate());
		}
	}
}
