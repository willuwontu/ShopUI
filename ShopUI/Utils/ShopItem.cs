using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using ItemShops.Monobehaviours;

namespace ItemShops.Utils
{
    class ShopItem : MonoBehaviour
    {
        private ButtonInteraction interact;
        private Shop shop;
        private GameObject _itemContainer = null;
        private GameObject _costContainer = null;
        public GameObject ItemContainer
        {
            get
            {
                if (!_itemContainer)
                {
                    _itemContainer = this.gameObject.transform.Find("Item Holder").gameObject;
                }

                return _itemContainer;
            }
        }
        public GameObject CostContainer
        {
            get
            {
                if (!_costContainer)
                {
                    _costContainer = this.gameObject.transform.Find("Cost View/Viewport/Content").gameObject;
                }

                return _costContainer;
            }
        }
        public Purchasable Purchasable { get; internal set; }
        public PurchaseLimit PurchaseLimit { get; internal set; }

        internal int _timesPurchased = 0;
        internal Dictionary<Player, int> _timesPlayerPurchased = new Dictionary<Player, int>();

        public int TimesPurchased { get { return _timesPurchased; } }
        public ReadOnlyDictionary<Player, int> TimesPlayerPurchased { get { return new ReadOnlyDictionary<Player, int>(_timesPlayerPurchased); } }

        public void OnPurchase(Player player)
        {
            _timesPurchased++;
            if (_timesPlayerPurchased.TryGetValue(player, out int times))
            {
                times++;
            }
            else
            {
                _timesPlayerPurchased.Add(player, 1);
            }

            Purchasable.OnPurchase(player, Purchasable);
        }

        public void ResetPurchases()
        {
            _timesPurchased = 0;
            _timesPlayerPurchased.Clear();
        }

        private void Start()
        {
            interact = this.gameObject.GetComponent<ButtonInteraction>();
            shop = this.gameObject.GetComponentInParent<Shop>();
            if (!interact || !shop)
            {
                UnityEngine.GameObject.Destroy(this);
                return;
            }
            interact.mouseClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            //bleh;
        }
    }
}
