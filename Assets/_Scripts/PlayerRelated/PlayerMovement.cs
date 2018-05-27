using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPGDemo.Scripts.CameraRelated;

namespace RPGDemo.Scripts.PlayerRelated
{    
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour 
    {
        [SerializeField]
        float walkMoveStopRadius = 0.2f;
        [SerializeField]
        float attackMoveStopRadius = 5f;

        ThirdPersonCharacter thirdPersonCharacter;
        CameraRaycaster cameraRayCaster;
        Vector3 currentDestination;
        Vector3 clickPoint;
        
        bool isInDirectMode = false;

        [SerializeField]
        Layer currentLayer;

        void Start () 
        {
            cameraRayCaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            ResetClickTarget();
            cameraRayCaster.LayersChangeEvent += OnLayerChange;
        }

        private void OnLayerChange(Layer layer)
        {
            currentLayer = layer;
        }

        void FixedUpdate ()
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                isInDirectMode = !isInDirectMode;
                ResetClickTarget();
            }

            if(isInDirectMode)
            {
                ProcessKeyboardMovement();
            }
            else
            {
                ProcessMouseMovement();
            }

        }

        private void ProcessKeyboardMovement()
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward + h * Camera.main.transform.right;
            thirdPersonCharacter.Move(move, false, false);
        }

        private void ProcessMouseMovement()
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (Input.GetMouseButton(0))
            {
                clickPoint = cameraRayCaster.Hit.point;
                switch (currentLayer)
                {
                    case Layer.Walkable:
                        currentDestination = ShortenDestination(clickPoint, walkMoveStopRadius);
                        break;

                    case Layer.Enemy:
                        currentDestination = ShortenDestination(clickPoint, attackMoveStopRadius);
                        break;

                    default:
                        Debug.Log("SHOUDN'T BE HERE");
                        break;
                }
                Debug.Log("Cursor raycast hit " + currentLayer.ToString());
            }
            WalkToDestination();
        }

        private void WalkToDestination()
        {
            var playerToClickPoint = currentDestination - transform.position;
            if (playerToClickPoint.magnitude >= walkMoveStopRadius)
            {
                thirdPersonCharacter.Move(playerToClickPoint, false, false);
            }
            else
            {
                thirdPersonCharacter.Move(Vector3.zero, false, false);
            }
        }

        private void ResetClickTarget()
        {
            currentDestination = transform.position;
        }

        Vector3 ShortenDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, clickPoint);
            Gizmos.DrawSphere(currentDestination, 0.1f);
            Gizmos.DrawSphere(clickPoint, 0.15f);

            //Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
        }
    }
    
}