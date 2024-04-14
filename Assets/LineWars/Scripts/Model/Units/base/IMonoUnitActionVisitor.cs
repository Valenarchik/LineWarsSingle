namespace LineWars.Model
{
    public interface IMonoUnitActionVisitor
    {
        public void Visit(MonoBuildRoadAction action);
        public void Visit(MonoBlockAction action);
        public void Visit(MonoMoveAction action);
        public void Visit(MonoHealAction action);
        public void Visit(MonoDistanceAttackAction action);
        public void Visit(MonoMeleeAttackAction action);
        public void Visit(MonoRLBlockAction action);
        public void Visit(MonoSacrificeForPerunAction action);
        public void Visit(MonoRamAction action);
        public void Visit(MonoBlowWithSwingAction action);
        public void Visit(MonoShotUnitAction action);
        public void Visit(MonoRLBuildAction action);
        public void Visit(MonoHealYourselfAction action);
        public void Visit(MonoStunAttackAction action);
        public void Visit(MonoHealingAttackAction action);
        public void Visit(MonoTargetPowerBasedAttackAction action);
        public void Visit(MonoUpArmorAction action);
        public void Visit(MonoPowerBasedHealAction action);
        public void Visit(MonoArmorBasedAttackAction action);
        public void Visit(MonoConsumeUnitAction action);
        public void Visit(MonoFogEraseAction action);
        public void Visit(MonoArsonAction action);
        public void Visit(MonoSpawningUnitAction action);
        public void Visit(MonoUpActionPointsAction action);
        public void Visit(MonoJumpAction action);
        public void Visit(MonoHealSacrificeAction action);
        public void Visit(MonoVenomousSpitAction action);
        public void Visit(MonoLichKingAttackAction action);
    }
}