using System.Collections.Generic;
using UnityEngine;

namespace Timer.UI.Lap
{
    public class LapController : MonoBehaviour
    {
        [System.Serializable]
        public struct LapInfo
        {
            public string StartTime;
            public string EndTime;
            public string Duration;
            public Color LapColor;
        }
        private List<LapInfo> lapInfoStruct = new List<LapInfo>();
        private LapInfo tempLapinfo = new LapInfo();
        [SerializeField]
        private Transform lapInfoSpawnTransform;
        [SerializeField]
        private LapElement lapElement;
        [SerializeField]
        private Transform lapLineSpawnTransform;
        [SerializeField]
        private LapProgressLine lapLine;
        [SerializeField]
        private ToggleSwitches lapToggle;
        private LapProgressLine tempLap;
        private string LapDurationtimeTextFormat(int value)
        {
            return "" + lapInfoStruct[value].StartTime + "\n" + lapInfoStruct[value].EndTime;
        }
        private string TimeToString(float timeToDisplay)
        {

            float hours = Mathf.FloorToInt(timeToDisplay / 3600);
            float minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        public void CreateLine()
        {
            LapProgressLine line = Instantiate(lapLine, lapLineSpawnTransform);
            line.SetLapLine(LapColors.GetColor());
            tempLap = line;
        }
        public void AddLapElement(int lapAmount)
        {
            LapElement lapGO = Instantiate(lapElement, lapInfoSpawnTransform);
            lapGO.SetLapElementTexts(lapAmount,
                lapInfoStruct[lapAmount - 1].Duration,
                LapDurationtimeTextFormat(lapAmount - 1),
                lapInfoStruct[lapAmount - 1].LapColor);
        }
        public void AddLapInfo(float startT, float endT, float duration)
        {
            tempLap.LapIsactive = false;
            tempLapinfo.StartTime = TimeToString(startT);
            tempLapinfo.EndTime = TimeToString(endT);
            tempLapinfo.Duration = TimeToString(duration);
            tempLapinfo.LapColor = LapColors.GetColor();
            LapColors.LapCounter();
            lapInfoStruct.Add(tempLapinfo);
            AddLapElement(lapInfoStruct.Count);
        }
        public void PauseLap()
        {
            tempLap.LapIsactive = false;
        }
        public void ResumeLap()
        {
            tempLap.LapIsactive = true;
        }
        public void ResetLapInfo()
        {
            if (lapLineSpawnTransform.childCount > 0)
            {
                ResetLapToggle();
                lapInfoStruct.Clear();
                for (int i = 0; i < lapLineSpawnTransform.childCount; i++)
                {
                    Destroy(lapLineSpawnTransform.GetChild(i).gameObject);
                    
                }
                for (int j = 0; j < lapInfoSpawnTransform.childCount; j++)
                {
                    Destroy(lapInfoSpawnTransform.GetChild(j).gameObject);
                }
            }
        }
        public void ResetLapToggle()
        {
            lapToggle.ToggleEvent();
        }
    }
}
