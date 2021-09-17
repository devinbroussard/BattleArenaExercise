using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;

        public Item CurrentItem
        {
            get { return _currentItem; }
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[]items) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
        }
    }
}
