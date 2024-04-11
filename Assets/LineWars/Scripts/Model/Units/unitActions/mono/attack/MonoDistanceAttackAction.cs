using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoDistanceAttackAction :
        MonoAttackAction<DistanceAttackAction<Node, Edge, Unit>>,
        IDistanceAttackAction<Node, Edge, Unit>
    {
        [SerializeField] private DistanceAttackAnimation distanceAttackAnimation;
        [SerializeField] private bool useInfinityDistance;
        [SerializeField, ConditionallyVisible(nameof(useInfinityDistance), true)] private int initialDistance = 1;
        public uint Distance => Action.Distance;
        public bool InfinityDistance => Action.InfinityDistance;
        protected override bool NeedAutoComplete => false;
        
        protected override DistanceAttackAction<Node, Edge, Unit> GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit>(
                Executor,
                InitialIsPenetratingDamage,
                (uint)initialDistance,
                MonoGraph.Instance);
        }

        public override void Attack(ITargetedAlive enemy)
        {
            if(enemy is Unit targetUnit && distanceAttackAnimation != null)
            {
                var context = new AnimationContext()
                {
                    TargetPosition = targetUnit.transform.position
                };

                distanceAttackAnimation.Ended.AddListener(AttackOnEvent);
                distanceAttackAnimation.Execute(context);
            }
            else
            {
                ExecuteAttack(enemy);
            }

            void AttackOnEvent(UnitAnimation _)
            {
                distanceAttackAnimation.Ended.RemoveListener(AttackOnEvent);
                ExecuteAttack(enemy);
            }
        }

        private void ExecuteAttack(ITargetedAlive enemy)
        {
            if (enemy is Unit unit1 && unit1.TryGetComponent(out AnimationResponses responses1))
                responses1.TrySetDeathAnimation(AnimationResponseType.DistanceDied);
            base.Attack(enemy);
            if (enemy == null || enemy.CurrentHp <= 0)
            {
                Complete();
                return;
            }
            if (enemy is Unit unit && unit.TryGetComponent(out AnimationResponses responses))
            {
                var animContext = new AnimationContext()
                {
                    TargetNode = Executor.Node,
                    TargetUnit = Executor
                };
                var respond = responses.Respond(AnimationResponseType.DistanceDamaged, animContext);
                if (respond != null)
                    respond.Ended.AddListener(CompleteOnRespondEnd);
                else
                    Complete();
            }
            else
            {
                Complete();
            }

            void CompleteOnRespondEnd(UnitAnimation respond)
            {
                respond.Ended.RemoveListener(CompleteOnRespondEnd);
                Complete();
            }
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}