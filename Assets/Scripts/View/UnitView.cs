using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class UnitView
    {
        /*private static readonly Dictionary<TeamFlag, GameObject> Originals = new Dictionary<TeamFlag, GameObject>();

        private static string GetPrefabPath(TeamFlag teamFlag)
        {
            switch (teamFlag)
            {
                case TeamFlag.Blue: return "Prefabs/BlueTeam/Blue";
                case TeamFlag.Red:  return "Prefabs/RedTeam/Red";
                default:
                    throw new ArgumentOutOfRangeException(nameof(teamFlag), teamFlag, null);
            }
        }

        public static Task Preload(CancellationToken ct)
        {
            var tasks = new List<Task>();
            var flags = Enum.GetValues(typeof(TeamFlag)).Cast<TeamFlag>().ToArray();
            foreach (var flag in flags)
            {
                tasks.Add(Task.Run(() =>
                {
                    var loadPref = Resources.LoadAsync<GameObject>(GetPrefabPath(flag));
                    while (!loadPref.isDone)
                    {
                        return null;
                    }

                    var go = (GameObject)loadPref.asset;
                    Originals.Add(flag, go);
                    
                    return go;
                }, ct));
            }

            Task.WaitAll(tasks.ToArray());
            
            return null;
        }*/
        
        public static UnitView Create(TeamFlag teamFlag, Transform parent)
        {
            GameObject unit = null;
            switch (teamFlag)
            {
                case TeamFlag.Blue:
                    unit = Library.Find<GameObject>(LibraryCatalogNames.BLUE_TEAM_UNIT);
                    break;
                
                case TeamFlag.Red:
                    unit = Library.Find<GameObject>(LibraryCatalogNames.RED_TEAM_UNIT);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(teamFlag), teamFlag, null);
            }

            if (unit == null)
            {
                throw new Exception($"Cannot create unit view by flag type [{nameof(teamFlag)}]. Not found.");
            }
            
            var go = UnityObject.Instantiate(unit, parent, true);
            return new UnitView(go.transform);
        }
        
        private readonly Transform _transform;

        private UnitView(Transform transform)
        {
            _transform = transform;
        }
        
        public void SetPosition(int x, int y)
        {
            _transform.localPosition = new Vector3(x, _transform.localPosition.y, y);          
        }

        public void Destroy()
        {
            UnityObject.Destroy(_transform.gameObject);
        }
    }
}