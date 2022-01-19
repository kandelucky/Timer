using UnityEngine;
using UnityEngine.UI;

namespace Timer.UI.Scroll
{
    public class ScrollerElement : MonoBehaviour
    {
        [SerializeField]
        private Text elementText;

        public void SetText(int value)
        {
            elementText.text = value.ToString("00");
        }
    }
}
