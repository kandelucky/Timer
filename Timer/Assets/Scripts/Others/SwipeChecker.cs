using System;
using UnityEngine;

namespace Timer
{
    public class SwipeChecker : MonoBehaviour
    {
        private enum SwipeDirections
        {
            Right,
            Left,
            None
        }
        private Touch theTouch;
        private Vector2 touchStartPosition, touchEndPosition;
        private bool isSwipe = false;
        private bool isClick = false;
        private Vector3 mousePos;
        private float swipeX = 0;
        [SerializeField]
        private float swipeCheckerlenght = 200;

        public event Action<bool> SwipeIsDetected = delegate { };

        private void Update()
        {
            if (!isSwipe)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (Input.touchCount > 0)
                    {
                        CheckTouchSwipe();
                    }
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    CheckMouseSwipe();
                }
            }
        }
        private void CheckTouchSwipe()
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Moved)
            {
                touchEndPosition = theTouch.position;
                swipeDirections();
                swipeX = touchEndPosition.x - touchStartPosition.x;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                isSwipe = false;
            }
        }
        private void CheckMouseSwipe()
        {
            if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                if (!isClick)
                {
                    swipeX = mousePos.x;
                    isClick = true;
                }
                swipeDirections();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetSwipeChecker();
            }
        }
        private SwipeDirections swipeDirections()
        {
            if (isClick) // if checked device is computer
            {
                if (mousePos.x > swipeX + swipeCheckerlenght)
                {
                    isSwipe = true;
                    SwipeIsDetected(true);
                    ResetSwipeChecker();
                    return SwipeDirections.Right;
                }
                else if (mousePos.x < swipeX - swipeCheckerlenght)
                {
                    isSwipe = true;
                    SwipeIsDetected(false);
                    ResetSwipeChecker();
                    return SwipeDirections.Left;
                }
                else return SwipeDirections.None;
            }
            else // todo check in mobile
            {
                if (swipeX > swipeCheckerlenght)
                {
                    isSwipe = true;
                    SwipeIsDetected(true);
                    ResetSwipeChecker();
                    return SwipeDirections.Right;
                }
                else if (swipeX < -swipeCheckerlenght)
                {
                    isSwipe = true;
                    SwipeIsDetected(false);
                    ResetSwipeChecker();
                    return SwipeDirections.Left;
                }
                else return SwipeDirections.None;
            }
        }
        private void ResetSwipeChecker()
        {
            isClick = isSwipe = false;
        }
    }
}
