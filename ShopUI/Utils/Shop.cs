using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Photon.Pun;
using UnboundLib.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ItemShops.Utils
{
    class Shop : MonoBehaviour
    {
        string name;

        List<Purchasable> _items = new List<Purchasable>();

        ReadOnlyCollection<Purchasable> ShopItems
        {
            get
            {
                return new ReadOnlyCollection<Purchasable>(_items);
            }
        }

        public void AddItem(Purchasable item)
        {
            _items.Add(item);
        }

        public void AddItems(Purchasable[] items)
        {
            _items.AddRange(items);
        }

        public void RemoveItem(Purchasable item)
        {
            _items.Remove(item);
        }

        public void ClearShop()
        {
            _items.Clear();
        }

        public void DisplayedColumns()
        {
            bleh;
        }

        public void ItemSizes()
        {
            bleh;
        }

        private void ItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Purchasable[] changedItems;
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    changedItems = ((IList<Purchasable>)args.NewItems).ToArray();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    changedItems = ((IList<Purchasable>)args.OldItems).ToArray();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private void Awake()
        {
            ShopManager.instance._shops.Add(this);
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }

        private void OnDestroy()
        {
            ShopManager.instance._shops.Remove(this);
            OnOnDestroy();
        }

        public virtual void OnOnDestroy()
        {

        }
    }
}
