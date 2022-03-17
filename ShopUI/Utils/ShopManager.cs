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
        public static bool isLockingInput
        {
            get
            {
                if (ShopManager.instance == null || (!(ShopManager.instance.Shops.Count() > 0)))
                {
                    return false;
                }
                return ShopManager.instance.Shops.Values.Any((shop) => shop.gameObject.activeSelf);
            }
        }

        internal Dictionary<string, Shop> _shops = new Dictionary<string, Shop>();

        public ReadOnlyDictionary<string, Shop> Shops { get { return new ReadOnlyDictionary<string, Shop>(this._shops); } }

        public Shop CurrentShop { get; internal set; }

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
            this.shopCanvas = Instantiate(ItemShops.instance.assets.LoadAsset<GameObject>("ShopCanvas"));
            DontDestroyOnLoad(shopCanvas);

            RectTransform rect = shopCanvas.GetOrAddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            this.shopCanvas.GetComponent<Canvas>().worldCamera = Camera.current;

            this.shopTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Shop");
            this.tagObjectTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Tag Object");
            this.costObjectTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Cost Object");
            this.shopItemTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Shop Item");
            this.cardContainerTemplate = ItemShops.instance.assets.LoadAsset<GameObject>("Card Container");
            this.checkmark = ItemShops.instance.assets.LoadAsset<Sprite>("checkmark");
            this.xmark = ItemShops.instance.assets.LoadAsset<Sprite>("xmark");
        }

        public Shop CreateShop(string id)
        {
            if (this._shops.ContainsKey(id))
            {
                throw new ArgumentException("ShopManager::CreateShop(string id) expects a unique id.");
            }

            Shop shop = null;
            var shopObj = Instantiate(shopTemplate, shopCanvas.transform);
            shopObj.GetOrAddComponent<RectTransform>().localScale = Vector3.one;
            shop = shopObj.AddComponent<Shop>();
            shop.UpdateTitle(id);
            shop.ID = id;

            this._shops.Add(id, shop);

            shop.Hide();

            return shop;
        }

        public void RemoveShop(Shop shop)
        {
            this._shops.Remove(shop.ID);
            UnityEngine.GameObject.Destroy(shop.gameObject);
        }

        public void DestroyAllShops()
        {
            Shop[] shops = Shops.Values.ToArray();

            this._shops.Clear();

            foreach (var shop in shops)
            {
                UnityEngine.GameObject.Destroy(shop.gameObject);
            }
        }

        /// <summary>
        /// Closes all shops for all players on this client.
        /// </summary>
        public void HideAllShops()
        {
            foreach (var shop in Shops.Values)
            {
                shop.Hide();
            }
        }

        /// <summary>
        /// Closes all shops for a specified player.
        /// </summary>
        /// <param name="player">The player to hide all shops from.</param>
        public void HideAllShops(Player player)
        {
            foreach (var shop in Shops.Values)
            {
                if (shop.CurrentPlayer == player)
                {
                    shop.Hide();
                }
            }
        }
    }
}
