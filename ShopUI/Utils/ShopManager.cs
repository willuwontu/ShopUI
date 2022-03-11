using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;
using TMPro;

namespace ItemShops.Utils
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager instance;

        internal List<Shop> _shops = new List<Shop>();

        public ReadOnlyCollection<Shop> Shops { get { return new ReadOnlyCollection<Shop>(this._shops); } }

        internal GameObject shopCanvas;

        private GameObject shopTemplate;
        internal GameObject shopItemTemplate;
        internal GameObject tagObjectTemplate;
        internal GameObject costObjectTemplate;
        internal GameObject cardContainerTemplate;
        internal Sprite checkmark;
        internal Sprite xmark;

        private void Awake()
        {
            if (instance != null)
            {
                UnityEngine.GameObject.DestroyImmediate(this);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            shopCanvas = Instantiate(ItemShops.instance.assets.LoadAsset<GameObject>("ShopCanvas"));
            DontDestroyOnLoad(shopCanvas);

            RectTransform rect = shopCanvas.GetOrAddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            shopCanvas.GetComponent<Canvas>().worldCamera = Camera.current;

            shopTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Shop");
            tagObjectTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Tag Object");
            costObjectTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Cost Object");
            shopItemTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Shop Item");
            cardContainerTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Card Container");
            checkmark = ItemShops.instance.assets.LoadAsset<Sprite>("checkmark");
            xmark = ItemShops.instance.assets.LoadAsset<Sprite>("xmark");
        }

        public Shop CreateShop(string name)
        {
            var shopObj = Instantiate(shopTemplate, shopCanvas.transform);
            shopObj.GetOrAddComponent<RectTransform>().localScale = Vector3.one;
            var shop = shopObj.AddComponent<Shop>();
            shop.UpdateName(name);

            _shops.Add(shop);

            shop.Hide();

            return shop;
        }
    }
}
