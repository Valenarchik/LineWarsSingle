using System;

namespace LineWars.Model
{
    public class JumpAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IJumpAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.Jump;

        public int MinJumpDistance { get; set; }

        public int MaxJumpDistance { get; set; }

        public JumpAction(
            TUnit executor, 
            int minJumpDistance, 
            int maxJumpDistance) : base(executor)
        {
            this.MinJumpDistance = minJumpDistance;
            this.MaxJumpDistance = maxJumpDistance;
        }

        public bool IsAvailable(TNode target)
        {
            var distance = Graph<TNode, TEdge>.StaticFindShortestPath(Executor.Node, target).Count;
            return distance >= MinJumpDistance &&
                distance <= MaxJumpDistance &&
                FreeNodeCondition() &&
                ForBigCondition() &&
                ActionPointsCondition()
                && target.CanOwnerMove(Executor.OwnerId);

            bool FreeNodeCondition()
            {
                return target.AnyIsFree || target.LeftUnit.OwnerId != Executor.OwnerId;
            }

            bool ForBigCondition()
            {
                return Executor.Size == UnitSize.Little ||
                    target.AllIsFree ||
                    (!target.LeftIsFree && target.LeftUnit.OwnerId != Executor.OwnerId) ||
                    (!target.RightIsFree && target.RightUnit.OwnerId != Executor.OwnerId);
            }
        }

        public void Execute(TNode target)
        {
            if(!target.AnyIsFree)
            {
                Executor.CurrentHp = 0;
                CompleteAndAutoModify();
                return;
            }
            if((!target.LeftIsFree && target.LeftUnit.OwnerId != Executor.OwnerId) ||
                (!target.RightIsFree && target.RightUnit.OwnerId != Executor.OwnerId))
            {
                Executor.CurrentHp = 0;
                CompleteAndAutoModify();
                return;
            }
            UnitUtilities<TNode, TEdge, TUnit>.MoveTo(Executor, target);
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TNode target)
        {
            return new TargetedUniversalCommand<
                TUnit,
                JumpAction<TNode, TEdge, TUnit>,
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
