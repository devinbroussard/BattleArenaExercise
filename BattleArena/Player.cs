﻿using System;
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

        /// <summary>
        /// Tries to equip item at the given index of the _items array
        /// </summary>
        /// <param name="index"></param>
        /// <returns>False if the index is outside the bounds of the array</returns>
        public bool TryEquipItem(int index)
        {
            if (index >= _items.Length || index < 0)
                return false;

            _currentItem = _items[index];

            return true;
        }

        /// <summary>
        /// Set the current item to be nothing
        /// </summary>
        /// <returns>False if there is no item equipped</returns>
        public bool TryRemoveCurrentItem()
        {
            if (_currentItem.Name == "Nothing")
                return false;

            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }
    }
}
