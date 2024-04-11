using System;

namespace LineWars.Model
{
    public class StunAttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IStunAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.Stun;
        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }
        public StunAttackAction(TUnit executor) : base(executor)
        {
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~StunAttackAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }

        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition() &&
                target != null &&
                target.OwnerId != Executor.OwnerId &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            Stun(target);
            CompleteAndAutoModify();
        }

        private void Stun(TUnit target)
        {
            target.CurrentActionPoints -= 1;
            target.DealDamageThroughArmor(Executor.CurrentPower);
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit,
                StunAttackAction<TNode, TEdge, TUnit>,
                TUnit>(this, target);
        }
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }
    }
}
