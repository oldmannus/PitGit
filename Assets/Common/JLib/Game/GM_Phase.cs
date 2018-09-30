using UnityEngine;
using System.Collections;

namespace JLib.Game
{
    public class GM_Phase : MonoBehaviour
    {
        public string SceneName = null;


        public virtual IEnumerator Enter(GM_Phase oldPhase) { yield return null; }
        public virtual IEnumerator Exit(GM_Phase newPhase) { yield return null; }
    }
}
