using System;

namespace LineWars.Model
{
    public class ArsonAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IArsonAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType { get; set; } = CommandType.Arson;
        public int FireEffectRounds { get; set; }

        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;

        public ArsonAction(TUnit executor, int fireEffectRounds) : base(executor)
        {
            this.FireEffectRounds = fireEffectRounds;
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~ArsonAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }

        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }

        public bool IsAvailable(TNode target)
        { 
            var line = Executor.Node.GetLine(target);
            return ActionPointsCondition()
                    && line != null
                    && target.OwnerId != Executor.OwnerId
                    && !target.AllIsFree;
        }

        public void Execute(TNode target)
        {
            if(!target.LeftIsFree && target.LeftUnit.Size == UnitSize.Little)
            {
                FireUnit(target.LeftUnit);
            }
            if(!target.RightIsFree)
            {
                FireUnit(target.RightUnit);
            }
            CompleteAndAutoModify();
        }

        private void FireUnit(TUnit unit)
        {
            var effect = new FireEffect<TNode, TEdge, TUnit>
                (unit, FireEffectRounds, Executor.CurrentPower);
            unit.AddEffect(effect);
        }

        public IActionCommand GenerateCommand(TNode target)
        {
            return new TargetedUniversalCommand<
                TUnit,
                ArsonAction<TNode, TEdge, TUnit>,
                TNode>(Executor, target);
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
