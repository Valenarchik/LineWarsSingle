using System.Collections.Generic;

namespace LineWars.Model
{
    public class HealSacrificeAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IHealSacrificeAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.HealSacrifice;
        public readonly IReadOnlyDictionary<UnitType, int> unitsToBuffAttack;

        public HealSacrificeAction(TUnit executor, IReadOnlyDictionary<UnitType, int> unitsToBuffAttack) : base(executor)
        {
            this.unitsToBuffAttack = unitsToBuffAttack;
        }

        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition() && 
                target.OwnerId == Executor.OwnerId
                && target != Executor;
         }

        public void Execute(TUnit target)
        {
            foreach(var node in Executor.Node.GetNeighbors())
            {
                foreach (var unit in node.Units)
                {
                    if (unit.OwnerId == Executor.OwnerId)
                    {
                        if (unitsToBuffAttack != null && unitsToBuffAttack.TryGetValue(unit.Type, out var buff))
                            unit.AddEffect(new PowerBuffEffect<TNode, TEdge, TUnit>(unit, buff));
                        unit.CurrentHp += Executor.CurrentHp;
                    }
                }
            }
            Executor.CurrentHp = 0;
            CompleteAndAutoModify();
        }
        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand<TUnit,
                HealSacrificeAction<TNode, TEdge, TUnit>,
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
