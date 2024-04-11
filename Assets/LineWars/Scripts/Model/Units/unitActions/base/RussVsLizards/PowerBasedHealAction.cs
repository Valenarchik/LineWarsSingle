namespace LineWars.Model
{
    public class PowerBasedHealAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IPowerBasedHealAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.PowerBasedHeal;
        
        public PowerBasedHealAction(TUnit executor) : base(executor)
        {
        }

        public bool IsAvailable(TUnit target)
        {
            return target != null &&
                ActionPointsCondition() &&
                target.CurrentPower != 0 &&
                target.OwnerId == Executor.OwnerId &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            target.CurrentHp += Executor.CurrentPower;
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit, PowerBasedHealAction<TNode, TEdge, TUnit>, TUnit>
                (this, target);
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
