using UnityEngine;
using JLib.Utilities;
using System.Collections;
using System.Collections.Generic;
using JLib.Unity;
using JLib.Game;

namespace JLib.Unity
{
    public class UN_CameraMgrReadyEvent : GameEvent
    {
        public UN_CameraMgr CamMgr;
    }

    public class UN_SetCameraToLookAtEvent : GameEvent
    {
        public Vector3 Target;
    }

    public interface UN_ICameraMgr
    {
    }

    public class UN_CameraMgr : MonoBehaviour, UN_ICameraMgr
    {
        [SerializeField]
        UN_Camera _uiCamera = null;

        List<UN_Camera>[] _cameras;

        protected UN_CameraMgr()
        {
            _cameras = new List<UN_Camera>[(int)UN_Camera.TagType.Count];
            for (int i = 0; i < (int)UN_Camera.TagType.Count; i++)
            {
                _cameras[i] = new List<UN_Camera>();
            }
        }

        int FindCameraNdx(UN_Camera cam)
        {
            // cameras might be null if we're in an auto-start situation
            if (cam == null || _cameras == null || _cameras[(int)cam.Tag] == null)
                return -1;
            else
            {
               return _cameras[(int)cam.Tag].FindIndex((x) => x == cam);
            }
        }

        public void Register(UN_Camera cam)
        {
            Dbg.Assert(cam != null);
            Dbg.Assert(_cameras != null);
            Dbg.Assert(FindCameraNdx(cam) < 0);
            _cameras[(int)cam.Tag].Add(cam);
        }

        public void Unregister(UN_Camera cam)
        {
            if (_cameras != null)
            {
                Dbg.Assert(FindCameraNdx(cam) >= 0);
                _cameras[(int)cam.Tag].Remove(cam);
            }
        }

        public UN_Camera FirstMainCamera()
        {
            if (_cameras[(int)UN_Camera.TagType.Main].Count <= 0)
                return null;
            else
                return _cameras[(int)UN_Camera.TagType.Main][0];
        }

        // shouldn't attach to, but can set reference point to align correctly
        public Transform GetUICameraTransform()
        {
            return _uiCamera.transform;
        }

    }
}
