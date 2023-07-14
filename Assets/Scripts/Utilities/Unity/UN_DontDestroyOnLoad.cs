using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit.Utilities
{
    public class UN_DontDestroyOnLoad : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}