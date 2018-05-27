using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGDemo.Scripts.CameraRelated;

namespace RPGDemo.Scripts.CursorRelated
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour 
    {
        [SerializeField]
        Texture2D walkCursor = null;
        [SerializeField]
        Texture2D enemyCursor = null;
        [SerializeField]
        Texture2D unknownCursor = null;

        [SerializeField]
        Vector2 cursorHotSpot = Vector2.zero;

        CameraRaycaster rayCaster;
        
        // Use this for initialization
        void Awake () 
        {
            rayCaster = GetComponent<CameraRaycaster>();
        }

        private void Start()
        {
            rayCaster.LayersChangeEvent += OnLayerChange;
        }

        private void OnLayerChange(Layer layer)
        {
            switch (layer)
            {
                case Layer.Enemy:
                    SetCursor(enemyCursor);
                    break;

                case Layer.Walkable:
                    SetCursor(walkCursor);
                    break;

                default:
                    SetCursor(unknownCursor);
                    break;
            }
        }

        private void SetCursor(Texture2D cursor)
        {
            Cursor.SetCursor(cursor, cursorHotSpot, CursorMode.Auto);
        }
    }
    
}