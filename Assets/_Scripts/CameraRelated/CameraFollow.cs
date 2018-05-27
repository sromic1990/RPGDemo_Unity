using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGDemo.Scripts.CameraRelated
{
    public class CameraFollow : MonoBehaviour 
    {
        GameObject player;
        Transform myTransform;
        
        // Use this for initialization
        void Start () 
        {
            player = GameObject.FindGameObjectWithTag("Player");
            myTransform = transform;
        }
        
        // Update is called once per frame
        void LateUpdate () 
        {
            myTransform.position = player.transform.position;
        }
    }
    
}