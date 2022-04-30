using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TMPCameraDebug : MonoBehaviour
    {
        public Transform realCamera;
        public Transform player;
        public Vector3 _camPlayer;

        private void Update()
        {
            _camPlayer = realCamera.forward;
        }
    }
}