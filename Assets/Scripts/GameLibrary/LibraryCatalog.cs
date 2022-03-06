using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//Класс остался для конфижных сущностей. Пока не удаляю.
public class LibraryCatalog
{
    private readonly Dictionary<LibraryCatalogNames, GameObject> _all = new ();
    private readonly Dictionary<LibraryCatalogNames, string> _path = new ();
    private readonly List<Task> _tasksTmp = new();
    
    public IDictionary<LibraryCatalogNames, GameObject> GetDictionaryAll()
    {
        return _all;
    }
    
    public async Task Load(CancellationToken ct)
    {
        foreach (var key in _path.Keys)
        {
            var task = LoadEntity(key, ct);
            _tasksTmp.Add(task);
        }
        _path.Clear();
        await Task.WhenAll(_tasksTmp);
        _tasksTmp.Clear();
    }

    public async Task LoadEntity(LibraryCatalogNames key, string path, CancellationToken ct)
    {
        _tasksTmp.Add(LoadEntity(key, ct));
        await Task.WhenAll(_tasksTmp);
        _tasksTmp.Clear();
    }

    private async Task LoadEntity(LibraryCatalogNames key, CancellationToken ct)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(_path[key]);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var gameObject = handle.Result;
            _all.Add(key, gameObject);
        }
    }
}