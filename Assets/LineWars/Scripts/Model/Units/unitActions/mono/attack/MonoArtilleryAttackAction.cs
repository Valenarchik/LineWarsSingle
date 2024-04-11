using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoArtilleryAttackAction :
        MonoAttackAction<ArtilleryAttackAction<Node, Edge, Unit>>,
        IArtilleryAttackAction<Node, Edge, Unit>
    {
        [field: SerializeField, Min(0)] public int InitialDistance { get; private set; }
        [SerializeField] private SimpleEffect explosionPrefab;
        public uint Distance => Action.Distance;
        public bool InfinityDistance => Action.InfinityDistance;

        public override void Attack(ITargetedAlive enemy)
        {
            if (enemy is not MonoBehaviour mono) return;
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = mono.transform.position;
            Executor.PlaySfx(attackSfx);
            explosion.Ended += () => { Action.Attack(enemy); };
        }

        protected override ArtilleryAttackAction<Node, Edge, Unit> GetAction()
        {
            return new ArtilleryAttackAction<Node, Edge, Unit>(Executor,
                InitialIsPenetratingDamage,
                (uint) InitialDistance,
                MonoGraph.Instance);
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}