using UnityEngine;

namespace Timer
{
    public class TimerLoader : MonoBehaviour
    {
        // This timer was created for My another project,
        // where I have different timers for many examines where there are a lot of scenes.
        // In My project are scene manager, where I have all of scenes with correct timer ScriptableObjects.
        // This is why here is TimerLoader and timer with ScriptableObject.

        [SerializeField]
        private TimerSO tempTimer;
        public static TimerSO Timer;
        private void Awake()
        {
            Timer = tempTimer;
            Timer.ResetAllTimer();
        }
    }
}
