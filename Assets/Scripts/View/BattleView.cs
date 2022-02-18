using System;
using System.Collections.Generic;
using Battle;
using Conf;
using Core;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private Transform _unitContainer;
        [SerializeField] private Button _startBattleButton;
    
        private readonly SortedList<int, UnitView> _unitsViewById = new ();
        private readonly List<UnitView> _unitsView = new ();
        
        private BattleViewModel _viewModel;
        
        public void Connect(BattleViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.BattleStatus.OnChange += OnBattleStatusChange;
            _startBattleButton.gameObject.SetActive(true);
            _startBattleButton.onClick.AddListener(OnStartBattleButtonClick);
        }

        public void OnUnitCreated(Unit unit, UnitInfo info)
        {
            var unitView = Create(unit.Team, _unitContainer, info.Visual, unit.Id);
            _unitsView.Add(unitView);
            _unitsViewById.Add(unitView.Id, unitView);
        }

        public void OnUnitMoved(int id, int x, int y)
        {
            GetUnitView(id).SetPosition(x, y);
        }

        public void OnUnitDie(int id)
        {
            var unitView = GetUnitView(id);
            if (unitView == null)
            {
                throw new Exception($"Unit with id[{id}] already destoyed");
            }

            unitView.PlayDie();
            //unitView.Destroy();
        }

        public void DestroyAllUnits()
        {
            foreach (var unitView in _unitsView)
            {
                unitView.Destroy();
            }
            _unitsView.Clear();
            _unitsViewById.Clear();
        }
        
        private UnitView GetUnitView(int id)
        {
            _unitsViewById.TryGetValue(id, out var unitView);
            return unitView;
        }
        
        private void OnDestroy()
        {
            _startBattleButton.onClick.RemoveListener(OnStartBattleButtonClick);
            _viewModel.BattleStatus.OnChange -= OnBattleStatusChange;
        }

        private UnitView Create(TeamFlag teamFlag, Transform parent, UnitInfo.Internal.Visual visual, int id)
        {
            var unit = Library.Find<GameObject>(visual.PrefabNameInCatalog);
            if (unit == null)
            {
                throw new Exception($"Cannot create unit view by flag type [{nameof(teamFlag)}]. Not found.");
            }
            
            var go = UnityObject.Instantiate(unit, parent, true);
            var unitView = new UnitView(go.transform, id);
            switch (teamFlag)
            {
                case TeamFlag.Player:
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
        
        private void OnBattleStatusChange(BattleStatus status)
        {
            if (status == BattleStatus.FINISHED_WON || status == BattleStatus.FINISHED_LOST)
            {
                _startBattleButton.gameObject.SetActive(true);
            }
        }

        private void OnStartBattleButtonClick()
        {
            _startBattleButton.gameObject.SetActive(false);
            _viewModel.GameStart();
        }
    }
}