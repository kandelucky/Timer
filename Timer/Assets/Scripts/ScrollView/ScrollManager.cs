using UnityEngine;
using UnityEngine.UI;

namespace Timer.UI.Scroll
{
    public class ScrollManager : MonoBehaviour
    {
        public enum DataValue { Hours, Minutes, Seconds }

        [SerializeField]
        private DataValue scrollerData;
        [SerializeField]
        private Transform contentTransform;
        [SerializeField]
        private int elementsAmount;
        [SerializeField]
        private ScrollerElement scrollElement;
        [SerializeField]
        private float scrollAligmentSpeed;
        [SerializeField]
        private AudioSource tickSound;

        private ScrollRect scrollRect;
        private RectTransform scrollContent;

        private bool scrollIsActive = false;
        private float[] allElementsHeights;
        private float elementHeight;
        private int elementNumber = 0;
        private int savedNumber;
        private bool IsAllign = false;
        private Vector2 contentAlignedPos;
        private bool isUpDir;
        private float scrollPositionY;

        private bool ContentPosEndPoint()
        {
            if (scrollContent.anchoredPosition.y <= 0 || scrollContent.anchoredPosition.y >= scrollContent.sizeDelta.y)

                return true;
            else
                return false;
        }

        private void Start()
        {
            Set();
            SpawnScrollElements();
            SetHeights();
        }
        private void Set()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollContent = scrollRect.content;
            contentTransform = scrollContent.transform;

            isUpDir = false;
            scrollIsActive = false;
            savedNumber = 0;
        }
        private void SpawnScrollElements()
        {
            for (int i = 0; i < elementsAmount; i++)
            {
                ScrollerElement tempElement = Instantiate(scrollElement, contentTransform);
                tempElement.SetText(i);
            }
        }
        void SetHeights()
        {
            allElementsHeights = new float[contentTransform.childCount];

            var rectTransform = scrollElement.gameObject.GetComponent<RectTransform>();
            elementHeight = rectTransform.sizeDelta.y;

            for (int i = 0; i < allElementsHeights.Length; i++)
            {
                allElementsHeights[i] = elementHeight * i;
            }
        }
        private void Update()
        {
            if (scrollIsActive)
            {
                CheckScrollDirection();
                CheckElementNumber();
            }
            else if (IsAllign)
            {
                SetScrollerPos();
            }
        }
        private void CheckScrollDirection()
        {
            Vector2 velocityChecker = scrollRect.velocity;

            if (velocityChecker.y == 0 || ContentPosEndPoint())
            {
                scrollIsActive = false;
                AlignElementsPos(0);
            }
            else if (velocityChecker.y > 0)
            {
                if (!isUpDir) isUpDir = true;
            }
            else if (velocityChecker.y < 0)
            {
                if (isUpDir) isUpDir = false;
            }
        }
        private void CheckElementNumber()
        {
            scrollPositionY = scrollContent.anchoredPosition.y;
            if (isUpDir)
            {
                if (scrollPositionY > allElementsHeights[elementNumber] + (elementHeight / 2))
                {
                    if (elementNumber < allElementsHeights.Length)
                    {
                        elementNumber++;
                        if (tickSound) tickSound.Play();
                    }
                }
            }
            else
            {
                if (scrollPositionY < allElementsHeights[elementNumber] - (elementHeight / 2))
                {
                    if (elementNumber > 0)
                    {
                        if (tickSound) tickSound.Play();
                        elementNumber--;
                    }
                }
            }
        }
        private void SetScrollerPos()
        {
            if (scrollIsActive) return;
            else
            {
                float step = scrollAligmentSpeed * Time.deltaTime*5;
                scrollContent.anchoredPosition = Vector2.MoveTowards(scrollContent.anchoredPosition, contentAlignedPos, step);
                if (scrollContent.anchoredPosition == contentAlignedPos)
                {
                    if (IsAllign) IsAllign = false;
                    SetTimerData();
                    if (tickSound) tickSound.Play();
                }
            }
        }
        public void AlignElementsPos(int elementNum)
        {
            if (elementNum > 0 && elementNum < allElementsHeights.Length) elementNumber = elementNum;
            else if (elementNum > 0 && elementNum >= allElementsHeights.Length) elementNumber = allElementsHeights.Length-1;
            savedNumber = elementNumber;
            contentAlignedPos = new Vector2(0, allElementsHeights[savedNumber]);
            if (contentAlignedPos != scrollContent.anchoredPosition)
            {
                IsAllign = true;
            }
            else SetTimerData();
        }
        private void SetTimerData()
        {
            switch (scrollerData)
            {
                case DataValue.Hours:
                    SetTimerTimes.SetHours(savedNumber);
                    break;
                case DataValue.Minutes:
                    SetTimerTimes.SetMinutes(savedNumber);
                    break;
                case DataValue.Seconds:
                    SetTimerTimes.SetSeconds(savedNumber);
                    break;
            }
        }
        public void ActivateScroll()
        {
            if (scrollRect.velocity.y != 0 && !ContentPosEndPoint())
            {
                scrollIsActive = true;
            }
        }
        public void ResetScroll()
        {
            Vector2 resetedPos = new Vector2(0, 0);
            if (scrollContent && scrollContent.anchoredPosition.y > 0) scrollContent.anchoredPosition = resetedPos;
        }
    }
}
