using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    public class ButtonSwitches : MonoBehaviour
    {
        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private Sprite normalButtonSprite;
        [SerializeField]
        private Sprite pressedButtonSprite;
        [SerializeField]
        private Transform[] buttonChilds;
        [SerializeField]
        private float eventDuration;
        private bool buttonIsPressed = false;
       
        public void ButtonEvent()
        {
            buttonIsPressed = !buttonIsPressed;
            StartCoroutine(StartSwitch());
        }
        
        private IEnumerator StartSwitch()
        {
            buttonImage.sprite = pressedButtonSprite;
            SetScale(getSmaller: true);
            yield return new WaitForSeconds(eventDuration);
            buttonImage.sprite = normalButtonSprite;
            SetScale(getSmaller: false);
        }
        private void SetScale(bool getSmaller)
        {
            if (getSmaller)
            {
                foreach (var child in buttonChilds)
                {
                    child.localScale = new Vector3(0.95f, 0.95f);
                }
            }
            else
            {
                foreach (var child in buttonChilds)
                {
                    child.localScale = new Vector3(1f, 1f);
                }
            }
        }
    }
}
