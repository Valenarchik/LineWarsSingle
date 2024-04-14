namespace LineWars.Model
{
    public interface IUnitActionVisitor<out TResult, TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public TResult Visit(IBuildAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IBlockAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IMoveAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IHealAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IDistanceAttackAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IMeleeAttackAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IRLBlockAction<TNode, TEdge, TUnit> action);
        public TResult Visit(ISacrificeForPerunAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IRamAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IBlowWithSwingAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IShotUnitAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IRLBuildAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IHealYourselfAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IStunAttackAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IHealingAttackAction<TNode, TEdge, TUnit> action);
        public TResult Visit(ITargetPowerBasedAttackAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IUpArmorAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IPowerBasedHealAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IArmorBasedAttackAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IConsumeUnitAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IFogEraseAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IArsonAction<TNode, TEdge, TUnit> action);
        public TResult Visit(ISpawningUnitAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IUpActionPointsAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IJumpAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IHealSacrificeAction<TNode, TEdge, TUnit> action);
        public TResult Visit(IVenomousSpitAction<TNode, TEdge, TUnit> action);
        public TResult Visit(ILichKingMeleeAttack<TNode, TEdge, TUnit> action);
    }
}