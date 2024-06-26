using JComp.CodeAnalysis.Syntax;

namespace JComp.Tests.CodeAnalysis.Syntax
{
	internal sealed class AssertingEnumerator : IDisposable
	{
		private readonly IEnumerator<SyntaxNode> _enumerator;
		private bool _hasErrors;

		public AssertingEnumerator(SyntaxNode node)
		{
			_enumerator = Flatten(node).GetEnumerator();
		}

		private bool MarkFailed()
		{
			_hasErrors = true;
			return false;
		}

		private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
		{
			var stack = new Stack<SyntaxNode>();
			stack.Push(node);
			while (stack.Count > 0)
			{
				var n = stack.Pop();
				yield return n;

				foreach (var child in n.GetChildren().Reverse())
					stack.Push(child);
			}
		}

		public void AssertToken(SyntaxKind kind, string text)
		{
			try
			{
				Assert.True(_enumerator.MoveNext());
				var token = Assert.IsType<SyntaxToken>(_enumerator.Current);

				Assert.Equal(kind, token.Kind);
				Assert.Equal(text, token.Text);

			}
			catch when (MarkFailed())
			{
				throw;
			}
		}

		public void AssertNode(SyntaxKind kind)
		{
			try
			{
				Assert.True(_enumerator.MoveNext());
				Assert.Equal(kind, _enumerator.Current.Kind);
				Assert.IsNotType<SyntaxToken>(_enumerator.Current);
			}
			catch when (MarkFailed())
			{
				throw;
			}
		}

		public void Dispose()
		{
			if (!_hasErrors)
				Assert.False(_enumerator.MoveNext(), $"Expected enumerator to be empty, got {_enumerator.Current} instead.");
			_enumerator.Dispose();
		}
	}
}