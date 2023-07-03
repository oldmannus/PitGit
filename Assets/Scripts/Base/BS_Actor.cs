using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit
{
    public class BS_Actor : MonoBehaviour
    {
        public BS_ActionManagerComponent    Actions { get; private set; }
        public BS_HealthComponent           Health { get; private set; }
        public BS_InventoryActor            Inventory { get; private set; }


        // Start is called before the first frame update
        protected virtual void Awake()
        {
            Actions = GetComponentInChildren<BS_ActionManagerComponent>();
            Health = GetComponentInChildren<BS_HealthComponent>();
            Inventory = GetComponentInChildren<BS_InventoryActor>();

            BS_ActorComponent[] acomps = GetComponentsInChildren<BS_ActorComponent>();
            foreach (var comp in acomps)
            {
                comp.SetActor(this);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}