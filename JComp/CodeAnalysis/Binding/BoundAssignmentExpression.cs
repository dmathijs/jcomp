namespace JComp.CodeAnalysis.Binding
{
	internal sealed class BoundAssignmentExpression : BoundExpression
	{
		public BoundAssignmentExpression(VariableSymbol variable, BoundExpression boundExpression)
		{
			Variable = variable;
			Expression = boundExpression;
		}

		public VariableSymbol Variable { get; }
		public BoundExpression Expression { get; }
		public override Type Type => Expression.Type;
		public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
	}
}