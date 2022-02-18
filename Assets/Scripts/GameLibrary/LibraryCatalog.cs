using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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
        var handle = Resources.LoadAsync<GameObject>(_path[key]);
        while (!handle.isDone)
        {
            await Task.Yield();
        }

        var go = (GameObject)handle.asset;
        _all.Add(key, go);
    }

    private void FillUnits()
    {
        _path.Add(LibraryCatalogNames.BLUE_TEAM_UNIT, "Prefabs/Units/BlueTeam/Blue");
        _path.Add(LibraryCatalogNames.RED_TEAM_UNIT, "Prefabs/Units/RedTeam/Red");
        _path.Add(LibraryCatalogNames.ARCHER_UNIT, "Prefabs/Units/Archer/ArcherUnit");
        _path.Add(LibraryCatalogNames.SOLDIER_UNIT, "Prefabs/Units/Soldier/SoldierUnit");
        _path.Add(LibraryCatalogNames.ASSASSIN_UNIT, "Prefabs/Units/Assassin/AssassinUnit");
    }
}