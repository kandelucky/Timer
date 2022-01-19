using System;
using Timer.UI.Scroll;
using UnityEngine;
using UnityEngine.UI;

namespace Timer.UI
{
    public class TimerUI : MonoBehaviour
    {
        TimerSO timer;
        public enum TimerType { Time, Percent }
        [Header("PanelSwitcher")]
        [SerializeField]
        private TimerScrollsController scrollHideController;
        [Header("Slider")]
        private Slider slider; // todo
        private Image sliderImage; // todo
        [SerializeField]
        private Image fillCircle;
        [SerializeField]
        private Color sliderMaxColor;
        [SerializeField]
        private Color sliderMinColor;
        [Header("Text")]
        [SerializeField]
        private Text timerText;
        [SerializeField]
        private TimerType timerShowType;
        [SerializeField]
        private string startText;
        [SerializeField]
        private Color textColor;
        [SerializeField]
        private string timerIsOutText;
        [SerializeField]
        private Text pauseText;
        [SerializeField]
        private Animator pauseTextAnim;
        [Header("Play/Pause Button")]
        [SerializeField]
        private GameObject startIcon;
        [SerializeField]
        private GameObject pauseIcon;
        private string TimerText(float timeToDisplay)
        {
            if (timerShowType == TimerType.Time)
            {
                float hours = Mathf.FloorToInt(timeToDisplay / 3600);
                float minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60);
                float seconds = Mathf.FloorToInt(timeToDisplay % 60);

                return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            }
            else
            {
                return string.Format("{0:00} %", timer.Percent(timer.TimerTime));
            }
        }
        
        private void Start()
        {
            SetTimerTimes.NewSetTimeIsDetected += UpdateUI;
        }
        public void SetTimerUI(bool newTimer)
        {
            if (newTimer)
            {
                if (timer.TimerWillReseted) ResetUI();
                else SetStartUI();
            }
            else UpdateProgressUI();
        }
        public void UpdateProgressUI() 
        {
            float timeToDisplay = timer.TimerTime + 1; // for timer times correct look
            if (timerText != null) timerText.text = TimerText(timeToDisplay);
            if (slider != null) SliderValue();
            if (fillCircle != null) FillValue();
        }
        public void UpdateUI()
        {
            if (timer.TimerIsRunning)
            {
                UpdateProgressUI();
            }
            else
            {
                if (timerText != null) timerText.text = TimerText(timer.AllSeconds());
            }
        }
        public void TimerIsStarted() // event listener
        {
            if (pauseText != null)
            {
                pauseText.text = "";
                pauseTextAnim.SetBool("Animate", false);
            }
            scrollHideController.IsScrollWillShowed = false;
            scrollHideController.ShowOrHideScrolls(timer.TimerWillReseted);
        }
        public void TimerIsPaused() // event listener
        {
            if (pauseText != null)
            {
                pauseText.text = "Timer Is Paused";
                pauseTextAnim.SetBool("Animate", true);
            }
        }
        public void TimerisEnd() // event listener
        {

            if (slider != null) slider.value = 1f; // for slider correct looks.
            if (fillCircle != null) fillCircle.fillAmount = 0;

            if (!timer.TimerIsRepeating)
            {
                if (timerText != null) timerText.text = timerIsOutText; // E.g. "Time is out!";
                if (timerShowType == TimerType.Percent) timerShowType = TimerType.Time;
                if (pauseText != null) pauseText.text = "Set Or Repear\nYour Timer!";
                ChangeStartButtonIcon();
                scrollHideController.IsScrollWillShowed = true;
                scrollHideController.ShowOrHideScrolls(timer.TimerWillReseted);
            }

        }
        public void ShowPercent()
        {
            if (timer.TimerIsRunning)
            {
                if (timerShowType == TimerType.Time) timerShowType = TimerType.Percent;
                else timerShowType = TimerType.Time;
            }
        }
        public void ChangeStartButtonIcon()
        {
            if (startIcon != null || pauseIcon != null)
                if (timer.TimerIsRunning)
                {
                    startIcon.SetActive(false);
                    pauseIcon.SetActive(true);
                }
                else
                {
                    startIcon.SetActive(true);
                    pauseIcon.SetActive(false);
                }
        }
        private void SetStartUI()
        {
            if (pauseText != null) pauseText.text = "Set Or Repeat\nYour Timer!";
            if (timerText != null)
            {
                if (timer.AllSeconds() == 0)
                {
                    timerText.text = startText; // E.g. "Are you ready?";
                }
                else timerText.text = TimerText(timer.AllSeconds());
            }
            ChangeStartButtonIcon();
            SetMaxProgressBars();
        }
        public void ResetUI() // event listener
        {
            if (timer == null) timer = TimerLoader.Timer;
            scrollHideController.IsScrollWillShowed = true;
            scrollHideController.ShowOrHideScrolls(timer.TimerWillReseted);
            if (pauseText != null) pauseText.text = "Please, Set Timer!";
            if (timerText != null) timerText.text = startText; // E.g. "Are you ready?";
            ChangeStartButtonIcon();
            SetMaxProgressBars();
        }
        public void ExitTimer()
        {
            Application.Quit();
            // to do options pop(dark mode, horisontal mode, many sounds for end, sounds in progress, vibrate on/off)
        }
        private void SetMaxProgressBars()
        {
            if (slider != null)
            {
                slider.value = slider.maxValue; // 100
                sliderImage.color = sliderMaxColor;
            }
            if (fillCircle != null)
            {
                fillCircle.fillAmount = 1;
                fillCircle.color = sliderMaxColor;
            }
        }
        private void FillValue()
        {
            fillCircle.fillAmount = timer.Percent(timer.TimerTime - 1) / 100;
            fillCircle.color = Color.Lerp(sliderMinColor, sliderMaxColor, fillCircle.fillAmount * 2);
        }
        private void SliderValue()
        {
            slider.value = timer.Percent(timer.TimerTime);
            sliderImage.color = Color.Lerp(sliderMinColor, sliderMaxColor, slider.value / 50);
        }
    }
}