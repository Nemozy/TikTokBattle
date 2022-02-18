using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Conf;
using UnityEngine;
using View;

namespace Core
{
    public class Battle : ICore, IBattle
    {
        public BattleStatus BattleStatus { get; private set; }

        private readonly MapGrid<Unit> _grid = new ();
        private readonly List<Unit> _units = new ();
        private readonly BattleView _battleView;
        private int _idCounter;
        
        public Battle(BattleView battleView)
        {
            _battleView = battleView;
            _idCounter = 0;
            BattleStatus = BattleStatus.LOADING;
            Clear();
        }
        
        public IUnit GetNearestEnemy(IUnit unit)
        {
            var enemies = unit.Team == TeamFlag.Red ? GetPlayerTeam() : GetRedTeam();
            return GetNearestAtTeam(unit, enemies);
        }

        public IUnit GetNearestFriend(IUnit unit)
        {
            var friends = unit.Team == TeamFlag.Red ? GetRedTeam() : GetPlayerTeam();
            return GetNearestAtTeam(unit, friends);
        }
        
        public float GetDistance(int x1, int y1, int x2, int y2)
        {
            return Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));
        }

        public float GetDistance(IUnit first, IUnit second)
        {
            return GetDistance(first.X, first.Y, second.X, second.Y);
        }
        
        public void Start(BattleInfo info)
        {
            _idCounter = 0;
            BattleStatus = BattleStatus.STARTED;
            foreach (var entry in info.Units)
            {
                SpawnUnit(entry.Flag, entry.Info, entry.SpawnX, entry.SpawnY);
            }
        }

        public void Tick()
        {
            LogicTick();
        }
        
        public void Finish()
        {
            Clear();
        }
        
        public bool IsFinished()
        {
            return !GetRedTeam().Any() || !GetPlayerTeam().Any();
        }

        public void AskMoveUnitTo(Unit unit, int toX, int toY)
        {
            var distance = GetDistance(unit.X, unit.Y, toX, toY);
            var step = Math.Min((int)Math.Round(distance), unit.Speed);
            var path = PathFinding.FindNextPoint(_grid, unit.X, unit.Y, toX, toY, step);
            if (path == Vector2.negativeInfinity)
            {
                return;
            }

            MoveUnit(unit, (int)path.x, (int)path.y);
        }
        
        private int GenerateUnitId()
        {
            return ++_idCounter;
        }

        private void Clear()
        {
            DestroyAllUnits();
            _grid.Clear();
            _units.Clear();
        }
        
        private void LogicTick()
        {
            var dead = new List<Unit>();
            foreach (var unit in _units)
            {
                if (unit.IsAlive())
                {
                    unit.Tick();
                }
                else
                {
                    dead.Add(unit);
                    _battleView.OnUnitDie(unit.Id);
                }
            }
            dead.ForEach(OnUnitDie);
            dead.Clear();

            if (!GetRedTeam().Any())
            {
                BattleStatus = BattleStatus.FINISHED_WON;
                return;
            }
            if (!GetPlayerTeam().Any())
            {
                BattleStatus = BattleStatus.FINISHED_LOST;
            }
        }

        private bool IsCellTaken(Vector2 position)
        {
            return _grid[(int) position.x, (int) position.y] != null;
        }

        private IEnumerable<IUnit> GetRedTeam()
        {
            return _units.Where(u => u.Team == TeamFlag.Red && u.IsAlive());
        }

        private IEnumerable<IUnit> GetPlayerTeam()
        {
            return _units.Where(u => u.Team == TeamFlag.Player && u.IsAlive());
        }
        
        private IUnit GetNearestAtTeam(IUnit unit, IEnumerable<IUnit> team)
        {
            var min = 0.0f;
            IUnit nearest = null;
            foreach (var current in team)
            {
                if (current == unit || !current.IsAlive())
                {
                    continue;
                }
                var distance = GetDistance(current, unit);
                if (nearest == null || distance < min)
                {
                    min = distance;
                    nearest = current;
                }
            }
            return nearest;
        }

        private void SpawnUnit(TeamFlag flag, UnitInfo info, int x, int y)
        {
            if (_grid[x, y] != null)
            {
                throw new ArgumentException($"Supplied coords ({x},{y}) already taken");
            }
            var unit = new Unit(flag, info, this, GenerateUnitId());
            _battleView.OnUnitCreated(unit, info);
            PlaceUnit(unit, x, y);
            _units.Add(unit);
        }

        private void MoveUnit(Unit unit, int x, int y)
        {
            _grid[unit.X, unit.Y] = null;
            PlaceUnit(unit, x, y);
        }
        
        private void PlaceUnit(Unit unit, int x, int y)
        {
            _grid[x, y] = unit;
            unit.X = x;
            unit.Y = y;
            _battleView.OnUnitMoved(unit.Id, x, y);
        }
        
        private void OnUnitDie(Unit unit)
        {
            _grid[unit.X, unit.Y] = null;
            _units.Remove(unit);
        }

        private void DestroyAllUnits()
        {
            _battleView.DestroyAllUnits();
        }
    }
}