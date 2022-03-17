using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using ItemShops.Monobehaviours;
using TMPro;

namespace ItemShops.Utils
{
    public class ShopItem : MonoBehaviour
    {
        private ButtonInteraction interact;
        private Shop shop;
        private TextMeshProUGUI _text;
        //private GameObject _itemContainer = null;
        //private GameObject _costContainer = null;
        //public GameObject ItemContainer
        //{
        //    get
        //    {
        //        if (!_itemContainer)
        //        {
        //            _itemContainer = this.gameObject.transform.Find("Container/Item Holder").gameObject;
        //        }

        //        return _itemContainer;
        //    }
        //}
        //public GameObject CostContainer
        //{
        //    get
        //    {
        //        if (!_costContainer)
        //        {
        //            _costContainer = this.gameObject.transform.Find("Container/Cost View/Viewport/Content").gameObject;
        //        }

        //        return _costContainer;
        //    }
        //}

        public TextMeshProUGUI Text
        {
            get
            {
                if (!_text)
                {
                    _text = this.gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                }
                return _text;
            }
        }
        private GameObject _highlight = null;

        public GameObject Highlight
        {
            get
            {
                if (!_highlight)
                {
                    _highlight = this.transform.Find("Highlight").gameObject;
                }
                return _highlight;
            }
        }

        private GameObject _darken = null;

        public GameObject Darken
        {
            get
            {
                if (!_darken)
                {
                    _darken = this.transform.Find("Darken").gameObject;
                }
                return _darken;
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

        public bool IsItemPurchasable(Player player = null)
        {
            bool canPurchase = this.Purchasable.CanPurchase(player);

            if (player && (this.PurchaseLimit.perPlayer > 0))
            {
                if (this.TimesPlayerPurchased.TryGetValue(player, out int times))
                {
                    if (times >= this.PurchaseLimit.perPlayer)
                    {
                        canPurchase = false;
                    }
                }
            }

            if (this.PurchaseLimit.global > 0)
            {
                if (this.TimesPurchased >= this.PurchaseLimit.global)
                {
                    canPurchase = false;
                }
            }

            return canPurchase;
        }

        public void UpdateDisplayName(string name)
        {
            this.Text.text = name;
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
            //interact.mouseEnter.AddListener(() => 
            //{ 
            //    Highlight.SetActive(true);  
            //    if (shop.highlightedItem)
            //    {
            //        shop.highlightedItem.GetComponent<ButtonInteraction>().mouseExit?.Invoke();
            //    }
            //    shop.highlightedItem = this; 
            //});
            //interact.mouseExit.AddListener(() => 
            //{ 
            //    Highlight.SetActive(false); 
            //    if (this == shop.highlightedItem)
            //    {
            //        shop.highlightedItem = null;
            //    }
            //});
        }

        private void OnClick()
        {
            shop.OnItemClicked(this);
        }

        internal string ID;
    }
}
