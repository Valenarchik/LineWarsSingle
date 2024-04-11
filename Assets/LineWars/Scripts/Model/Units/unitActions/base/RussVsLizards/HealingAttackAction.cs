using System;

namespace LineWars.Model
{
    public class HealingAttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IHealingAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.HealingAttack;
        public int Damage => Executor.CurrentPower;
        public HealingAttackMode Mode { get; set; }
        public int ConsHeal { get; set; }
        public event Action<int> DamageChanged;
        
        public HealingAttackAction(TUnit executor, HealingAttackMode mode, int consHeal) : base(executor)
        {
            Executor.UnitPowerChanged += OnUnitPowerChanged;
            this.Mode = mode;
            this.ConsHeal = consHeal;
        }

        ~HealingAttackAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }

        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition() &&
                target.OwnerId != Executor.OwnerId &&
                target != null &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            Attack(target);
            CompleteAndAutoModify();
        }

        private void Attack(TUnit target)
        {
            var enemyNode = target.Node;
            target.DealDamageThroughArmor(Executor.CurrentPower);

            switch (Mode)
            {
                case HealingAttackMode.UseYorSelfPower:
                    Executor.CurrentArmor += Executor.CurrentPower;
                    break;
                case HealingAttackMode.Constant:
                    Executor.CurrentArmor += ConsHeal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (target.IsDied
                && enemyNode.AllIsFree
                && UnitUtilities<TNode, TEdge, TUnit>.CanMoveTo(Executor, enemyNode)
               )
            {
                UnitUtilities<TNode, TEdge, TUnit>.MoveTo(Executor, enemyNode);
            }
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit, HealingAttackAction<TNode, TEdge, TUnit>, TUnit>
                (this, target);
        }
        
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
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

    public enum HealingAttackMode
    {
        UseYorSelfPower,
        Constant
    }
}
