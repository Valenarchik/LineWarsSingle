namespace LineWars.Model
{
    public class ConsumeUnitAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IConsumeUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.ConsumeUnit;
        public ConsumeUnitAction(TUnit executor) : base(executor)
        {
        }

        public void Execute(TUnit target)
        {
            Executor.CurrentPower += target.CurrentPower;
            target.CurrentHp = 0;
            CompleteAndAutoModify();
        }

        public bool IsAvailable(TUnit target)
        {
            return target != null &&
                ActionPointsCondition() &&
                target.CurrentPower != 0 &&
                target.OwnerId == Executor.OwnerId &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit,
                ConsumeUnitAction<TNode, TEdge, TUnit>,
                TUnit>(this, target);
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
