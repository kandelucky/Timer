using UnityEngine;
using UnityEngine.UI;
namespace Timer.UI.Lap
{
    public class LapElement : MonoBehaviour
    {
        [SerializeField]
        private Text IDText;
        [SerializeField]
        private Text lapDuration;
        [SerializeField]
        private Text timerValue;

        public void SetLapElementTexts(int id, string duration, string value, Color color)
        {
            IDText.text = id.ToString();
            lapDuration.text = duration;
            lapDuration.color = color;
            timerValue.text = value;
        }
    }
}
