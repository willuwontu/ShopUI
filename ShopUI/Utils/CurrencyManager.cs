using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemShops.Utils
{
    public class CurrencyManager : MonoBehaviour
    {
        private Dictionary<string, Action<Image>> CurrencyImageActions = new Dictionary<string, Action<Image>>();

        private Action<Image> defaultImageAction = (image) => 
            { 
                image.sprite = ItemShops.instance.assets.LoadAsset<Sprite>("dollar-sign");
                image.color = new Color32(118, 117, 35, 255);
            };

        public static CurrencyManager instance;

        public bool RegisterCurrencyIcon(string currency, Action<Image> imageAction)
        {
            if (CurrencyImageActions.ContainsKey(currency))
            {
                return false;
            }
            CurrencyImageActions.Add(currency, imageAction);

            return true;
        }

        public Action<Image> CurrencyImageAction(string currency)
        {
            Action<Image> imageAction = null;
            if (CurrencyImageActions.TryGetValue(currency, out var action))
            {
                imageAction = action;
            }
            else
            {
                imageAction = defaultImageAction;
            }
            return imageAction;
        }

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
