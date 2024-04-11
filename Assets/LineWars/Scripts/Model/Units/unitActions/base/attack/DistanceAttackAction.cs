
namespace LineWars.Model
{
    public class DistanceAttackAction<TNode, TEdge, TUnit> :
        AttackAction<TNode, TEdge, TUnit>,
        IDistanceAttackAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        protected readonly IGraphForGame<TNode, TEdge, TUnit> Graph;
        public uint Distance { get; set; }
        public override CommandType CommandType { get; set; } = CommandType.Fire;

        public DistanceAttackAction(
            TUnit executor,
            bool isPenetratingDamage,
            uint distance,
            IGraphForGame<TNode, TEdge, TUnit> graph) : base(executor, isPenetratingDamage)
        {
            Distance = distance;
            Graph = graph;
        }

        
        public override bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false)
        {
            return !AttackLocked
                   && Executor.CurrentPower > 0
                   && enemy.OwnerId != Executor.OwnerId
                   && Graph.FindShortestPath(node, enemy.Node).Count - 1 <= Distance
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public override void Attack(TUnit enemy)
        {
            enemy.DealDamageThroughArmor(Executor.CurrentPower);
            CompleteAndAutoModify();
        }
        

        
        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}