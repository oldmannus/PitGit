using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit
{
    public class PT_StartInScene : MonoBehaviour
    {
        [SerializeField]
        int _sceneNdx = 0;


        protected void Awake()
        {
            if (PT_Game.Instance == null)
                SceneManager.LoadScene(_sceneNdx);  // note that for best effect, the script order on this should be hi
        }
    }
}
