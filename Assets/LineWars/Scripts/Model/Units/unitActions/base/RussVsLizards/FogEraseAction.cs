using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class FogEraseAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IFogEraseAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public FogEraseAction(TUnit executor) : base(executor)
        {
        }

        public override CommandType CommandType { get; set; } = CommandType.EraseFog;

        public bool IsAvailable(TNode target)
        {
            return target.OwnerId != Executor.OwnerId;
        }

        public void Execute(TNode target)
        {
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TNode target)
        {
            return new TargetedUniversalCommand<
                TUnit,
                FogEraseAction<TNode, TEdge, TUnit>,
                TNode>(Executor, target);
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
