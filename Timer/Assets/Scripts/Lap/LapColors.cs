using UnityEngine;

namespace Timer.UI.Lap
{
    public class LapColors : MonoBehaviour
    {
        [SerializeField]
        private Color[] LapColorsPack;
        public static Color[] LapColor;
        private static int LapColorCounter;
        private void Start()
        {
            LapColor = new Color[LapColorsPack.Length];
            for (int i = 0; i < LapColor.Length; i++)
            {
                LapColor[i] = LapColorsPack[i];
            }
        }
        public static Color GetColor()
        {
            return LapColor[LapColorCounter];

        }
        public static void LapCounter()
        {
            if (LapColorCounter < LapColor.Length-1)
            {
                LapColorCounter++;
            }
            else LapColorCounter = 0;
        }
    }
}
