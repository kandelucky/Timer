using Timer.UI.Lap;
using UnityEngine;
using UnityEngine.Events;

namespace Timer
{
    public class SimpleTimer : MonoBehaviour
    {
        public enum TimerType { Time, Percent }

        private TimerSO timer;
        [SerializeField]
        LapController lapController;
        [SerializeField]
        private TimerType timerShowType;
        [SerializeField]
        private AudioSource timerIsOutSound;
        [SerializeField]
        private AudioSource RepeatingTimerIsOutSound;
        [SerializeField]
        private ToggleSwitches repeatToggle;

        private bool NewTimer()
        {
            if (timer.TimerTime == 0)
                return true;
            else
                return false;
        }
        private float intervalCounter;
        private bool TimerIsReadyForStart()
        {
            if (timer.AllSeconds() == 0)
            {
                return false;
            }
            else
                return true;
        }
        private bool timerIsPaused;
        private bool timerRepeating;
        private bool lapCounterIsOn = false;
        private float lapStartTime;

        public UnityEvent ResetTimerEvent;
        public UnityEvent TimerIsEndEvent;
        public UnityEvent UptadeTimerUIEvent;
        public UnityEvent TimerIsPauseEvent;
        public UnityEvent TimerIsStartEvent;

        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        private void Start()
        {
            timer = TimerLoader.Timer;
            // if first start
            ResetAllTimer();
        }
        private void Update()
        {
            if (timer.TimerIsRunning)
            {
                if (timer.TimerTime > 0)
                {
                    TimerUpdate();
                }
                else TimerEnd();
            }
        }
        private void TimerUpdate()
        {
            timer.TimerTime -= Time.deltaTime;

            intervalCounter += Time.deltaTime;
            if (intervalCounter >= timer.Interval)
            {
                if (lapCounterIsOn) timer.LapDuration += intervalCounter;
                UptadeTimerUIEvent.Invoke(); // UI event
                
                intervalCounter = 0;
            }
        }
        private void TimerEnd()
        {
            if (timer.TimerWillReseted) ResetAllTimer();
            else
            {
                timer.EndTimer();
                timerIsPaused = true;
                if (timer.TimerIsRepeating)
                {
                    if (RepeatingTimerIsOutSound != null) RepeatingTimerIsOutSound.Play();
                    RepeatTimer();
                }
                else
                {
                    if (timerIsOutSound != null) timerIsOutSound.Play();
                    TimerIsEndEvent.Invoke(); // UI event
                }
            }
        }
        public void StartTimer()
        {
            if (TimerIsReadyForStart())
            {
                if (NewTimer())
                {
                    timer.ResetTimer();
                    if (!timer.IsContinuable) timer.ResetRepeatingTimer(); // todo
                    timer.StartTimer();
                }
                else
                {
                    timer.StartTimer();
                    if (lapCounterIsOn) lapController.ResumeLap();
                }
                timerIsPaused = false;
                if (timerIsOutSound.isPlaying) timerIsOutSound.Stop();
                TimerIsStartEvent.Invoke();
            }
            else return;
        }
        public void PauseTimer()
        {
            timerIsPaused = true;
            timer.PauseTimer();
            if (lapCounterIsOn) lapController.PauseLap();
            TimerIsPauseEvent.Invoke(); // UI Event
        }
        public void TimerStartAndPause() // Button action
        {
            if (timerIsPaused)
            {
                StartTimer();
            }
            else
            {
                PauseTimer();
            }
        }
        private void RepeatTimer()
        {
            // to do simple counter from 3 to 0 ??
            timerIsPaused = true;
            timer.ResetTimer();
            TimerStartAndPause();
        }
        public void SetTimerAsRepeating()
        {
            //timer.TimerRepeating(!timerRepeating); test
            //timerRepeating = !timerRepeating;
            if (timerRepeating)
            {
                timerRepeating = false;
                timer.TimerRepeating(timerRepeating);
            }
            else
            {
                timerRepeating = true;
                timer.TimerRepeating(timerRepeating);
            }
        }
        public void OnOffLapCounter()
        {
            if (timer.AllSeconds() >= 100) // for lap circle correct view. Unfortunately I could not accurately calculate its length and rotation
            {
                if (lapCounterIsOn)
                {
                    lapController.AddLapInfo(lapStartTime, timer.TimerTime, timer.LapDuration);
                    timer.LapDuration = 0;
                    lapCounterIsOn = false;
                }
                else if (!timerIsPaused)
                {
                    if (lapController == null) lapController = GetComponent<LapController>();
                    lapStartTime = timer.TimerTime;
                    lapController.CreateLine();
                    lapCounterIsOn = true;
                }
            }
            else lapController.ResetLapToggle();
            
        }
        public void ResetAllTimer()
        {
            ResetRepeatToggle();
            timer.ResetAllTimer();
            timerIsPaused = true;
            timerRepeating = false;
            ResetTimerEvent.Invoke(); // UI Event
        }
        private void ResetRepeatToggle()
        {
            if (timer.TimerIsRepeating)
            {
                repeatToggle.ToggleEvent();
            }
        }
    }
}