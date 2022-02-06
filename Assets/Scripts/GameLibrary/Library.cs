using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Library
{
    private static readonly List<LibraryEntity> All = new ();
    
    public static async Task Load(CancellationToken ct)
    {
        var libraryCatalog =  new LibraryCatalog();
        await libraryCatalog.Load(ct);

        foreach (var pair in libraryCatalog.GetDictionaryAll())
        {
            var value = pair.Value;
            var entity = new LibraryEntity(value.GetType(), pair.Key, value);
            All.Add(entity);
        }
    }
    
    public static LibraryEntity Find(System.Type type, LibraryCatalogNames name)
    {
        return All.Find(e=> e.Type == type && e.Name == name);
    }

    public static T Find<T>(LibraryCatalogNames name) //where T : System.Type
    {
        return (T)All.Find(e=> e.Name == name).Object;
    }
}