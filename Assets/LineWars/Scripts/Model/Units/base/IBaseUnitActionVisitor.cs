namespace LineWars.Model
{
    public interface IBaseUnitActionVisitor<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public void Visit(BuildAction<TNode, TEdge, TUnit> action);
        public void Visit(BlockAction<TNode, TEdge, TUnit> action);
        public void Visit(MoveAction<TNode, TEdge, TUnit> action);
        public void Visit(HealAction<TNode, TEdge, TUnit> action);
        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(RLBlockAction<TNode, TEdge, TUnit> action);
        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit> action);
        public void Visit(RamAction<TNode, TEdge, TUnit> action);
        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit> action);
        public void Visit(ShotUnitAction<TNode, TEdge, TUnit> action);
        public void Visit(RLBuildAction<TNode, TEdge, TUnit> action);
        public void Visit(HealYourselfAction<TNode, TEdge, TUnit> action);
        public void Visit(StunAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(HealingAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(TargetPowerBasedAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(UpArmorAction<TNode, TEdge, TUnit> action);
        public void Visit(PowerBasedHealAction<TNode, TEdge, TUnit> action);
        public void Visit(ArmorBasedAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(ConsumeUnitAction<TNode, TEdge, TUnit> action);
        public void Visit(FogEraseAction<TNode, TEdge, TUnit> action);
        public void Visit(ArsonAction<TNode, TEdge, TUnit> action);
        public void Visit(SpawningUnitAction<TNode, TEdge, TUnit> action);
        public void Visit(UpActionPointsAction<TNode, TEdge, TUnit> action);
        public void Visit(JumpAction<TNode, TEdge, TUnit> action);
        public void Visit(HealSacrificeAction<TNode,TEdge, TUnit> action);
        public void Visit(VenomousSpitAction<TNode, TEdge, TUnit> action);
        public void Visit(LichKingMeleeAttack<TNode, TEdge, TUnit> action);
    }
}