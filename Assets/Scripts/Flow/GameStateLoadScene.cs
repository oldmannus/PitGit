using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pit.Flow
{
    public class GameStateLoadScene : GameState
    {
        [Serializable]
        public class AutoLoadSceneAsset
        {
            public LoadSceneMode SceneMode;
            public bool LoadOnEnter = true;     // when we do onenter, load the scene. if false. wait for code
            public string SceneName = "";
        }

        [SerializeField] 
        protected List<AutoLoadSceneAsset> _scenes = new();

        protected override void OnDisable()
        {
            base.OnDisable();

            // ### What about duplicate scenes? TODO fix this
            //for (int i = 0; i < _scenes.Count; i++)
            //{
            //    if (_scenes[i].SceneMode == LoadSceneMode.Additive &&
            //        _scenes[i].LoadOnEnter)
            //    {
            //        SceneManager.UnloadSceneAsync(_scenes[i].SceneName); //TODO: good?
            //    }
            //}
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // if we have a non-additive scene to load, we need to do that. 
            AutoLoadSceneAsset nonAddScene = _scenes.Find(x => x.SceneMode == LoadSceneMode.Single && x.LoadOnEnter);
            if (nonAddScene != null)
            {
                if (!SceneManager.GetSceneByName(nonAddScene.SceneName).isLoaded)
                {
                    SceneManager.LoadScene(nonAddScene.SceneName);  // TODO make async, add ui screen?
                }
            }

            for (int i = 0; i < _scenes.Count; i++)
            {
                if (_scenes[i].SceneMode == LoadSceneMode.Additive &&
                    _scenes[i].LoadOnEnter)
                {
                    if (!SceneManager.GetSceneByName(_scenes[i].SceneName).isLoaded)
                    {
                        SceneManager.LoadScene(_scenes[i].SceneName, _scenes[i].SceneMode);
                    }
                }
            }

            TryToAutoSkipOut();
        }
    }
}