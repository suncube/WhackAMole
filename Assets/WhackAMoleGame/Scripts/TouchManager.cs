using System;
using UnityEngine;

namespace WhackAMoleGame
{
    public class TouchManager : MonoBehaviour
    {
        public Camera Camera;
        public static TouchManager Runtime;

        void Awake()
        {
            Runtime = this;
        }
        
        void Update()
        {
            TouchProcessing();
        }

        void TouchProcessing()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            //mouse

            if (Input.GetMouseButtonDown(0))
                InputTouchBegin(Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
                InputTouchEnded();

            InputTouchMoved(Input.mousePosition);
#endif
#if !UNITY_STANDALONE && !UNITY_EDITOR
    //
    //touch
    //
        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);
            if (touchZero.phase == TouchPhase.Began)
            {
                InputTouchBegin(touchZero.position);
            }
            if (touchZero.phase == TouchPhase.Ended)
            {
                InputTouchEnded();
            }
            if (touchZero.phase == TouchPhase.Moved)
            {
                InputTouchMoved(touchZero.position);
            }
        }
#endif
        }

        public Action<GameObject> OnHitObject;
        private void InputTouchBegin(Vector2 point)
        {
            RaycastHit hit;
            var ray = Camera.ScreenPointToRay(point);

            if (!Physics.Raycast(ray, out hit)) return;

            if (OnHitObject != null)
                OnHitObject(hit.transform.gameObject);
        }

        private void InputTouchEnded()
        {

        }

        private void InputTouchMoved(Vector2 point)
        {
        }
    }
}