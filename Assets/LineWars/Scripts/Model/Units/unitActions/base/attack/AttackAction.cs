
using System;

namespace LineWars.Model
{
    public abstract class AttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public bool AttackLocked { get;  set; }
        public bool IsPenetratingDamage { get; set; }
        
        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;

        public bool CanAttack(ITargetedAlive enemy, bool ignoreActionPointsCondition = false) =>
            CanAttackFrom(Executor.Node, enemy, ignoreActionPointsCondition);

        public bool CanAttackFrom(TNode node, ITargetedAlive enemy, bool ignoreActionPointsCondition = false)
        {
            return enemy switch
            {
                TEdge edge => CanAttackFrom(node, edge, ignoreActionPointsCondition),
                TUnit unit => CanAttackFrom(node, unit, ignoreActionPointsCondition),
                _ => false
            };
        }

        public void Attack(ITargetedAlive enemy)
        {
            switch (enemy)
            {
                case TEdge edge:
                    Attack(edge);
                    break;
                case TUnit unit:
                    Attack(unit);
                    break;
            }
        }


        public virtual bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false)
        {
            return false;
        }

        public virtual bool CanAttackFrom(TNode node, TEdge edge, bool ignoreActionPointsCondition = false)
        {
            return false;
        }

        public virtual void Attack(TUnit unit)
        {
        }

        public virtual void Attack(TEdge edge)
        {
        }

        protected AttackAction(TUnit executor, bool isPenetratingDamage) : base(executor)
        {
            IsPenetratingDamage = isPenetratingDamage;
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~AttackAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }
        
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }
    }
}