using System;
using System.Collections;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;

namespace ItemShops.Utils
{
    class PurchasableCard : Purchasable
    {
        CardInfo _card;
        Dictionary<string, int> _cost;
        PurchaseLimit _purchaseLimit;
        List<Tag> _tags;
        string _name;

        public override PurchaseLimit PurchaseLimit
        {
            get
            {
                return _purchaseLimit;
            }
        }
        public override Dictionary<string, int> Cost
        {
            get
            {
                return _cost;
            }
        }
        public override List<Tag> Tags
        {
            get
            {
                return _tags;
            }
        }
        public override string Name
        {
            get
            {
                return _name;
            }
        }
        public PurchasableCard(CardInfo card, Dictionary<string, int> cost, PurchaseLimit purchaseLimit, List<Tag> tags)
        {
            this._card = card;
            this._cost = cost;
            this._purchaseLimit = purchaseLimit;
            this._tags = tags;
            this._name = card.cardName.ToUpper();
        }

        public override void OnPurchase(Player player, Purchasable item)
        {
            var card = ((PurchasableCard)item)._card;
            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 2f, 2f);

            ItemShops.instance.StartCoroutine(ShowCard(player, card));

            _timesPurchased++;
            if (_timesPlayerPurchased.TryGetValue(player, out int times))
            {
                times++;
            }
            else
            {
                _timesPlayerPurchased.Add(player, 1);
            }
        }

        public override GameObject CreateItem()
        {
            GameObject container = GameObject.Instantiate(ItemShops.instance.assets.LoadAsset<GameObject>("Card Container"));

            var holder = container.transform.Find("Card Holder").gameObject;

            GetCardVisuals(_card, holder);

            return container;
        }

        private IEnumerator ShowCard(Player player, CardInfo card)
        {
            yield return ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, card, 2f);

            yield break;
        }

        private void GetCardVisuals(CardInfo card, GameObject parent)
        {
            RectTransform rect = null;
            GameObject cardObj= null;

            try
            {
                cardObj = GameObject.Instantiate<GameObject>(card.gameObject.transform.GetChild(0).GetChild(0).gameObject, parent.gameObject.transform);
            }
            catch (Exception)
            {
                cardObj = GameObject.Instantiate<GameObject>(card.gameObject, parent.gameObject.transform);
                var temp = cardObj;
                cardObj = cardObj.GetComponentInChildren<Canvas>().gameObject;
                cardObj.transform.SetParent(parent.gameObject.transform);

                UnityEngine.GameObject.Destroy(temp);
            }
            cardObj.SetActive(true);

            rect = cardObj.GetOrAddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);

            var rarityThings = cardObj.GetComponentsInChildren<CardRarityColor>();

            foreach (var thing in rarityThings)
            {
                try
                {
                    thing.GetComponentInParent<CardVisuals>().toggleSelectionAction = (Action<bool>)Delegate.Remove(thing.GetComponentInParent<CardVisuals>().toggleSelectionAction, new Action<bool>(thing.Toggle));
                    UnityEngine.GameObject.Destroy(thing);
                }
                catch (Exception)
                {
                    UnityEngine.GameObject.Destroy(thing);
                }
            }

            var canvasGroups = cardObj.GetComponentsInChildren<CanvasGroup>();
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = 1;
            }

            UnityEngine.GameObject.Destroy(cardObj.transform.Find("Back").gameObject);

            var artHolder = cardObj.transform.Find("Front/Background/Art").gameObject;

            var art = GameObject.Instantiate<GameObject>(card.cardArt, artHolder.transform);
            rect = art.GetOrAddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);
        }
    }
}
