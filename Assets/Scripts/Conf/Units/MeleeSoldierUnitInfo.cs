namespace Conf
{
    public class MeleeSoldierUnitInfo : UnitInfo
    {
        public int AbilityDamageRate;
        
        public MeleeSoldierUnitInfo() : base(LibraryCatalogNames.SOLDIER_UNIT)
        {
            MaxHealth = 1000;
            MaxMana = 100;
            Speed = 1;
            Damage = 15;
            ManaRegen = 20;
            AttackDistance = 1;
            AbilityDamageRate = 10;
            Visual = new Internal.Visual{PrefabNameInCatalog = LibraryCatalogNames.SOLDIER_UNIT};
        }
    }
}