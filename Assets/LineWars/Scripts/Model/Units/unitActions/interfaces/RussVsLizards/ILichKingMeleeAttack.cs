namespace LineWars.Model
{
    public interface ILichKingMeleeAttack<TNode, TEdge, TUnit> :
        IMeleeAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        
    }
}