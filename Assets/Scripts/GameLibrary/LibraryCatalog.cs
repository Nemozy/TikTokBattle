using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LibraryCatalog
{
    private readonly Dictionary<LibraryCatalogNames, GameObject> _all = new ();
    private readonly Dictionary<LibraryCatalogNames, string> _path = new ();
    
    public LibraryCatalog()
    {
        FillUnits();
    }

    public IDictionary<LibraryCatalogNames, GameObject> GetDictionaryAll()
    {
        return _all;
    }
    
    public async Task Load(CancellationToken ct)
    {
        var tasks = new List<Task>();
        foreach (var key in _path.Keys)
        {
            var task = LoadEntity(key, ct);
            tasks.Add(task);
        }
        
        await Task.WhenAll(tasks);
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

    private void FillUnits()
    {
        _path.Add(LibraryCatalogNames.BLUE_TEAM_UNIT, "Assets/Media/Prefabs/Units/BlueTeam/Blue.prefab");
        _path.Add(LibraryCatalogNames.RED_TEAM_UNIT, "Assets/Media/Prefabs/Units/RedTeam/Red.prefab");
        _path.Add(LibraryCatalogNames.ARCHER_UNIT, "Assets/Media/Prefabs/Units/Archer/ArcherUnit.prefab");
        _path.Add(LibraryCatalogNames.SOLDIER_UNIT, "Assets/Media/Prefabs/Units/Soldier/SoldierUnit.prefab");
        _path.Add(LibraryCatalogNames.ASSASSIN_UNIT, "Assets/Media/Prefabs/Units/Assassin/AssassinUnit.prefab");
    }
}