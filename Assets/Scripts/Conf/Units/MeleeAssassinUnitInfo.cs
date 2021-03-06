namespace Conf
{
    public class MeleeAssassinUnitInfo : UnitInfo
    {
        public int AbilityDamageRate;
        public int CritChance;
        public int CritRate;
        
        public MeleeAssassinUnitInfo() : base(LibraryCatalogNames.ASSASSIN_UNIT)
        {
            MaxHealth = 350;
            MaxMana = 100;
            Speed = 2;
            Damage = 55;
            ManaRegen = 10;
            AttackDistance = 1;
            AbilityDamageRate = 4;
            CritChance = 35;
            CritRate = 2;
            Visual = new Internal.Visual{PrefabNameInCatalog = LibraryCatalogNames.ASSASSIN_UNIT};
        }
    }
}