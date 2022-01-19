using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    public class ToggleSwitches : MonoBehaviour
    {
        [SerializeField]
        public bool ToggleIsSwitchable { get; set; } = true;
        [SerializeField]
        private Image toggleImage;
        [SerializeField]
        private Sprite normalToggleSprite;
        [SerializeField]
        private Sprite pressedToggleSprite;
        [SerializeField]
        private Transform[] toggleChilds;
        [SerializeField]
        private Color toggleOnColor;
        [SerializeField]
        private Color toggleOffColor;
        [SerializeField]
        private float eventDurationIfNotSwitchable;

        private bool toggleIsPressed = false;
        public void ToggleEvent()
        {
             if (ToggleIsSwitchable) SwitchToggle();
             else StartCoroutine(StartForceSwitch());
        }
        private void SwitchToggle()
        {
            if (toggleIsPressed)
            {
                toggleImage.sprite = normalToggleSprite;
                if (toggleChilds.Length > 0)
                {
                    SetColor();
                    SetScale();
                }
            }
            else
            {
                toggleImage.sprite = pressedToggleSprite;
                if (toggleChilds.Length > 0)
                {
                    SetColor();
                    SetScale();
                }
            }
        }
        private void SetScale()
        {
            if (toggleIsPressed)
            {
                
                foreach (var child in toggleChilds)
                {
                    child.localScale = new Vector3(0.95f, 0.95f);
                }
            }
            else
            {
                foreach (var child in toggleChilds)
                {
                    child.localScale = new Vector3(1f, 1f);
                }
            }
        }
        private void SetColor()
        {
            if (toggleIsPressed)
            {
                foreach (var child in toggleChilds)
                {
                    if (child.GetComponent<Text>()) child.GetComponent<Text>().color = toggleOnColor;
                    else if (child.GetComponent<Image>()) child.GetComponent<Image>().color = toggleOnColor;
                }
            }
            else
            {
                foreach (var child in toggleChilds)
                {
                    if (child.GetComponent<Text>()) child.GetComponent<Text>().color = toggleOffColor;
                    else if (child.GetComponent<Image>()) child.GetComponent<Image>().color = toggleOffColor;
                }
            }
            toggleIsPressed = !toggleIsPressed;
        }
        private IEnumerator StartForceSwitch()
        {
            toggleImage.sprite = pressedToggleSprite;
            SetScale(getSmaller: true);
            yield return new WaitForSeconds(eventDurationIfNotSwitchable);
            toggleImage.sprite = normalToggleSprite;
            SetScale(getSmaller: false);
        }
        private void SetScale(bool getSmaller)
        {
            if (getSmaller)
            {
                foreach (var child in toggleChilds)
                {
                    child.localScale = new Vector3(0.95f, 0.95f);
                }
            }
            else
            {
                foreach (var child in toggleChilds)
                {
                    child.localScale = new Vector3(1f, 1f);
                }
            }
        }
    }
}
