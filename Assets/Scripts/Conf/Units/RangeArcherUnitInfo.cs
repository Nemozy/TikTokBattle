namespace Conf
{
    public class RangeArcherUnitInfo : UnitInfo
    {
        public int KillChance;
        
        public RangeArcherUnitInfo() : base(LibraryCatalogNames.ARCHER_UNIT)
        {
            MaxHealth = 270;
            MaxMana = 100;
            Speed = 3;
            Damage = 60;
            ManaRegen = 50;
            AttackDistance = 10;
            KillChance = 25;
            Visual = new Internal.Visual{PrefabNameInCatalog = LibraryCatalogNames.ARCHER_UNIT};
        }
    }
}