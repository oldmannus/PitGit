//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//namespace Pit.Flow
//{

//    /// <summary>
//    /// WidgetMgr is to manage all sort of 3d UI object in the scene
//    /// Path lines, various indicators, destinations, zones, regions, etc
//    /// </summary>
//    public class UI_WidgetMgr : MonoBehaviour
//    {
//        [SerializeField]
//        UN_Pool _pathLinePool = null;

//        [SerializeField]
//        UN_Pool _pathDestinationPool = null;

//        Dictionary<int, Widget> _widgets = new Dictionary<int, Widget>();

//        // everything the widget mgr shows needs an id so other systems can update or change it
//        public class Widget
//        {
//            public GameObject Visual =  null;
//            public int Id = -1;
//        }

        
//        int _highestId = 66;

//        protected override void OnEnable()
//        {
//            base.OnEnable();
//        }

//        protected override void OnDisable()
//        { 
//            base.OnEnable();
//        }

//        Widget GetWidget( int widgetId, UN_Pool pool)
//        {
//            Widget widget = null;
//            if (widgetId < 0 || _widgets.TryGetValue(widgetId, out widget) == false)
//            {
//                if (widgetId >= 0)
//                {
//                    Dbg.LogError("Invalid widget id! Making new widget");
//                }

//                widget = new Widget();
//                widget.Id = ++_highestId;
//                widget.Visual = pool.Acquire();
//                _widgets.Add(widget.Id, widget);
//            }

//            return widget;
//        }


//        int ReleaseWidget( int widgetId, UN_Pool pool)
//        {
//            Widget widget = null;
//            if (_widgets.TryGetValue(widgetId, out widget) == false)
//            {
//                Dbg.LogError("Invalid widget id!");
//            }
//            else
//            {
//                _widgets.Remove(widgetId);
//                pool.Release(widget.Visual);
//            }
//            return -1;
//        }



//        public int ShowDestinationPoint( Vector3 position, int widgetId = -1)
//        {
//            Widget widget = GetWidget(widgetId, _pathDestinationPool);
//            widget.Visual.transform.position = position;
//            UN.SetActive(widget.Visual, true);
//            return widget.Id;
//        }

//        public int HideDestinationPoint(int widgetId)
//        {
//            return ReleaseWidget(widgetId, _pathDestinationPool);
//        }

//        public int ShowPath(List<Vector3> pathPositions, int widgetId = -1)
//        {
//            Widget widget = GetWidget(widgetId, _pathLinePool);

//            Dbg.Assert(widget != null);
//            UN_VisualLine line = widget.Visual.GetComponent<UN_VisualLine>();
//            if (Dbg.Assert(line != null))
//                return -1;

//            line.SetLine(pathPositions);
//            UN.SetActive(line.gameObject, true);
//            return widget.Id;
//        }

//        public int HidePath(int widgetId)
//        {
//            return ReleaseWidget(widgetId, _pathLinePool);
//        }
//    }

//}