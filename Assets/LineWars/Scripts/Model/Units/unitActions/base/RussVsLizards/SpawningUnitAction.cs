namespace LineWars.Model
{
    public class SpawningUnitAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        ISpawningUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public IUnitFabric<TNode, TEdge, TUnit> UnitFabric { get; private set; }
        public override CommandType CommandType { get; set; }

        public SpawningUnitAction(TUnit executor, IUnitFabric<TNode, TEdge, TUnit> unitFabric) 
            : base(executor)
        {
            this.UnitFabric = unitFabric;
        }

        public bool IsAvailable(TNode target)
        {
            var line = Executor.Node.GetLine(target);
            return ActionPointsCondition()
                   && line != null
                   && UnitFabric.CanSpawn(target)
                   && NoEnemiesCondition(target);
        }

        private bool NoEnemiesCondition(TNode target)
        {
            if (!target.LeftIsFree && 
                target.LeftUnit.OwnerId != Executor.OwnerId)
                return false;
            if (!target.RightIsFree &&
                target.RightUnit.OwnerId != Executor.OwnerId)
                return false;
            return true;
        }

        public void Execute(TNode target)
        {
            UnitFabric.Spawn(target);
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TNode target)
        {
            return new TargetedUniversalCommand
                <TUnit,
                SpawningUnitAction<TNode, TEdge, TUnit>,
                TNode>(this, target);
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
