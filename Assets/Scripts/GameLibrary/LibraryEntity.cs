public class LibraryEntity
{
    public LibraryCatalogNames Name;
    public readonly System.Type Type;
    public readonly System.Object Object;
    
    public LibraryEntity(System.Type type, LibraryCatalogNames name, System.Object obj)
    {
        Type = type;
        Name = name;
        Object = obj;
    }
}