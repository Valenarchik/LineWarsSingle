
using System;

namespace LineWars.Model
{
    public class DistanceAttackAction<TNode, TEdge, TUnit> :
        AttackAction<TNode, TEdge, TUnit>,
        IDistanceAttackAction<TNode, TEdge, TUnit>
    
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        protected readonly IGraphForGame<TNode, TEdge, TUnit> Graph;
        public uint Distance { get; set; }
        public DistanceAttackType Type { get; set; }
        public override CommandType CommandType { get; set; } = CommandType.Fire;
        public bool InfinityDistance => Distance == uint.MaxValue;

        public DistanceAttackAction(
            TUnit executor,
            IGraphForGame<TNode, TEdge, TUnit> graph,
            bool isPenetratingDamage,
            uint distance,
            DistanceAttackType distanceAttackType) : base(executor, isPenetratingDamage)
        {
            Distance = distance;
            Graph = graph;
            Type = distanceAttackType;
        }
        
        public override bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false)
        {
            return !AttackLocked
                   && Executor.CurrentPower > 0
                   && enemy.OwnerId != Executor.OwnerId
                   && (InfinityDistance || Graph.FindShortestPath(node, enemy.Node).Count - 1 <= Distance)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public override void Attack(TUnit enemy)
        {
            switch (Type)
            {
                case DistanceAttackType.Single:
                    enemy.DealDamageThroughArmor(Executor.CurrentPower);
                    break;
                case DistanceAttackType.HalfMultiple:
                    if (enemy.TryGetNeighbour(out var neighbour))
                    {
                        enemy.DealDamageThroughArmor(Executor.CurrentPower/2);
                        neighbour.DealDamageThroughArmor(Executor.CurrentPower/2);
                    }
                    else
                    {
                        enemy.DealDamageThroughArmor(Executor.CurrentPower);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            CompleteAndAutoModify();
        }
        
        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}