using UnityEngine;
using System;
using JLib.Utilities;
using JLib.Unity;
using JLib.Sim;
using JLib.Game;

namespace Pit
{
    public class MT_SetCameraToLookAtEvent : GameEvent
    {
        public Vector3 Target;
    }


    public class MT_PanningCamera : UN_Camera
    {
        [SerializeField]
        GameObject _gameCameraLookAtObj = null;

        [SerializeField]
        Camera _gameCamera = null;

        [SerializeField]
        float _minLookDistance = 1;

        [SerializeField]
        float _maxLookDistance = 1000;

        [SerializeField]
        float _panSpeed = 1;

        [SerializeField]
        float _zoomSpeed = 1;

        [SerializeField]
        float _rotSpeed = 1;


        [SerializeField]
        float _xRotAngle = 30.0f;


        float _yRotAngle = 0.0f;
        float _curLookRatio = 0.3f;


        public Camera MainCamera { get { return _gameCamera; } }

        protected void Awake()
        {
            if (GM_Game.Instance == null)
                enabled = false;
        }

        protected void Start()
        {
            Dbg.Assert(_gameCameraLookAtObj != null);
            Dbg.Assert(_gameCamera != null);
            UpdateCamera();
        }

        protected void OnEnable()
        {
            Events.AddGlobalListener<MT_SetCameraToLookAtEvent>(OnLookAt);
        }
        protected void OnDisable()
        {
            Events.RemoveGlobalListener<MT_SetCameraToLookAtEvent>(OnLookAt);
        }



        void OnLookAt(MT_SetCameraToLookAtEvent pos)
        {
            _gameCameraLookAtObj.transform.position = pos.Target;
        }


        void UpdateCamera()
        {
            Vector3 camLocalPos = Vector3.zero;
            camLocalPos.z = -MathExt.Lerp(_minLookDistance, _maxLookDistance, _curLookRatio);

            Vector3 angles = Vector3.zero;
            angles.x = _xRotAngle;
            angles.y = _yRotAngle;

            _gameCamera.transform.localPosition = camLocalPos;
            _gameCameraLookAtObj.transform.rotation = Quaternion.Euler(angles);
        }


        // TODO: remove inputs from here. Either parameterize, or pass in inputs
        private void Update()
        {

            if (Input.GetKey(KeyCode.A))
            {
                UpdatePanX(1 * _panSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                UpdatePanX(-1 * _panSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W))
            {
                UpdatePanZ(1 * _panSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                UpdatePanZ(-1 * _panSpeed * Time.deltaTime);
            }
            else if (Input.GetMouseButton(1))   // right click
            {
                // then we want to rotate around our look point, so do so
                float mouseMove = Input.GetAxis("Mouse X");
                if (Math.Abs(mouseMove) > float.Epsilon)
                    _yRotAngle += mouseMove * _rotSpeed;
            }

            _curLookRatio = Mathf.Clamp01(_curLookRatio - (_zoomSpeed * Input.GetAxis("Mouse ScrollWheel")));

            UpdateCamera();
        }

        void UpdatePanX(float panAmt)
        {
            Vector3 offset = _gameCameraLookAtObj.transform.TransformDirection(Vector3.left) * panAmt;
            // ### TODO : add bounds
            Vector3 newPosition = _gameCameraLookAtObj.transform.position + offset;
            _gameCameraLookAtObj.transform.position = newPosition;
        }
        void UpdatePanZ(float panAmt)
        {
            Transform baseTrans = _gameCameraLookAtObj.transform;
            float yRot = baseTrans.rotation.eulerAngles.y;
            baseTrans.rotation = Quaternion.Euler(0, yRot, 0);

            Vector3 offset = baseTrans.TransformDirection(Vector3.forward) * panAmt;

            // ### TODO : add bounds
            Vector3 newPosition = _gameCameraLookAtObj.transform.position + offset;
            _gameCameraLookAtObj.transform.position = newPosition;
        }

    }
}