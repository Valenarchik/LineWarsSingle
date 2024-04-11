using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IMoveAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType { get; set; } = CommandType.Move;

        public MoveAction(TUnit executor) : base(executor)
        {
        }

        public bool CanMoveTo([NotNull] TNode target)
        {
            return ActionPointsCondition()
                   && UnitUtilities<TNode, TEdge, TUnit>.CanMoveTo(Executor, target);
        }

        public void MoveTo([NotNull] TNode target)
        {
            InspectNodeForCallback();
            UnitUtilities<TNode, TEdge, TUnit>.MoveTo(Executor, target);
            CompleteAndAutoModify();

            void InspectNodeForCallback()
            {
                if (target.OwnerId == -1)
                {
                    OnCapturingFreeNode();
                    return;
                }

                if (target.OwnerId != Executor.OwnerId)
                {
                    OnCapturingEnemyNode();
                    if (target.IsBase)
                        OnCapturingEnemyBase();
                }
            }
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        #region CallBack

        protected virtual void OnCapturingEnemyBase()
        {
        }

        protected virtual void OnCapturingEnemyNode()
        {
        }

        protected virtual void OnCapturingFreeNode()
        {
        }

        #endregion
    }
}