﻿namespace Conf
{
    public class MeleeHedgehogUnitInfo : UnitInfo
    {
        public int HealHpThresholdPercent;
        public int HealValue;
        public int AbilityDamageIncreaseStep;

        public MeleeHedgehogUnitInfo()
        {
            MaxHealth = 2500;
            MaxMana = 100;
            Speed = 1;
            Damage = 5;
            ManaRegen = 20;
            AttackDistance = 2;
            HealHpThresholdPercent = 25;
            HealValue = 200;
            AbilityDamageIncreaseStep = 10;
            Visual = new Internal.Visual{PrefabNameInCatalog = LibraryCatalogNames.RED_TEAM_UNIT};
        }
    }
}