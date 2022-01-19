using System.Collections;
using UnityEngine;

namespace Timer
{
    [RequireComponent(typeof(SwipeChecker))]
    public class PanelsSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Animator panelsAnimator;
        private bool isSwitchingTime = false;

        private void Awake()
        {
            GetComponent<SwipeChecker>().SwipeIsDetected += StartAnimation;
        }
        private void StartAnimation(bool isRightDirection)
        {
            if (isSwitchingTime) return;
            else StartCoroutine(SwitchPanels(!isRightDirection));
        }
        private IEnumerator SwitchPanels(bool isRightDirection)
        {
            isSwitchingTime = true;
            if (isRightDirection) panelsAnimator.Play("GoToLapPanel");
            else panelsAnimator.Play("GoToUIPanel");
            yield return new WaitForSeconds(1f);
            isSwitchingTime = false;
        }
    }
}
