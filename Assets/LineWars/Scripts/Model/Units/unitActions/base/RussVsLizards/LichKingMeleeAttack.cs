using System;

namespace LineWars.Model
{
    public class LichKingMeleeAttack<TNode, TEdge, TUnit> : AttackAction<TNode, TEdge, TUnit>,
        ILichKingMeleeAttack<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.LichKingAttack;
        public bool Onslaught { get; set; }
        public UnitBlockerSelector BlockerSelector { get; set; }
        
        public LichKingMeleeAttack(TUnit executor,
            bool isPenetrating,
            bool onslaught,
            UnitBlockerSelector blockerSelector) : base(executor, isPenetrating)
        {
            Onslaught = onslaught;
            if (blockerSelector == null)
                throw new ArgumentNullException($"blocker selector is null");
            BlockerSelector = blockerSelector;
        }
        
        public override bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false)
        {
            // TODO
            return base.CanAttackFrom(node, enemy, ignoreActionPointsCondition);
        }

        public override void Attack(TUnit unit)
        {
            // TODO
            base.Attack(unit);
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