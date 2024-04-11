
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMeleeAttackAction :
        MonoAttackAction<MeleeAttackAction<Node, Edge, Unit>>,
        IMeleeAttackAction<Node, Edge, Unit>
    {
        [field: SerializeField] public UnitBlockerSelector InitialBlockerSelector { get; private set; }

        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [field: SerializeField] public bool InitialOnslaught { get; private set; }

        [SerializeField] private ActionUnitAnimation attackAnimation;
        public bool Onslaught
        {
            get => Action.Onslaught;
            set => Action.Onslaught = value;
        }

        public UnitBlockerSelector BlockerSelector
        {
            get => Action.BlockerSelector;
            set => Action.BlockerSelector = value;
        }

        protected override bool NeedAutoComplete => false;
        
        public override void Attack(ITargetedAlive enemy)
        {
            if (enemy is Unit unit && attackAnimation != null)
                AttackWithAnimation(unit);
            else
            {
                InstantAttack(enemy);
            }    
        }

        private void AttackWithAnimation(Unit unit)
        {
            attackAnimation.SetAction(OnAttack);
            attackAnimation.Ended.AddListener(OnAnimationEnd);
            var context = new AnimationContext()
            {
                TargetUnit = unit
            };
            attackAnimation.Execute(context);
            void OnAnimationEnd(UnitAnimation unitAnimation)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                Player.LocalPlayer.RecalculateVisibility();
                Complete();
            }

            void OnAttack()
            {
                var hasResponses = unit.TryGetComponent(out AnimationResponses responses);
                if (hasResponses)
                    responses.TrySetDeathAnimation(AnimationResponseType.MeleeDied);
                base.Attack(unit);
                if(unit != null && unit.CurrentHp > 0 && hasResponses)
                {
                    var context1 = new AnimationContext()
                    {
                        TargetUnit = Executor
                    };
                    responses.Respond(AnimationResponseType.MeleeDamaged, context1);
                }
            }
        }

        private void InstantAttack(ITargetedAlive enemy)
        {
            base.Attack(enemy);
            Executor.transform.position = Executor.Node.transform.position;
            Player.LocalPlayer.RecalculateVisibility();
            Complete();
        }

        protected override MeleeAttackAction<Node, Edge, Unit> GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit>(
                Executor,
                InitialIsPenetratingDamage,
                InitialOnslaught,
                InitialBlockerSelector);
        }


        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}