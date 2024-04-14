namespace LineWars.Model
{
    public class CopyActionVisitor<TNode, TEdge, TUnit> : 
        IBaseUnitActionVisitor<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit> 
    {
        public TUnit Unit { get; private set; }
        public IGraphForGame<TNode, TEdge, TUnit> Graph { get; private set; }
        public UnitAction<TNode, TEdge, TUnit> Result { get; private set; }

        public CopyActionVisitor(TUnit unit, IGraphForGame<TNode, TEdge, TUnit> graph)
        {
            Unit = unit;
            Graph = graph;
        }

        public void Visit(BuildAction<TNode, TEdge, TUnit> action)
        {
            Result = new BuildAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit> action)
        {
            Result = new BlockAction<TNode, TEdge, TUnit>(Unit, action.ContrAttackDamageModifier, action.Protection);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit> action)
        {
            Result = new MoveAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(HealAction<TNode, TEdge, TUnit> action)
        {
            Result = new HealAction<TNode, TEdge, TUnit>(Unit, action.IsMassHeal, action.HealingAmount);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new DistanceAttackAction<TNode, TEdge, TUnit>(
                Unit, Graph, action.IsPenetratingDamage, action.Distance, action.Type);
            Result.ActionModifier = action.ActionModifier;
        }
        
        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new MeleeAttackAction<TNode, TEdge, TUnit>(Unit,action.IsPenetratingDamage, action.Onslaught, action.BlockerSelector);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit> action)
        {
            Result = new RLBlockAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit> action)
        {
            Result = new SacrificeForPerunAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RamAction<TNode, TEdge, TUnit> action)
        {
            Result = new RamAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit> action)
        {
            Result = new BlowWithSwingAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ShotUnitAction<TNode, TEdge, TUnit> action)
        {
            Result = new ShotUnitAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RLBuildAction<TNode, TEdge, TUnit> action)
        {
            Result = new RLBuildAction<TNode, TEdge, TUnit>(Unit, action.PossibleBuildings, action.Factory);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(HealYourselfAction<TNode, TEdge, TUnit> action)
        {
            Result = new HealYourselfAction<TNode, TEdge, TUnit>(Unit, action.HealAmount);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(StunAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new StunAttackAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(HealingAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new HealingAttackAction<TNode, TEdge, TUnit>(Unit, action.Mode, action.ConsHeal);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(TargetPowerBasedAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new TargetPowerBasedAttackAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(UpArmorAction<TNode, TEdge, TUnit> action)
        {
            Result = new UpArmorAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(PowerBasedHealAction<TNode, TEdge, TUnit> action)
        {
            Result = new PowerBasedHealAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ArmorBasedAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new PowerBasedHealAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ConsumeUnitAction<TNode, TEdge, TUnit> action)
        {
            Result = new ConsumeUnitAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(FogEraseAction<TNode, TEdge, TUnit> action)
        {
            Result = new FogEraseAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ArsonAction<TNode, TEdge, TUnit> action)
        {
            Result = new ArsonAction<TNode, TEdge, TUnit>(Unit, action.FireEffectRounds);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(SpawningUnitAction<TNode, TEdge, TUnit> action)
        {
            Result = new SpawningUnitAction<TNode, TEdge, TUnit>(Unit, action.UnitFabric);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(UpActionPointsAction<TNode, TEdge, TUnit> action)
        {
            Result = new UpActionPointsAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(JumpAction<TNode, TEdge, TUnit> action)
        {
            Result = new JumpAction<TNode, TEdge, TUnit>(Unit, action.MinJumpDistance, action.MaxJumpDistance);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(HealSacrificeAction<TNode, TEdge, TUnit> action)
        {
            Result = new HealSacrificeAction<TNode, TEdge, TUnit>(Unit, action.unitsToBuffAttack);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(VenomousSpitAction<TNode, TEdge, TUnit> action)
        {
            Result = new VenomousSpitAction<TNode, TEdge, TUnit>(Unit, action.VenomRounds);
            Result.ActionModifier = action.ActionModifier; 
        }

        public void Visit(LichKingMeleeAttack<TNode, TEdge, TUnit> action)
        {
            Result = new LichKingMeleeAttack<TNode, TEdge, TUnit>(
                Unit, action.IsPenetratingDamage, action.Onslaught, action.BlockerSelector);
        }
    }

    public static class CopyActionVisitor
    {
        public static CopyActionVisitor<TNode, TEdge, TUnit> 
            Create<TNode, TEdge, TUnit>(TUnit unit, IGraphForGame<TNode, TEdge, TUnit> graph)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>

        {
            return new CopyActionVisitor<TNode, TEdge, TUnit> (unit, graph);
        }
    }
}
