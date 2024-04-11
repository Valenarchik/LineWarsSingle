namespace LineWars.Model
{
    public class MonoLichKingAttackAction: MonoAttackAction<LichKingMeleeAttack<Node, Edge, Unit>>,
        ILichKingMeleeAttack<Node, Edge, Unit>
    {
        public bool Onslaught { get; set; }
        public UnitBlockerSelector BlockerSelector { get; set; }
        
        protected override LichKingMeleeAttack<Node, Edge, Unit> GetAction()
        {
            throw new System.NotImplementedException();
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