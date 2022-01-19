using System.Collections;
using UnityEngine;

namespace Timer.UI.Scroll
{
    public class TimerScrollsController : MonoBehaviour
    {
        [SerializeField]
        private ScrollManager hoursScrollController;
        [SerializeField]
        private ScrollManager minutesScrollController;
        [SerializeField]
        private ScrollManager secondesScrollController;
        [SerializeField]
        private Animator scrollsAnimator;
        public bool IsButtonsCanSetScroll { get; private set; }
        public bool IsScrollWillShowed { get; set; }
        public void ResetScrolls()
        {
            hoursScrollController.ResetScroll();
            minutesScrollController.ResetScroll();
            secondesScrollController.ResetScroll();
        }
        public void SetMinutesScroll(int scrollPos)
        {
            minutesScrollController.AlignElementsPos(scrollPos);
        }
        public void ShowOrHideScrolls(bool isReseted)
        {
            if (isReseted) ResetScrolls();
            StartCoroutine(AnimateScrollsPanel());
        }
        private IEnumerator AnimateScrollsPanel()
        {
            yield return new WaitForSeconds(0.1f);
            if (IsScrollWillShowed)
            {
                IsButtonsCanSetScroll = true;
                scrollsAnimator.Play("ShowScrolls");
            }
            else
            {
                IsButtonsCanSetScroll = false;
                scrollsAnimator.Play("HideScrolls");
            }
        }
    }
}
