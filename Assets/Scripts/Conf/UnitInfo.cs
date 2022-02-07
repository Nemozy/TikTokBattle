namespace Conf
{
    public class UnitInfo
    {
        public int MaxHealth;
        public int MaxMana;
        public int ManaRegen;
        public int Speed;
        public int Damage;
        public int AttackDistance;
        public Internal.Visual Visual;
        
        public static class Internal
        {
            public class Visual
            {
                public LibraryCatalogNames PrefabNameInCatalog;
            }
        }
    }
}