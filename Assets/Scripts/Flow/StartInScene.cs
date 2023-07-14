using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pit.Flow
{
    public class StartInScene : MonoBehaviour
    {
        [SerializeField]
        int _sceneNdx = 0;

        protected void Awake()
        {
            if (Pit.Framework.Game.Instance == null)
                SceneManager.LoadScene(_sceneNdx);  // note that for best effect, the script order on this should be hi
        }
    }
}
