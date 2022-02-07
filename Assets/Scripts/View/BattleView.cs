using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class BattleView
    {
        private readonly Transform _unitContainer;
        private readonly Transform _transform;
        private readonly Dictionary<int, UnitView> _unitsView;

        public BattleView(Transform transform, Transform unitContainer)
        {
            _unitsView = new Dictionary<int, UnitView>();
            _transform = transform;
            _unitContainer = unitContainer;
        }
        
        public void OnUnitCreated(Unit unit)
        {
            _unitsView.Add(unit.Id, Create(unit.Team, _unitContainer, unit.Id));
        }

        public void OnUnitMoved(int id, int x, int y)
        {
            _unitsView[id].SetPosition(x, y);
        }

        public void OnUnitDestroy(int id)
        {
            if (!_unitsView.ContainsKey(id))
            {
                throw new Exception($"Unit with id[{id}] already destoyed");
            }
            _unitsView[id].Destroy();
        }
        
        public UnitView Create(TeamFlag teamFlag, Transform parent, int id)
        {
            var unit = Library.Find<GameObject>(LibraryCatalogNames.BLUE_TEAM_UNIT);
            if (unit == null)
            {
                throw new Exception($"Cannot create unit view by flag type [{nameof(teamFlag)}]. Not found.");
            }
            
            var go = UnityObject.Instantiate(unit, parent, true);
            var unitView = new UnitView(go.transform, id);
            switch (teamFlag)
            {
                case TeamFlag.Blue:
                    unitView.SetTeamColor(Color.blue);
                    break;
                
                case TeamFlag.Red:
                    unitView.SetTeamColor(Color.red);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(teamFlag), teamFlag, null);
            }
            return unitView;
        }
    }
}