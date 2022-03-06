namespace Pool
{
    public class PoolObjectType
    {
        public readonly LibraryCatalogNames ObjectType;
        
        public PoolObjectType(LibraryCatalogNames type = LibraryCatalogNames.EMPTY)
        {
            ObjectType = type;
        }
    }
}