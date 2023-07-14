using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pit.Utilities;

namespace Pit.Sim
{
    public class SM_SpaceEvent : GameEvent
    {
        public SM_Space Space;
    }

    public class SM_SpaceStartedEvent : SM_SpaceEvent { }
    public class SM_SpaceDestroyedEvent : SM_SpaceEvent { }



    // this is a generic container for 'play spaces' i.e. levelsin the sim
    // Not all sims have playspaces, and some can have multiple
    public class SM_Space : MonoBehaviour
    {
        //public FBoundingBox Bounds;     // in world coordinates of game     

        protected virtual void Awake()
        {
            gameObject.transform.position = Vector3.zero;   // as we are the root for many spawned things
            gameObject.transform.rotation = Quaternion.identity;
        }
        protected virtual void Start()
        {
            Events.SendGlobal(new SM_SpaceStartedEvent() { Space = this });
        }

        protected virtual void OnDestroy()
        {
            Events.SendGlobal(new SM_SpaceDestroyedEvent() { Space = this });
        }

    }
}
