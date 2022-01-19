using UnityEngine;

namespace Timer
{
    [CreateAssetMenu(fileName = "TimerSO", menuName = "Timer/TimerSO")]
    public class TimerSO : ScriptableObject
    {
        public bool TimerIsRunning { get; private set; }
        public bool TimerIsRepeating { get; private set; }
        public bool TimerIsPaused { get; private set; }
        public float LapDuration { get; set; }
        public float TimerTime { get; set; }
        private float RemainingTime()
        {
            return TimerTime + AdditionalSeconds - SubtractedSeconds;
        }

        [Header("Timer")]
        [SerializeField]
        [Range(0, 24)]
        private int hours;
        public int Hours
        {
            get { return hours; }
            set
            {
                if (value <= 0) hours = 0;
                else if (value > 24) hours = 24;
                else hours = value;
            }
        }
        [SerializeField]
        [Range(0, 60)]
        private int minutes;
        public int Minutes
        {
            get { return minutes; }
            set
            {
                if (value < 0) minutes = 0;
                else if (value > 60) minutes = 60;
                else minutes = value;
            }
        }
        [SerializeField]
        [Range(0, 60)]
        private int seconds;
        public int Seconds
        {
            get { return seconds; }
            set
            {
                if (value < 0) seconds = 0;
                else if (value > 60) seconds = 60;
                else seconds = value;
            }
        }

        [Header("Intervals (in seconds)")]
        [SerializeField]
        [Range(0, 10)]
        private float interval;
        public float Interval
        {
            get { return interval; }
            set
            {
                if (value < 0) interval = 0;
                else if (value > 10) interval = 10;
                else interval = value;
            }
        }
        [Header("Continuable")]
        [SerializeField]
        private bool isContinuable; // for many scenes
        public bool IsContinuable
        {
            get { return isContinuable; }
            set { isContinuable = value; }
        }

        private int additionalSeconds;
        public int AdditionalSeconds
        {
            get { return additionalSeconds; }
            set
            {
                if (value <= 0)
                {
                    additionalSeconds = 0;
                }
                else additionalSeconds = value;
            }
        }
        private int subtractedSeconds;
        public int SubtractedSeconds
        {
            get { return subtractedSeconds; }
            set
            {
                if (value <= 0)
                {
                    subtractedSeconds = 0;
                }
                else subtractedSeconds = value;
            }
        }
        private int additionalSecondsForTimeReset;
        private int subtractedSecondesForTimeReset;
        public bool TimerWillReseted { get; private set; }
        public float AllSeconds()
        {
            float hoursInSeconds;
            float minutesInSeconds;
            float all;

            if (Hours > 0) { hoursInSeconds = 3600 * (float)Hours; } else { hoursInSeconds = 0; }
            if (Minutes > 0) { minutesInSeconds = 60 * (float)Minutes; } else { minutesInSeconds = 0; }
            all = (hoursInSeconds + minutesInSeconds + Seconds + additionalSecondsForTimeReset) - subtractedSecondesForTimeReset;
            if (all >= 0) return all;
            else
            {
                TimerWillReseted = true;
                return 0;
            }

        }
        public float Percent(float timeToDisplay)
        {
            timeToDisplay += 1; // for timer times correct look
            return timeToDisplay / (AllSeconds() + 1) * 100;
        }

        public void ResetAllTimer()
        {
            Hours = Minutes = Seconds = AdditionalSeconds = SubtractedSeconds =
                additionalSecondsForTimeReset = subtractedSecondesForTimeReset = 0;
            TimerTime = LapDuration = 0;
            TimerIsRunning = TimerIsRepeating = TimerWillReseted = false;
        }
        public void ResetTimer()
        {
            LapDuration = 0;
            TimerTime = AllSeconds();
            TimerIsRunning = false;
        }
        public void StartTimer()
        {
            TimerIsPaused = false;
            TimerIsRunning = true;
        }
        public void StopTimer()
        {
            TimerIsRunning = false;
        }
        public void EndTimer()
        {
            AdditionalSeconds = 0;
            SubtractedSeconds = 0;
            TimerTime = 0;
            TimerIsRunning = false;
        }
        public void PauseTimer()
        {
            StopTimer();
            TimerIsPaused = true;
        }
        public void AddMoreMinutes(int additionalMin)
        {
            AdditionalSeconds += (additionalMin * 60);
            additionalSecondsForTimeReset += (additionalMin * 60);
            TimerTime = RemainingTime();
            AdditionalSeconds = 0;
            if (TimerTime > AllSeconds()) ResetAllSeconds();
        }
        public void SubtractMinutes(int subtractedMin)
        {
            SubtractedSeconds += (subtractedMin * 60);
            subtractedSecondesForTimeReset += (subtractedMin * 60);
            TimerTime = RemainingTime();
            SubtractedSeconds = 0;
            if (TimerTime <= 0) ResetAllSeconds();
        }
        private void ResetAllSeconds()
        {
            TimerTime = AllSeconds();
            additionalSecondsForTimeReset = subtractedSecondesForTimeReset = 0;
        }
        public void TimerRepeating(bool repeatTimer)
        {
            TimerIsRepeating = repeatTimer;
        }
        public void ResetRepeatingTimer()
        {
            AdditionalSeconds = SubtractedSeconds =
            additionalSecondsForTimeReset = subtractedSecondesForTimeReset = 0;
            TimerTime = AllSeconds();
        }
    }
}