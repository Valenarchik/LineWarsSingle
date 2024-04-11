namespace LineWars.Model
{
    public enum CommandType
    {
        None,
        MeleeAttack,
        Heal,
        Explosion,
        Fire,
        Move,
        Build,
        Block,
        SacrificePerun,
        Ram,
        BlowWithSwing,
        ShotUnit,
        VodaBajkalskaya,
        Stun,
        HealingAttack,
        TargetPowerBasedAttack,
        PowerBasedHeal,
        UpArmor,
        ArmorBasedAttack,
        ConsumeUnit,
        EraseFog,
        Arson,
        SpawnWolf,
        SpawnSheep,
        UpActionPoints,
        Jump,
        HealSacrifice,
        VenomousSpit,
        PrashchnikShoot,
        SvatogorShoot
    }

    public static class CommandTypeExtensions
    {
        public static bool IsAttack(this CommandType commandType)
        {
            return commandType is CommandType.Arson
                or CommandType.Ram
                or CommandType.Explosion
                or CommandType.Fire
                or CommandType.Stun
                or CommandType.HealingAttack
                or CommandType.MeleeAttack
                or CommandType.ArmorBasedAttack
                or CommandType.TargetPowerBasedAttack
                or CommandType.BlowWithSwing
                or CommandType.VenomousSpit;
        }
    }
}   

