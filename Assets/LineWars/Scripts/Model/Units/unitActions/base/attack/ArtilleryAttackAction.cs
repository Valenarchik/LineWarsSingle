namespace LineWars.Model
{
    public class ArtilleryAttackAction<TNode, TEdge, TUnit> :
        DistanceAttackAction<TNode, TEdge, TUnit>,
        IArtilleryAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType { get; set; } = CommandType.Explosion;
        public ArtilleryAttackAction(
            TUnit executor,
            bool isPenetratingDamage,
            uint distance,
            IGraphForGame<TNode, TEdge, TUnit> graph)
            : base(executor, isPenetratingDamage, distance, graph)
        {
        }
        
        public override bool CanAttackFrom(TNode node, TEdge edge, bool ignoreActionPointsCondition = false)
        {
            var pathLen1 = Graph.FindShortestPath(node, edge.FirstNode).Count - 1;
            var pathLen2 = Graph.FindShortestPath(node, edge.SecondNode).Count - 1;
            return !AttackLocked
                   && Executor.CurrentPower > 0
                   && edge.LineType >= LineType.CountryRoad
                   && (pathLen1 <= Distance && pathLen2 + 1 <= Distance
                       || pathLen2 <= Distance && pathLen1 + 1 <= Distance)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }
        
        public override void Attack(TEdge edge)
        {
            edge.CurrentHp -= Executor.CurrentPower;
            CompleteAndAutoModify();
        }

        public override void Attack(TUnit enemy)
        {
            var damage = Executor.CurrentPower;
            if (enemy.TryGetNeighbour(out var neighbour))
            {
                damage /= 2;
                neighbour.CurrentHp -= damage;
            }

            enemy.CurrentHp -= damage;
            CompleteAndAutoModify();
        }


        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }
    }
}