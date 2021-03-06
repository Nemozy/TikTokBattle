using System;
using Conf;
using Logic;
using View;

namespace Core
{
    public class Unit : IUnit
    {
        private enum State
        {
            Spawn,
            Turn,
            Move,
            Die
        }

        private readonly UnitLogic _logic;
        private readonly Battle _battle;
        
        private State _state;
        
        private int _destX;
        private int _destY;
        
        public TeamFlag Team { get; }
        
        public int Id { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public int MaxHealth { get; }
        public int Health { get; private set; }
        public int MaxMana { get; }
        public int Mana { get; private set; }
        public int Speed { get; }

        public Unit(TeamFlag team, UnitInfo info, Battle battle, int id)
        {
            Team = team;
            MaxMana = info.MaxMana;
            MaxHealth = info.MaxHealth;
            Speed = info.Speed;
            Health = MaxHealth;
            Mana = 0;
            
            _logic = UnitLogicFactory.Create(info, this, battle);
            
            _battle = battle;
            _state = State.Spawn;
            Id = id;
        }

        public void Tick()
        {
            switch (_state)
            {
                case State.Spawn:
                    _logic.OnSpawn();
                    _state = State.Turn;
                    break;
                case State.Move:
                    _battle.AskMoveUnitTo(this, _destX, _destY);
                    _state = State.Turn;
                    break;
                case State.Turn:
                    if (Mana == MaxMana)
                    {
                        _logic.OnAbility();
                        SubMana(MaxMana);
                        break;
                    }
                    _logic.OnTurn();
                    break;
                case State.Die:
                    _logic.OnDie();
                    break;
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }
        
        public void AddMana(int mana)
        {
            mana = _logic.OnBeforeManaChange(mana);
            Mana = Math.Min(MaxMana, Mana + mana);
        }
        
        public void SubMana(int mana)
        {
            mana = -_logic.OnBeforeManaChange(-mana);
            Mana = Math.Min(MaxMana, Mana - mana);
        }
        
        public void Heal(int heal)
        {
            heal = _logic.OnHeal(heal);
            Health = Math.Min(MaxHealth, Health + heal);
        }

        public void Damage(int damage)
        {
            damage = _logic.OnDamage(damage);
            Health = Math.Max(0, Health - damage);
            if (!IsAlive())
            {
                _state = State.Die;
            }
        }

        public void MoveTo(int x, int y)
        {
            _destX = x;
            _destY = y;
            _state = State.Move;
        }
    }
}