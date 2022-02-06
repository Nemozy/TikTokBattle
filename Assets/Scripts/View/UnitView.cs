using System;
using Core;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class UnitView
    {
        public static UnitView Create(TeamFlag teamFlag, Transform parent)
        {
            GameObject unit;
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