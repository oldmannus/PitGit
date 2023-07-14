using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit
{
    public class MT_ArenaSpawnPoint : MonoBehaviour
    {
        public int TeamNdx;         // really for just grouping teams together

        [NonSerialized]
        public bool Used = false;   // used on spawn to know if spot was used already
    }
}
