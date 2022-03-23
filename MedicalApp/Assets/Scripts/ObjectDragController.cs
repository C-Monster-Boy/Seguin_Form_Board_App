using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    public class ObjectDragController : MonoBehaviour
    {
        public Vector2 screenPos;
        public Vector3 worldPos;
        public bool isDragActive;
        SeguinObject lastDragged;

        public delegate void DropEvent(SeguinObject currentObj, RaycastHit2D[] dropSiteItems);
        public static event DropEvent OnDropActionPerformed;

        public delegate void DragStart(SeguinObject seguinObject);
        public static event DragStart OnDragActionStarted;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            bool dropInput = Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended);
            if(isDragActive && dropInput)
            {
                Drop();
                return;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                screenPos = new Vector2(mousePos.x, mousePos.y);
            }
            else if(Input.touchCount > 0)
            {
                screenPos = Input.GetTouch(0).position;
            }
            else
            {
                return;
            }

            worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            if (isDragActive)
            {
                Drag();
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
                if(hit.collider != null)
                {
                    SeguinObject obj = hit.transform.gameObject.GetComponent<SeguinObject>();
                    if(obj != null && obj.isInteractable)
                    {
                        lastDragged = obj;
                        InitDrag();
                    }
                }
            }
        }

        private void InitDrag()
        {
            isDragActive = true;
            InvokeEvent_DragStart();          
        }

        private void Drag()
        {
            if (lastDragged != null)
                lastDragged.transform.position = new Vector2(worldPos.x, worldPos.y);
        }

        private void Drop()
        {
            isDragActive = false;

            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);
 
            InvokeEvent_Drop(lastDragged, hits);
            lastDragged = null;
        }

        private void InvokeEvent_Drop(SeguinObject currentObj, RaycastHit2D[] dropSiteItems)
        {
            OnDropActionPerformed?.Invoke(currentObj, dropSiteItems);
        }

        private void InvokeEvent_DragStart()
        {
            OnDragActionStarted?.Invoke(lastDragged);
        }

    }
}

