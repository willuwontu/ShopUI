using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemShops.Utils
{
    abstract class Purchasable
    {
        public abstract string Name { get; }
        public abstract Dictionary<string, int> Cost { get; }
        public abstract PurchaseLimit PurchaseLimit { get; }
        public abstract GameObject CreateItem();
        public abstract void OnPurchase(Player player, Purchasable item);
        public abstract List<Tag> Tags { get; }

        internal GameObject _gameObject = null;

        public GameObject GameObject { get { return _gameObject; } }

        internal int _timesPurchased = 0;
        internal Dictionary<Player, int> _timesPlayerPurchased = new Dictionary<Player, int>();
        
        public int TimesPurchased { get { return _timesPurchased; } }
        public Dictionary<Player, int> TimesPlayerPurchased { get { return _timesPlayerPurchased; } }

        public void ResetPurchases()
        {
            _timesPurchased = 0;
            _timesPlayerPurchased.Clear();
        }
    }
}
