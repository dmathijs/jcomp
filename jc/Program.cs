using System;
using JComp.CodeAnalysis;
using JComp.CodeAnalysis.Binding;
using JComp.CodeAnalysis.Syntax;

namespace JComp
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var showTree = false;
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
				var binder = new Binder();
				var boundExpression = binder.BindExpression(syntaxTree.Root);

				IReadOnlyList<string> diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

				Console.ForegroundColor = ConsoleColor.DarkGray;
				if (showTree)
				{
					PrettyPrint(syntaxTree.Root);
				}
				Console.ResetColor();

				if (!diagnostics.Any())
				{
					var evaluator = new Evaluator(boundExpression);
					var result = evaluator.Evaluate();
					Console.WriteLine(result);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					foreach (var diagnostic in diagnostics)
					{
						Console.WriteLine(diagnostic);
					}
					Console.ResetColor();
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


