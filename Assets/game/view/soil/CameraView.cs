using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.game.view.soil
{
    class CameraView : TerrariumElement
    {
        public Vector3 DefaultPosition;
        public bool IsOrthographic;

        private Camera _cam;

        void Start()
        {
            this._cam = GetComponent<Camera>();

            StartTransition(1, 0, 10);
        }

        void Update()
        {
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            _cam.transform.position = DefaultPosition;
            _cam.orthographic = IsOrthographic;
        }

        // returns how long the transition is gonna take in seconds
        public float StartTransition(float startValue, float targetValue, float speed)
        {
            StartCoroutine(TransitionEnumerator(startValue, targetValue, speed));

            return speed * 100;
        }

        private Material _transitionMaterial;
        private IEnumerator TransitionEnumerator(float startValue, float targetValue, float speed)
        {
            if (_transitionMaterial == null)
            {
                _transitionMaterial = GetComponent<SimpleBlit>().TransitionMaterial;
            }

            _transitionMaterial.SetFloat("_Cutoff", startValue);

            if (startValue > targetValue)
            {
                for (int i = 0; i < 100; i++)
                {
                    _transitionMaterial.SetFloat("_Cutoff", (startValue - targetValue) * ((100 - i) / 100f));
                    yield return new WaitForSeconds(0.1f / speed);
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    _transitionMaterial.SetFloat("_Cutoff", targetValue * (i / 100f));
                    yield return new WaitForSeconds(0.1f / speed);
                }
            }

            _transitionMaterial.SetFloat("_Cutoff", targetValue);
        }
    }
}
