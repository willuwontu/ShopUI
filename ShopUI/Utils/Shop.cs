using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ItemShops.Extensions;
using ItemShops.Monobehaviours;

namespace ItemShops.Utils
{
    public class Shop : MonoBehaviour
    {
        string _name;

        public bool IsOpen
        {
            get
            {
                return this.gameObject.activeSelf;
            }
        }
        public string Name { get { return _name; } }

        List<ShopItem> _items = new List<ShopItem>();

        ShopItem currentPurchase = null;
        Player currentPlayer = null;

        private TextMeshProUGUI _title = null;
        private GameObject _tagContainer = null;
        private TMP_InputField _filter = null;
        private GameObject _itemContainer = null;
        private TextMeshProUGUI _purchaseNameText = null;
        private GameObject _purchaseCostContainer = null;
        private Button _purchaseButton = null;
        private GameObject _moneyContainer = null;

        public virtual TextMeshProUGUI Title
        {
            get
            {
                if (!_title)
                {
                    _title = this.gameObject.transform.Find("Titlebar/Title").GetComponent<TextMeshProUGUI>();
                }

                return _title;
            }
        }
        public virtual GameObject TagContainer
        {
            get
            {
                if (!_tagContainer)
                {
                    _tagContainer = this.gameObject.transform.Find("Shop Sections/Filter Section/Tag View/Viewport/Content").gameObject;
                }

                return _tagContainer;
            }
        }
        public virtual TMP_InputField Filter
        {
            get
            {
                if (!_filter)
                {
                    _filter = this.gameObject.transform.Find("Shop Sections/Filter Section/Item Filter").GetComponent<TMP_InputField>();
                }

                return _filter;
            }
        }
        public virtual GameObject ItemContainer
        {
            get
            {
                if (!_itemContainer)
                {
                    _itemContainer = this.gameObject.transform.Find("Shop Sections/Item Section/Item View/Viewport/Content/Grid").gameObject;
                }

                return _itemContainer;
            }
        }
        public virtual TextMeshProUGUI PurchaseNameText
        {
            get
            {
                if (!_purchaseNameText)
                {
                    _purchaseNameText = this.gameObject.transform.Find("Shop Sections/Item Section/Purchase Area/Item Info/Name Container/Name").GetComponent<TextMeshProUGUI>();
                }

                return _purchaseNameText;
            }
        }
        public virtual GameObject PurchaseCostContainer
        {
            get
            {
                if (!_purchaseCostContainer)
                {
                    _purchaseCostContainer = this.gameObject.transform.Find("Shop Sections/Item Section/Purchase Area/Item Info/Cost View/Viewport/Content").gameObject;
                }
                return _purchaseCostContainer;
            }
        }
        public virtual Button PurchaseButton
        {
            get
            {
                if (!_purchaseButton)
                {
                    _purchaseButton = this.gameObject.transform.Find("Shop Sections/Item Section/Purchase Area/Purchase Button").GetComponent<Button>();
                }
                return _purchaseButton;
            }
        }
        public virtual GameObject MoneyContainer
        {
            get
            {
                if (!_moneyContainer)
                {
                    _moneyContainer = this.gameObject.transform.Find("Shop Sections/Money Section/Money View/Viewport/Content").gameObject;
                }
                return _moneyContainer;
            }
        }
        public ReadOnlyCollection<ShopItem> ShopItems
        {
            get
            {
                return new ReadOnlyCollection<ShopItem>(_items);
            }
        }

        public void UpdateName(string name)
        {
            this._name = name;
            this.Title.text = name;
        }

        private ShopItem AddItem(Purchasable item, PurchaseLimit purchaseLimit, bool update)
        {
            ShopItem shopItem = CreateItem(item, purchaseLimit);
            shopItem.Purchasable = item;
            _items.Add(shopItem);

            if (update)
            {
                UpdateItems();
            }

            return shopItem;
        }

        public ShopItem AddItem(Purchasable item, PurchaseLimit purchaseLimit)
        {
            return AddItem(item, purchaseLimit, true);
        }

        public ShopItem AddItem(Purchasable item)
        {
            return AddItem(item, new PurchaseLimit(0, 0), true);
        }

