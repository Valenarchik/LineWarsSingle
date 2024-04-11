using System;
using System.Diagnostics.CodeAnalysis;
namespace LineWars.Model
{
    public class BlockAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>, 
        IBlockAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {        
        private bool isBlocked;
        public IntModifier ContrAttackDamageModifier { get; set; }
        public bool Protection { get;  set; }
        public event Action<bool, bool> CanBlockChanged;
        
        
        public bool IsBlocked => isBlocked || Protection;
        
        public override CommandType CommandType { get; set; } = CommandType.Block;

        public BlockAction(
            TUnit executor,
            IntModifier contrAttackDamageModifier,
            bool protection) : base(executor)
        {
            ContrAttackDamageModifier = contrAttackDamageModifier;
            Protection = protection;
        }
        
        public bool CanBlock()
        {
            return ActionPointsCondition();
        }

        public void EnableBlock()
        {
            SetBlock(true);
            CompleteAndAutoModify();
        }

        public bool CanContrAttack([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            return (IsBlocked)
                && ContrAttackDamageModifier.Modify(Executor.CurrentPower) > 0;
        }

        public void ContrAttack([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var contrAttackDamage = ContrAttackDamageModifier.Modify(Executor.CurrentPower);

            enemy.CurrentHp -= contrAttackDamage;
            SetBlock(false);
        }

        private void SetBlock(bool value)
        {
            var before = isBlocked;
            isBlocked = value;
            if (before != value)
                CanBlockChanged?.Invoke(before, value);
        }
        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}
