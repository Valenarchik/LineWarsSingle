
using System;

namespace LineWars.Model
{
    public class VenomousSpitAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IVenomousSpitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int venomRounds;
        public override CommandType CommandType { get; set; } = CommandType.VenomousSpit;
        public int VenomRounds => venomRounds;
        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;
        public VenomousSpitAction(TUnit executor, int venomRounds) : base(executor)
        {
            this.venomRounds = venomRounds;
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~VenomousSpitAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }
        
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }
        
        public bool IsAvailable(TUnit target)
        {
            var line = Executor.Node.GetLine(target.Node);
            return ActionPointsCondition()
                    && line != null
                    && Executor.CanMoveOnLineWithType(line.LineType)
                    && target.OwnerId != Executor.OwnerId
                    && Executor.CurrentPower > 0;
        }

        public void Execute(TUnit target)
        {
            var effect = new VenomEffect<TNode, TEdge, TUnit>(target, venomRounds, Executor.CurrentPower);
            target.AddEffect(effect);
            target.DealDamageThroughArmor(Executor.CurrentPower);
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand<TUnit,
                VenomousSpitAction<TNode, TEdge, TUnit>,
                TUnit>(this, target);
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
