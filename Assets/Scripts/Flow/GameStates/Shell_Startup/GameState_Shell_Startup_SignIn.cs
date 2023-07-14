using UnityEngine;
using System.Collections;
using Pit.Framework;
using UnityEngine.InputSystem;


namespace Pit.Flow
{
    public class GameState_Shell_Startup_SignIn : GameState
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerMgr.Instance.AddNewPlayer("Default Local Player");
        }

        public override void OnClick(InputValue value) 
        {
            if (value.isPressed == false)               // do on click up
                Flow.QueueState(_transitionsOut[0]);
        }


        protected override void Update()
        {
            base.Update();
                //### PJS TODO fix this - make it do on input enter
                
        }
    }
}