using System;
using System.Collections;
using System.Linq;

namespace LineWars.Model
{
    public class RamAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IRamAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType { get; set; } = CommandType.Ram;
        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;
        
        public RamAction(TUnit executor) : base(executor)
        {
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~RamAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }
        
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }

        public bool CanRam(TNode node)
        {
            var line = Executor.Node.GetLine(node);
            return ActionPointsCondition()
                   && line != null
                   && Executor.CanMoveOnLineWithType(line.LineType)
                   && node.OwnerId != Executor.OwnerId
                   && !node.AllIsFree;
        }

        public void Ram(TNode node)
        {
            var enumerator = SlowRam(node);
            while (enumerator.MoveNext())
            {
            }
        }

        public IEnumerator SlowRam(TNode enemyNode)
        {
            var enemies = enemyNode.Units
                .ToArray();
            var damage = Executor.CurrentPower / enemies.Length;
            var possibleNodeForRetreat = enemyNode
                .GetNeighbors()
                .Where(node => node.OwnerId == enemyNode.OwnerId)
                .ToArray();

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentHp + enemy.CurrentArmor <= damage)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit(enemy);
                    continue;
                }

                var nodeForRetreat = possibleNodeForRetreat
                    .FirstOrDefault(x => UnitUtilities<TNode, TEdge, TUnit>.CanMoveTo(enemy, x));
                if (nodeForRetreat == null)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit(enemy);
                    continue;
                }

                enemy.DealDamageThroughArmor(damage);
                UnitUtilities<TNode, TEdge, TUnit>.MoveTo(enemy, nodeForRetreat);
                yield return new MovedUnit(enemy, nodeForRetreat);
            }

            if (UnitUtilities<TNode, TEdge, TUnit>.CanMoveTo(Executor, enemyNode))
            {
                UnitUtilities<TNode, TEdge, TUnit>.MoveTo(Executor, enemyNode);
                yield return new MovedUnit(Executor, enemyNode);
            }
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