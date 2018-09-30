using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLib.Unity
{
    public class UN_DontDestroyOnLoad : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}