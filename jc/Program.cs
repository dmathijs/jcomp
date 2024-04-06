﻿using System;
using JComp.CodeAnalysis;

namespace JComp
{
	class Program
	{
		static void Main(string[] args)
		{
			bool showTree = false;
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

				var color = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.DarkGray;
				if (showTree)
				{
					PrettyPrint(syntaxTree.Root);
				}
				Console.ForegroundColor = color;

				if (!syntaxTree.Diagnostics.Any())
				{
					var evaluator = new Evaluator(syntaxTree.Root);
					var result = evaluator.Evaluate();
					Console.WriteLine(result);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					foreach (var diagnostic in syntaxTree.Diagnostics)
					{
						Console.WriteLine(diagnostic);
					}
					Console.ForegroundColor = color;
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

