using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnboundLib;
using Sonigon;
using Sonigon.Internal;

namespace ItemShops.Monobehaviours
{
    public class ButtonInteraction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent mouseClick = new UnityEvent();
        public UnityEvent mouseEnter = new UnityEvent();
        public UnityEvent mouseExit = new UnityEvent();
        public Button button;

        private System.Random random = new System.Random();

        private void Start()
        {
            button = gameObject.GetComponent<Button>();

            mouseEnter.AddListener(OnEnter);
            mouseExit.AddListener(OnExit);
            mouseClick.AddListener(OnClick);
        }

        public void OnEnter()
        {
            if (button.interactable)
            {
                //source.PlayOneShot(ItemShops.instance.hover[random.Next(ItemShops.instance.hover.Count)]);
                SoundManager.Instance.Play(
                    ItemShops.instance.hoverSounds[random.Next(ItemShops.instance.hoverSounds.Count)],
                    base.transform,
                    new SoundParameterBase[] { new SoundParameterIntensity(Optionshandler.vol_Sfx * Optionshandler.vol_Master, UpdateMode.Once) }
                    );
            }
        }

        public void OnExit()
        {
            if (button.interactable)
            {
                SoundManager.Instance.Play(
                    ItemShops.instance.hoverSounds[random.Next(ItemShops.instance.hoverSounds.Count)],
                    base.transform,
                    new SoundParameterBase[] { new SoundParameterIntensity(Optionshandler.vol_Sfx * Optionshandler.vol_Master, UpdateMode.Once) }
                    );
            }
        }

        public void OnClick()
        {
            if (button.interactable)
            {
                SoundManager.Instance.Play(
                    ItemShops.instance.clickSounds[random.Next(ItemShops.instance.clickSounds.Count)],
                    base.transform,
                    new SoundParameterBase[] { new SoundParameterIntensity(Optionshandler.vol_Sfx * Optionshandler.vol_Master, UpdateMode.Once) }
                    );
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseEnter?.Invoke();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            mouseExit?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            mouseClick?.Invoke();
        }
    }
}
