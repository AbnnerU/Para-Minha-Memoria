using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.GridMap
{
    public class GridMapObjectDrag : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private MapGridContent content;

        [SerializeField] private CanvasGroup canvasGroup;

        public Action<Vector3, GridMapObjectDrag> OnStartDrag;

        public Action<Vector3, GridMapObjectDrag> OnEndDrag;

        private Vector3 defaltPosition;

        private Transform contentTranform;

        private void Start()
        {
            contentTranform = content.transform;

            defaltPosition = contentTranform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);

            OnStartDrag?.Invoke(pos, this);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);

            OnEndDrag?.Invoke(pos, this);
        }

        public Transform GetContentTransform()
        {
            return contentTranform;
        }

        public MapGridContent GetMapGridContent()
        {
            return content;
        }

        public void SetToDefaltPosition()
        {
            contentTranform.position = defaltPosition;
        }

        public void SetNewDefaltPostion(Vector3 newPosition)
        {
            defaltPosition = newPosition;
        }

        public void SetCurrentParentPositionAsDefalt()
        {
            defaltPosition = contentTranform.position;
        }
    }
}