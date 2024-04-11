using System;
using System.Linq;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IShotUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType { get; set; } = CommandType.ShotUnit;


        public ShotUnitAction(TUnit executor) : base(executor)
        {
        }

        public bool IsAvailable([NotNull] TUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            var line = Executor.Node.GetLine(unit.Node);
            return ActionPointsCondition()
                   && unit.Size == UnitSize.Little
                   && (Executor.IsNeighbour(unit) || line != null)
                   && Executor != unit;
        }

        public bool IsAvailable(TUnit unitTarget, TNode nodeTarget)
        {
            return IsAvailable(unitTarget)
                   && BanCondition(unitTarget, nodeTarget)
                   && nodeTarget != Executor.Node
                   && nodeTarget != unitTarget.Node;
        }

        private bool BanCondition(TUnit unit, TNode node)
        {
            return node.CanOwnerMove(unit.OwnerId);
        }

        public void Execute(TUnit takenUnit, TNode node)
        {
            if (takenUnit.Node.LeftUnit == takenUnit)
                takenUnit.Node.LeftUnit = null;
            if (takenUnit.Node.RightUnit == takenUnit)
                takenUnit.Node.RightUnit = null;

            if (node.AllIsFree)
            {
                AssignNodeAndUnit(takenUnit, node);
            }
            else
            {
                var enemies = node.Units.ToArray();
                var myDamage = (takenUnit.CurrentHp + takenUnit.CurrentArmor) / enemies.Length;
                var enemyDamage = enemies.Select(x => x.CurrentHp + x.CurrentArmor).Sum();
                foreach (var enemy in enemies)
                    enemy.DealDamageThroughArmor(myDamage);
                takenUnit.DealDamageThroughArmor(enemyDamage);
                if (!takenUnit.IsDied)
                {
                    AssignNodeAndUnit(takenUnit, node);
                }
            }


            CompleteAndAutoModify();
        }

        private static void AssignNodeAndUnit(TUnit takenUnit, TNode node)
        {
            takenUnit.Node = node;
            takenUnit.UnitDirection = UnitDirection.Left;
            node.LeftUnit = takenUnit;
            if (node.OwnerId != takenUnit.OwnerId)
                node.ConnectTo(takenUnit.OwnerId);
        }


        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) =>
            visitor.Visit(this);
    }
}