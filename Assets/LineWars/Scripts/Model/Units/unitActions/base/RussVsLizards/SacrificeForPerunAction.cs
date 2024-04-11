using System;
using System.Linq;

namespace LineWars.Model
{
    public class SacrificeForPerunAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        ISacrificeForPerunAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType { get; set; } = CommandType.SacrificePerun;

        public SacrificeForPerunAction(TUnit executor) : base(executor)
        {
        }


        public bool CanSacrifice(TNode node)
        {
            return ActionPointsCondition()
                   && node.OwnerId != Executor.OwnerId;
        }

        public void Sacrifice(TNode node)
        {
            var units = new[] {node.LeftUnit, node.RightUnit}
                .Where(x => x != null)
                .Distinct()
                .ToArray();

            if (units.Length != 0)
            {
                var damage = Executor.CurrentHp / units.Length;

                foreach (var unit in units)
                    unit.DealDamageThroughArmor(damage);
            }
            Executor.CurrentHp = 0;
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