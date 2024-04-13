using System;
using JComp.CodeAnalysis;
using JComp.CodeAnalysis.Syntax;

namespace JComp
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var showTree = false;
			var variables = new Dictionary<VariableSymbol, object>();

			while (true)
			{
				var line = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(line))
				{
					return;
				}

				if (line == "#showTree")
				{
					showTree = !showTree;
					Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
					continue;
				}
				else if (line == "#cls")
				{
					Console.Clear();
					continue;
				}

				var syntaxTree = SyntaxTree.Parse(line);
				var compilation = new Compilation(syntaxTree);
				var result = compilation.Evaluate(variables);

				IReadOnlyList<Diagnostic> diagnostics = result.Diagnostics;

				Console.ForegroundColor = ConsoleColor.DarkGray;
				if (showTree)
				{
					PrettyPrint(syntaxTree.Root);
				}
				Console.ResetColor();

				if (!diagnostics.Any())
				{
					Console.WriteLine(result.Value);
				}
				else
				{
					foreach (var diagnostic in diagnostics)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine(diagnostic);
						Console.ResetColor();

						var prefix = line[..diagnostic.Span.Start];
						var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
						var suffix = line.Substring(diagnostic.Span.End);

						Console.Write("    ");
						Console.Write(prefix);

						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write(error);
						Console.ResetColor();

						Console.Write(suffix);
						Console.WriteLine();
					}
				}
			}
		}

		static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
		{
			var marker = isLast ? "└──" : "├──";

			Console.Write($"{indent}{marker}{node.Kind}");

			if (node is SyntaxToken t && t.Value != null)
			{
				Console.Write(" ");
				Console.Write(t.Value);
			}

			Console.WriteLine();

			indent += isLast ? "   " : "│  ";

			var lastChild = node.GetChildren().LastOrDefault();

			foreach (var child in node.GetChildren())
			{
				PrettyPrint(child, indent, child == lastChild);
			}
		}
	}
}


