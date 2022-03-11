using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ItemShops.Utils
{
    abstract class Purchasable
    {
        public abstract string Name { get; }
        public abstract Dictionary<string, int> Cost { get; }
        public abstract GameObject CreateItem(GameObject parent);
        public abstract void OnPurchase(Player player, Purchasable item);
        public abstract Tag[] Tags { get; }
    }
}
