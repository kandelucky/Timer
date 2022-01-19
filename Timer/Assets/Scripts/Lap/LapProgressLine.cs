using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    public class LapProgressLine : MonoBehaviour
    {
        private TimerSO timer;
        [SerializeField]
        private Image fillCircle;
        private float tempTime;

        public bool LapIsactive { get; set; }

        public float Percent(float timeToDisplay)
        {
            return timeToDisplay / (timer.AllSeconds()) * 360;
        }
        private void OnEnable()
        {
            timer = TimerLoader.Timer;
            fillCircle.fillAmount = 0;
            LapIsactive = true;
            
        }
        public void SetLapLine(Color color)
        {
            tempTime = (timer.Percent(timer.TimerTime-1) / 100) - 1;
            fillCircle.color = color;
            gameObject.transform.eulerAngles = new Vector3(
                                 gameObject.transform.eulerAngles.x,
                                 gameObject.transform.eulerAngles.y,
                                 gameObject.transform.eulerAngles.z + (Percent(timer.TimerTime-1)));
        }
        private void Update()
        {
            if (LapIsactive)
            {
                float tempF = ((timer.Percent(timer.TimerTime - 1)) / 100) - 1;
                tempF -= tempTime;
                fillCircle.fillAmount = -tempF;
            }
        }
    }
}
