namespace LineWars.Model
{
    public class UpActionPointsAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IUpActionPointsAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.UpActionPoints;

        public UpActionPointsAction(TUnit executor) : base(executor)
        {
        }

        public bool IsAvailable(TUnit target)
        {
            return target != null &&
                   target.OwnerId == Executor.OwnerId &&
                   ActionPointsCondition() &&
                   Executor.CurrentPower != 0 &&
                   Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            target.CurrentActionPoints += Executor.CurrentPower;
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand<TUnit, UpActionPointsAction<TNode, TEdge, TUnit>, TUnit>(
                Executor,
                target
            );
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}