using System;
using Timer.UI;
using Timer.UI.Scroll;
using UnityEngine;
using UnityEngine.Events;

namespace Timer
{
    public class SetTimerTimes : MonoBehaviour
    {
        private static TimerSO timer;
        [SerializeField]
        TimerScrollsController timerScrolls;
        [SerializeField]
        private TimerUI timerUI;
        public static event Action NewSetTimeIsDetected = delegate { };
        private void Start()
        {
            timer = TimerLoader.Timer;
        }
        public static void SetHours(int value)
        {
            timer.Hours = value;
            NewSetTimeIsDetected();
        }
        public static void SetMinutes(int value)
        {
            timer.Minutes = value;
            NewSetTimeIsDetected();
        }
        public static void SetSeconds(int value)
        {
            timer.Seconds = value;
            NewSetTimeIsDetected();
        }
        public void AddMoreMinutes(int value)
        {
            if (timerScrolls.IsButtonsCanSetScroll) // set minutes
            {
                timerScrolls.SetMinutesScroll(timer.Minutes + value);
            }
            else timer.AddMoreMinutes(value); // add minutes
            NewSetTimeIsDetected();
        }
        public void SubtractMinutes(int value)
        {
            if (timerScrolls.IsButtonsCanSetScroll) // set minutes
            {
                timerScrolls.SetMinutesScroll(timer.Minutes - value);
            }
            else timer.SubtractMinutes(value); // subtract Minutes
            NewSetTimeIsDetected();
        }
    }
}
