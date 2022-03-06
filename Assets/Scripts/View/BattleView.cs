using System;
using System.Collections.Generic;
using Battle;
using Conf;
using Core;
using Pool;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UnityObject = UnityEngine.Object;
using UnitVisual = Conf.UnitInfo.Internal.Visual;

namespace View
{
    public class BattleView : BaseView
    {
        [SerializeField] private Transform _unitContainer;
        [SerializeField] private Button _startBattleButton;
    
        private readonly SortedList<int, UnitView> _unitsViewById = new ();
        private readonly List<UnitView> _unitsView = new ();
        
        private BattleViewModel _viewModel;

        public void Connect(BattleViewModel viewModel)
        {
            base.Connect(new PoolObjectType());
            
            _viewModel = viewModel;
            _viewModel.BattleStatus.OnChange += OnBattleStatusChange;
            _startBattleButton.gameObject.SetActive(true);
            _startBattleButton.onClick.AddListener(OnStartBattleButtonClick);
            _viewModel.OnClearAllUnits += ClearAllUnits;
            _viewModel.OnUnitCreated.OnChange += OnUnitCreatedHandler;
            _viewModel.OnUnitDie.OnChange += OnUnitDieHandler;
            _viewModel.OnUnitMoved.OnChange += OnUnitMovedHandler;
        }

        private void OnUnitMovedHandler((int, int, int) value)
        {
            var unitId = value.Item1;
            var posX = value.Item2;
            var posY = value.Item3;
            var unitView = GetUnitView(unitId);
            unitView.SetPosition(posX, posY);
        }

        private void OnUnitDieHandler(int unitId)
        {
            var unitView = GetUnitView(unitId);
            unitView.PlayDie();
            _unitsView.Remove(unitView);
        }

        private void OnUnitCreatedHandler((Unit, UnitInfo) value)
        {
            var unit = value.Item1;
            var unitInfo = value.Item2;
            var unitView = Create(unit.Team, _unitContainer, unitInfo.Visual, unit.Id, unitInfo);
            _unitsView.Add(unitView);
            _unitsViewById.Add(unitView.Id, unitView);
        }

        private void ClearAllUnits()
        {
            foreach (var unitView in _unitsView)
            {
                unitView.Release();
            }

            _unitsView.Clear();
            _unitsViewById.Clear();
        }
        
        private void DestroyAllUnits()
        {
            //TODO: взять из пулла всех юнитов и отпустить их там
            foreach (var unitView in _unitsView)
            {
                unitView.Release();
                //unitView.Destroy();
            }
        }
        
        private UnitView GetUnitView(int id)
        {
            _unitsViewById.TryGetValue(id, out var unitView);
            if (unitView == null)
            {
                throw new Exception($"Unit with id[{id}] not found.");
            }
            return unitView;
        }
        
        private void OnDestroy()
        {
            _startBattleButton.onClick.RemoveListener(OnStartBattleButtonClick);
            _viewModel.BattleStatus.OnChange -= OnBattleStatusChange;
        }

        private UnitView Create(TeamFlag teamFlag, Transform parent, UnitVisual visual, int id, PoolObjectType type)
        {
            var unitView = GamePool.Pool.Get<UnitView>(type);
            unitView.transform.SetParent(parent);
            unitView.Connect(id, type);
            
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
            if (status == BattleStatus.FinishedWon || status == BattleStatus.FinishedLost)
            {
                _startBattleButton.gameObject.SetActive(true);
            }
        }

        private void OnStartBattleButtonClick()
        {
            _startBattleButton.gameObject.SetActive(false);
            _viewModel.OnGameStart.Value.Invoke();
        }
    }
}