        public ShopItem[] AddItems(Purchasable[] items)
        {
            ShopItem[] shopItems = items.Select(item => AddItem(item, new PurchaseLimit(0, 0), false)).ToArray();
            this.ExecuteAfterFrames(2, UpdateItems);
            return shopItems;
        }

        public ShopItem[] AddItems(Purchasable[] items, PurchaseLimit purchaseLimit)
        {
            ShopItem[] shopItems = items.Select(item => AddItem(item, new PurchaseLimit(purchaseLimit), false)).ToArray();
            UpdateItems();
            return shopItems;
        }

        public ShopItem[] AddItems(Purchasable[] items, PurchaseLimit[] purchaseLimits)
        {
            if (items.Length != purchaseLimits.Length)
            {
                throw new ArgumentException("'Shop::AddItems(Purchasable[] items, PurchaseLimit[] purchaseLimits)' expects 2 arrays of equal length.");
            }

            List<ShopItem> shopItems = new List<ShopItem>();

            for (int i = 1; i < items.Length; i++)
            {
                shopItems.Add(AddItem(items[i], new PurchaseLimit(purchaseLimits[i]), false));
            }

            UpdateItems();

            return shopItems.ToArray();
        }

        public void RemoveItem(ShopItem item)
        {
            _items.Remove(item);
            UnityEngine.GameObject.Destroy(item.gameObject);
            UpdateItems();
        }

        public void RemoveAllItems()
        {
            var shopItems = ShopItems.ToArray();
            foreach (var item in shopItems)
            {
                UnityEngine.GameObject.Destroy(item.gameObject);
            }
            _items.Clear();
            UpdateItems();
        }

        private ShopItem CreateItem(Purchasable item, PurchaseLimit purchaseLimit)
        {
            var itemObj = Instantiate<GameObject>(ShopManager.instance.shopItemTemplate, ItemContainer.transform);
            itemObj.GetComponent<RectTransform>().localScale = Vector3.one;
            itemObj.AddComponent<ButtonInteraction>();

            var shopItem = itemObj.AddComponent<ShopItem>();
            shopItem.Purchasable = item;
            shopItem.PurchaseLimit = purchaseLimit;

            item.CreateItem(shopItem.ItemContainer).GetComponent<RectTransform>().localScale = Vector3.one;

            foreach (var cost in shopItem.Purchasable.Cost)
            {
                CreateCostItem(shopItem.CostContainer, cost.Key, cost.Value).GetComponent<RectTransform>().localScale = Vector3.one;
            }

            return shopItem;
        }

        private GameObject CreateCostItem(GameObject parent, string currency, int amount)
        {
            var costObj = Instantiate<GameObject>(ShopManager.instance.costObjectTemplate, parent.transform);
            costObj.GetComponent<RectTransform>().localScale = Vector3.one;

            var costItem = costObj.AddComponent<CostItem>();
            costItem.Text.text = $"{amount}";
            CurrencyManager.instance.CurrencyImageAction(currency)(costItem.Image);

            return costObj;
        }

        public void Show(Player player)
        {
            currentPlayer = player;
            UpdateMoney();
            this.gameObject.SetActive(true);
        }

        private void UpdateMoney()
        {
            foreach (Transform child in MoneyContainer.transform)
            {
                UnityEngine.GameObject.Destroy(child.gameObject);
            }

            if (currentPlayer)
            {
                foreach (var money in currentPlayer.GetAdditionalData().bankAccount.Money)
                {
                    CreateCostItem(this.MoneyContainer, money.Key, money.Value);
                }
            }
        }

        public void Hide()
        {
            currentPlayer = null;
            currentPurchase = null;
            this.gameObject.SetActive(false);
        }

