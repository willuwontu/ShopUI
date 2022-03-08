using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemShops.Utils
{
    class ShopManager : MonoBehaviour
    {
        public static ShopManager instance;

        internal List<Shop> _shops = new List<Shop>();

        public ReadOnlyCollection<Shop> Shops { get { return new ReadOnlyCollection<Shop>(this._shops); } }

        internal GameObject shopCanvas;

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

            RectTransform rect = shopCanvas.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            shopCanvas.GetComponent<Canvas>().worldCamera = Camera.current;
        }
    }
}
