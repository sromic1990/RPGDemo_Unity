using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGDemo.Scripts.CameraRelated
{
    public class CameraRaycaster : MonoBehaviour 
    {
        public Layer[] layerPriorities = { Layer.Enemy, Layer.Walkable };

        [SerializeField]
        float distanceToBackGround = 100f;
        Camera viewCamera;

        RaycastHit raycastHit;
        public RaycastHit Hit
        {
            get { return raycastHit; }
        }

        Layer layerHit;
        public Layer CurrentLayerHit
        {
            get { return layerHit; }
            set
            {
                Layer layerhitTemp = layerHit;
                layerHit = value;
                if(layerhitTemp != layerHit)
                {
                    if(LayersChangeEvent != null)
                    {
                        LayersChangeEvent(layerHit);
                    }
                }
            }
        }

        public delegate void OnLayerChange(Layer layer);
        public event OnLayerChange LayersChangeEvent;

        // Use this for initialization
        void Start () 
        {
            viewCamera = Camera.main;    
        }
        
        // Update is called once per frame
        void Update () 
        {
            //look for and return priority layer hi
            foreach(Layer layer in layerPriorities)
            {
                var hit = RaycastForLayer(layer);
                if(hit.HasValue)
                {
                    raycastHit = hit.Value;
                    CurrentLayerHit = layer;
                    return;
                }
            }

            //Otherwise return Background hit
            raycastHit.distance = distanceToBackGround;
            CurrentLayerHit = Layer.RaycastEndStop;
        }

        RaycastHit? RaycastForLayer(Layer layer)
        {
            int layerMask = 1 << (int)layer;
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit, distanceToBackGround, layerMask);
            if(hasHit)
            {
                return hit;
            }
            return null;
        }
    }
    
}