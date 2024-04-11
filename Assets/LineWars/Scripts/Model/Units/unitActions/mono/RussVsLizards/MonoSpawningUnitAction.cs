using UnityEngine;

namespace LineWars.Model
{
    public class MonoSpawningUnitAction :
        MonoUnitAction<SpawningUnitAction<Node, Edge, Unit>>,
        ISpawningUnitAction<Node, Edge, Unit>
    {
        [SerializeField] private Unit spawnUnit;
        [SerializeField] private UnitAnimation spawnAnimation;

        protected override bool NeedAutoComplete => false;

        protected override SpawningUnitAction<Node, Edge, Unit> GetAction()
        {
            var player = new System.Lazy<BasePlayer>(() => Executor.Owner);
            var unitFabric = new MonoUnitFabric(player, spawnUnit);
            return new SpawningUnitAction<Node, Edge, Unit>(Executor, unitFabric);
        }

        public void Execute(Node target)
        {
            if (spawnAnimation == null)
                ExecuteInstant(target);
            else
                ExecuteAnimation(target);
        }

        private void ExecuteInstant(Node target)
        {
            Action.Execute(target);
            Complete();
        }

        private void ExecuteAnimation(Node target)
        {
            var context = new AnimationContext()
            {
                TargetPosition = target.Position
            };

            spawnAnimation.Ended.AddListener(OnAnimEnd);
            spawnAnimation.Execute(context);

            void OnAnimEnd(UnitAnimation _)
            {
                ExecuteInstant(target);
            }
        }

        public bool IsAvailable(Node target)
        {
            return Action.IsAvailable(target);
        }

        public IActionCommand GenerateCommand(Node target)
        {
            return new TargetedUniversalCommand<
                Unit,
                MonoSpawningUnitAction,
                Node>(this, target);
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