        private void UpdateItems()
        {
            var tagItems = this.GetComponentsInChildren<TagItem>();
            string[] existingTags = tagItems.Select(tag => tag.tag.name).ToArray();
            List<string> itemTags = new List<string>();
            
            foreach (var item in ShopItems)
            {
                itemTags.AddRange(item.Purchasable.Tags.Select(tag => tag.name));
            }

            string[] ghostTags = existingTags.Where(tag => (!itemTags.Contains(tag))).ToArray();

            itemTags = itemTags.Distinct().Where(tag => (!existingTags.Contains(tag))).ToList();

            foreach (var item in itemTags)
            {
                var tagObj = Instantiate(ShopManager.instance.tagObjectTemplate, this.TagContainer.transform);
                tagObj.GetComponent<RectTransform>().localScale = Vector3.one;
                tagObj.GetComponentInChildren<TextMeshProUGUI>().text = item;
                tagObj.AddComponent<ButtonInteraction>();

                var tagItem = tagObj.AddComponent<TagItem>();
                tagItem.tag = new Tag(item);
            }

            foreach (var tagItem in tagItems.Where(item => ghostTags.Contains(item.tag.name)))
            {
                UnityEngine.GameObject.Destroy(tagItem.gameObject);
            }

            tagItems = this.GetComponentsInChildren<TagItem>();

            UpdateFilters();
        }

        internal void UpdateFilters()
        {
            var tagItems = this.GetComponentsInChildren<TagItem>();

            string[] excludedTags = tagItems.Where(tagItem => tagItem.FilterState == FilterState.Excluded).Select(tagItem => tagItem.tag).ToArray().Select(tag => tag.name).ToArray();
            string[] requiredTags = tagItems.Where(tagItem => tagItem.FilterState == FilterState.Required).Select(tagItem => tagItem.tag).ToArray().Select(tag => tag.name).ToArray();

            ShopItem[] validItems = _items.Where(item => (!(excludedTags.Intersect(item.Purchasable.Tags.Select(tag=> tag.name).ToArray()).ToArray().Length > 0)) && (requiredTags.Intersect(item.Purchasable.Tags.Select(tag => tag.name).ToArray()).ToArray().Length == requiredTags.Length)).ToArray();
            
            if (Filter.text.Trim().Length > 0)
            {
                validItems = validItems.Where(item => 
                {
                    return item.gameObject.GetComponentsInChildren<TextMeshProUGUI>().Select(tmp => tmp.text.ToLower()).Any(text => text.Contains(Filter.text.Trim().ToLower()));
                }).ToArray();
            }

            foreach (var item in this.ShopItems)
            {
                item.gameObject.SetActive(validItems.Contains(item));
            }
        }

        private void ClearFilters()
        {
            var tagItems = this.GetComponentsInChildren<TagItem>();

            foreach (var item in tagItems)
            {
                item.SetState(FilterState.Allowed);
            }
            UpdateFilters();
        }

        internal void OnItemClicked(ShopItem item)
        {
            ClearPurchaseArea();

            PurchaseNameText.text = item.Purchasable.Name;

            foreach (var cost in item.Purchasable.Cost)
            {
                var costItem = CreateCostItem(PurchaseCostContainer, cost.Key, cost.Value).GetComponent<CostItem>();

                if (currentPlayer && currentPlayer.GetAdditionalData().bankAccount.Money.TryGetValue(cost.Key, out var playerMoney))
                {
                    if (playerMoney < cost.Value)
                    {
                        costItem.Text.color = new Color32(235, 10, 10, 255);
                    }
                    else
                    {
                        costItem.Text.color = new Color32(225, 225, 225, 255);
                    }
                }
                else
                {
                    costItem.Text.color = new Color32(235, 10, 10, 255);
                }
            }

            currentPurchase = item;
        }

        public void ClearPurchaseArea()
        {
            PurchaseNameText.text = "";

            foreach (Transform child in PurchaseCostContainer.transform)
            {
                UnityEngine.GameObject.Destroy(child.gameObject);
            }
        }

        private void OnPurchaseClicked()
        {
            OnPurchaseClicked(currentPlayer);
        }

        internal void OnPurchaseClicked(Player player)
        {
            if (currentPurchase != null)
            {
                if (player.GetAdditionalData().bankAccount.Withdraw(currentPurchase.Purchasable.Cost))
                {
                    currentPurchase.OnPurchase(player);
                    UpdateMoney();
                    currentPurchase = null;
                    ClearPurchaseArea();
                }
            }
        }

        private void Start()
        {
            Filter.onValueChanged.AddListener((str)=> { UpdateFilters(); });
            var interact = PurchaseButton.gameObject.AddComponent<ButtonInteraction>();
            interact.mouseClick.AddListener(OnPurchaseClicked);
        }
    }
}
