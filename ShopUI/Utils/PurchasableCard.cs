using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;
using ItemShops.Monobehaviours;
using TMPro;

namespace ItemShops.Utils
{
    public class PurchasableCard : Purchasable
    {
        CardInfo _card;
        Dictionary<string, int> _cost;
        Tag[] _tags;
        string _name;

        public CardInfo Card
        {
            get
            {
                return _card;
            }
        }

        public override Dictionary<string, int> Cost
        {
            get
            {
                return _cost;
            }
        }
        public override Tag[] Tags
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
        public PurchasableCard(CardInfo card, Dictionary<string, int> cost, Tag[] tags)
        {
            this._card = card;
            this._cost = cost;
            this._tags = tags;
            this._name = card.cardName.ToUpper();
        }

        public PurchasableCard(CardInfo card, Dictionary<string, int> cost, Tag[] tags, string name)
        {
            this._card = card;
            this._cost = cost;
            this._tags = tags;
            this._name = name;
        }

        public override void OnPurchase(Player player, Purchasable item)
        {
            var card = ((PurchasableCard)item)._card;
            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 2f, 2f);

            ItemShops.instance.StartCoroutine(ShowCard(player, card));
        }

        public override bool CanPurchase(Player player)
        {
            return ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, this.Card);
        }

        public override GameObject CreateItem(GameObject parent)
        {
            GameObject container = null;
            GameObject holder = null;

            try
            {
                container = GameObject.Instantiate(ItemShops.instance.assets.LoadAsset<GameObject>("Card Container"));
            }
            catch (Exception)
            {

                UnityEngine.Debug.Log("Issue with creating the card container");
            }

            try
            {
                holder = container.transform.Find("Card Holder").gameObject;
            }
            catch (Exception)
            {

                UnityEngine.Debug.Log("Issue with getting the Card Holder");
                holder = container.transform.GetChild(0).gameObject;
            }

            GameObject cardObj = null;

            try
            {
                cardObj = GetCardVisuals(_card, holder);
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("Issue with getting card visuals");
            }

            container.transform.SetParent(parent.transform);

            return container;
        }

        private IEnumerator ShowCard(Player player, CardInfo card)
        {
            yield return ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, card, 2f);

            yield break;
        }

        private GameObject GetCardVisuals(CardInfo card, GameObject parent)
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

            try
            {
                var art = GameObject.Instantiate<GameObject>(card.cardArt, artHolder.transform);
                rect = art.GetOrAddComponent<RectTransform>();
                rect.localScale = Vector3.one;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.pivot = new Vector2(0.5f, 0.5f);
            }
            catch (NullReferenceException)
            {

            }
            catch (ArgumentNullException)
            {

            }
            catch (ArgumentException)
            {

            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }

            var particles = cardObj.GetComponentsInChildren<GeneralParticleSystem>().Select(system => system.gameObject);
            foreach (var particle in particles)
            {
                UnityEngine.GameObject.Destroy(particle);
            }

            var titleText = cardObj.transform.Find("Front/Text_Name").GetComponent<TextMeshProUGUI>();

            if ((titleText.color.r < 0.18f) && (titleText.color.g < 0.18f) && (titleText.color.b < 0.18f))
            {
                titleText.color = new Color32(200, 200, 200, 255);
            }

            return cardObj;
        }
    }
}
