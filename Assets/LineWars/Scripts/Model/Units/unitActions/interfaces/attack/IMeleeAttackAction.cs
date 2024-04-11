namespace LineWars.Model
{
    public interface IMeleeAttackAction<TNode, TEdge, TUnit> :
        IAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public bool Onslaught { get; set; }
        public UnitBlockerSelector BlockerSelector { get; set; }
    }
}