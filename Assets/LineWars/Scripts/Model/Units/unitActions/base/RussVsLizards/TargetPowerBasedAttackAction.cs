using System;

namespace LineWars.Model
{
    public class TargetPowerBasedAttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        ITargetPowerBasedAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.TargetPowerBasedAttack;
        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }

        public TargetPowerBasedAttackAction(TUnit executor) : base(executor)
        {
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~TargetPowerBasedAttackAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }

        public bool IsAvailable(TUnit target)
        {
            return target != null &&
                ActionPointsCondition() &&
                //target.CurrentPower != 0 &&
                target.OwnerId != Executor.OwnerId &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            target.DealDamageThroughArmor(target.CurrentPower + Executor.CurrentPower);
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit, TargetPowerBasedAttackAction<TNode, TEdge, TUnit>, TUnit>
                (this, target);
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
