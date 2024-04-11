namespace LineWars.Model
{
    public class RLBlockAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IRLBlockAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType { get; set; } = CommandType.Block;

        public RLBlockAction(TUnit executor) : base(executor)
        {
        }

        public bool CanBlock()
        {
            return ActionPointsCondition()
                   && Executor.CurrentArmor < Executor.CurrentPower; // проверка на дурака
        }

        public void EnableBlock()
        {
            Executor.CurrentArmor = Executor.CurrentPower;
            CompleteAndAutoModify();
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