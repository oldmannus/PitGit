using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit
{
    public class BS_ActorComponent : MonoBehaviour
    {
        public BS_Actor Actor { get; private set; }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        public void SetActor(BS_Actor actor)
        {
            Actor = actor;
        }
    }
        
}