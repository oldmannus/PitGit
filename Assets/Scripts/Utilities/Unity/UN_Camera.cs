//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit.Utilities
//{
//    public class UN_Camera : MonoBehaviour
//    {
//        public enum TagType
//        {
//            UIMain,           // usually one that handles 2d UI stuff
//            Main,           // usually one that receives input
//            Secondary,      // minimap, portals, etc.   
//            Count
//        }
//        [SerializeField]
//        TagType _tag = TagType.Main;

//        [SerializeField]
//        Camera _camera = null;
        
//        public TagType Tag { get { return _tag; } }

//        private void Start()
//        {
//            Game.Cameras.Register(this);
//        }
//        private void OnDestroy()
//        {
//            if (Game.IsLoaded)                   // avoid using global resources if on destroy, as we might be shutting down
//                Game.Cameras.Unregister(this);
//        }

//        public Ray ScreenPointToRay(Vector3 position)
//        {
//            return _camera.ScreenPointToRay(position);
//        }
//    }
//}
