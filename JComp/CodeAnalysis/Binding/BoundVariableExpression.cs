namespace JComp.CodeAnalysis.Binding
{
	internal sealed class BoundVariableExpression : BoundExpression
	{
		public BoundVariableExpression(VariableSymbol variable)
		{
			Variable = variable;
		}

		public override Type Type => Variable.Type;
		public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;

		public VariableSymbol Variable { get; }
	}
}