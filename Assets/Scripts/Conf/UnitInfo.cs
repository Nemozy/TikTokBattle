using Pool;

namespace Conf
{
    public class UnitInfo : PoolObjectType
    {
        public int MaxHealth;
        public int MaxMana;
        public int ManaRegen;
        public int Speed;
        public int Damage;
        public int AttackDistance;
        public Internal.Visual Visual;

        public UnitInfo(LibraryCatalogNames type) : base(type)
        {
        }

        public static class Internal
        {
            public class Visual
            {
                public LibraryCatalogNames PrefabNameInCatalog;
            }
        }
    }
}