using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemShops.Utils
{
    class CurrencyHandler : MonoBehaviour
    {
        Dictionary<string, Action<Image>> CurrencyImageActions = new Dictionary<string, Action<Image>>();

        private Action<Image> defaultImageAction = (image) => 
            { 
                image.sprite = ItemShops.instance.assets.LoadAsset<Sprite>("dollar-sign");
                image.color = new Color32(118, 117, 35, 255);
            };

        static CurrencyHandler instance;

        private void Awake()
        {
            if (instance != null)
            {
                UnityEngine.GameObject.Destroy(this);
            }
            instance = this;
        }
        private void Start()
        {

        }
    }
}
