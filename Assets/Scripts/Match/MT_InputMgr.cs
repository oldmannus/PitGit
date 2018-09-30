using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Sim;
using JLib.Utilities;


namespace Pit
{

    public class MT_InputMgr  : SM_InputMgr
    {
        public override void Awake()
        {
            MT_InputModeBase imb = new MT_InputModeBase();
            _inputModes.Add(new MT_InputModeBase());
            _inputModes.Add(new MT_InputModeSelectTerrainPoint());
            _inputModes.Add(new MT_InputModeSelectPath());
            _inputModes.Add(new MT_InputModeNone());
            _defaultMode = imb;

            // do after those are setup
            base.Awake();
        }

    }
}
