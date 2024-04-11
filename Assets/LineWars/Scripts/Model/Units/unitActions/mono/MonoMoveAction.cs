using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoMoveAction : MonoUnitAction<MoveAction<Node, Edge, Unit>>,
        IMoveAction<Node, Edge, Unit>
    {
        [SerializeField] private SFXData moveSfx;
        [SerializeField] private SFXList reactionsSfx;
        [SerializeField] private UnitAnimation moveAnimation;

        private readonly IDJ dj =  new RandomDJ(0.5f);

        protected override bool NeedAutoComplete => false;

        public bool CanMoveTo(Node target) => Action.CanMoveTo(target);
        
        public void MoveTo(Node target)
        {
            if(moveAnimation == null)
            {
                MoveInstant(target);
            }
            else
            {
                var animContext = new AnimationContext()
                {
                    TargetPosition = target.transform.position
                };
                moveAnimation.Ended.AddListener(OnMoveEnd);
                moveAnimation.Execute(animContext);
            }
            Executor.PlaySfx(moveSfx);
            Executor.PlaySfx(dj.GetSound(reactionsSfx));
            Player.LocalPlayer.RecalculateVisibility();
            void OnMoveEnd(UnitAnimation anim)
            {
                moveAnimation.Ended.RemoveListener(OnMoveEnd);
                MoveInstant(target);
            }
        }

        private void MoveInstant(Node target)
        {
            Action.MoveTo(target);
            Executor.transform.position = target.transform.position;
            Complete();
        }

        protected override MoveAction<Node, Edge, Unit> GetAction()
        {
            var action = new MoveAction<Node, Edge, Unit>(Executor);
            return action;
        }

        public override void Accept(IMonoUnitActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}