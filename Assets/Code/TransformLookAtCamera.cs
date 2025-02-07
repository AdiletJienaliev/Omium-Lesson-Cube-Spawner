using System;
using UnityEngine;

namespace Cube
{
    public class TransformLookAtCamera : MonoBehaviour
    {
        private Camera cam; 
        private void Awake( ) =>     cam = Camera.main;

        private void Update()
        {
            transform.LookAt(cam.transform.position);
        }
    }
}