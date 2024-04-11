namespace LineWars.Model
{
    public interface IDistanceAttackAction<TNode, TEdge, TUnit> :
        IAttackAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        public uint Distance { get; }
        public bool InfinityDistance { get; }
    }
}