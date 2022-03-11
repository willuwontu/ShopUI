using System;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI;
using ItemShops.Monobehaviours;

namespace ItemShops.Utils
{
    class TagItem : MonoBehaviour
    {
        public Tag tag;

        ButtonInteraction interact;
        Shop shop;
        Image stateImage;
        FilterState _currentState = FilterState.Allowed;
        public FilterState FilterState => _currentState;

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

            stateImage = this.transform.Find("State Image").gameObject.GetComponent<Image>();
            stateImage.color = Color.clear;
        }

        private void OnDestroy()
        {

        }

        private void OnClick()
        {
            switch (_currentState)
            {
                case FilterState.Allowed:
                    _currentState = FilterState.Required;
                    stateImage.color = new Color32(0, 255, 10, 176);
                    stateImage.sprite = ShopManager.instance.checkmark;
                    shop.UpdateFilters();
                    break;
                case FilterState.Required:
                    _currentState = FilterState.Excluded;
                    stateImage.color = new Color32(255, 31, 0, 176);
                    stateImage.sprite = ShopManager.instance.xmark;
                    shop.UpdateFilters();
                    break;
                case FilterState.Excluded:
                    _currentState = FilterState.Allowed;
                    stateImage.color = Color.clear;
                    shop.UpdateFilters();
                    break;
                default:
                    break;
            }
        }

        internal void SetState(FilterState state)
        {
            switch (state)
            {
                case FilterState.Allowed:
                    _currentState = FilterState.Required;
                    stateImage.color = new Color32(0, 255, 10, 176);
                    stateImage.sprite = ShopManager.instance.checkmark;
                    break;
                case FilterState.Required:
                    _currentState = FilterState.Required;
                    stateImage.color = new Color32(0, 255, 10, 176);
                    stateImage.sprite = ShopManager.instance.checkmark;

                    break;
                case FilterState.Excluded:
                    _currentState = FilterState.Excluded;
                    stateImage.color = new Color32(255, 31, 0, 176);
                    stateImage.sprite = ShopManager.instance.xmark;
                    break;
                default:
                    break;
            }
        }
    }

    enum FilterState
    {
        Allowed,
        Required,
        Excluded
    }
}
