using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnboundLib;
using Sonigon;
using Sonigon.Internal;
using TMPro;

namespace ItemShops.Utils
{
    class CostItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Currency { get; internal set; }

        private Shop shop;

        private UnityEvent mouseEnter = new UnityEvent();
        private UnityEvent mouseExit = new UnityEvent();

        private Image _icon = null;
        private TextMeshProUGUI _text = null;

        public Image Image
        {
            get
            {
                if (!_icon)
                {
                    _icon = this.gameObject.transform.Find("Cost Icon").GetComponent<Image>();
                }
                return _icon;
            }
        }

        public TextMeshProUGUI Text
        {
            get
            {
                if (!_text)
                {
                    _text = this.gameObject.transform.Find("Cost Text").GetComponent<TextMeshProUGUI>();
                }
                return _text;
            }
        }
        private void Start()
        {
            shop = this.gameObject.GetComponentInParent<Shop>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseEnter?.Invoke();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            mouseExit?.Invoke();
        }
    }
}
