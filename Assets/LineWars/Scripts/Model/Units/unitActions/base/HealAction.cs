using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public class HealAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IHealAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public bool IsMassHeal { get; set; }
        public int HealingAmount { get; set; }
        public bool HealLocked { get; set; }
        
        public override CommandType CommandType { get; set; } = CommandType.Heal;

        public HealAction(TUnit executor, bool isMassHeal, int healingAmount) : base(executor)
        {
            IsMassHeal = isMassHeal;
            HealingAmount = healingAmount;
        }

        public bool CanHeal([NotNull] TUnit target, bool ignoreActionPointsCondition = false)
        {
            return !HealLocked
                   && OwnerCondition()
                   && SpaceCondition()
                   && (ignoreActionPointsCondition || ActionPointsCondition())
                   && target != Executor
                   && target.CurrentHp != target.MaxHp;

            bool SpaceCondition()
            {
                var line = Executor.Node.GetLine(target.Node);
                return line != null || Executor.IsNeighbour(target);
            }

            bool OwnerCondition()
            {
                return target.OwnerId == Executor.OwnerId;
            }
        }

        public void Heal([NotNull] TUnit target)
        {
            target.CurrentHp += HealingAmount;
            if (IsMassHeal && Executor.TryGetNeighbour(out var neighbour))
                neighbour.CurrentHp += HealingAmount;


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