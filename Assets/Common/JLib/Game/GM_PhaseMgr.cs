using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using JLib.Utilities;
using JLib.Unity;

namespace JLib.Game
{

    public class GM_GamePhaseChangedEvent : GameEvent
    {
        public System.Type OldPhase;
        public System.Type NewPhase;
    }

    public class SceneChangedEvent : GameEvent
    {
        public string Name;
    }

    public class GM_PhaseMgr : MonoBehaviour
    {
        [SerializeField]
        List<GM_Phase> _phases = new List<GM_Phase>();
        GM_Phase _curPhase = null;
        GM_Phase _queuedPhase = null;
        GM_Phase _previousPhase = null;


        bool _changingState = false;
        

        public System.Type GetCurrentPhase()
        {
            return _curPhase == null ? null : _curPhase.GetType();
        }


        protected virtual void Awake()
        {
        }
 

        protected virtual void Update()
        {
            if (_queuedPhase!=null && _changingState == false)
            {
                _changingState = true;
                GM_Phase newPhase = _queuedPhase;
                _queuedPhase = null;
                StartCoroutine(SetActivePhase(newPhase));
            }
        }

        public void FinishEnteringPhase()
        {
          
        }

        protected void AddPhase( GM_Phase phase)
        {
            _phases.Add(phase);
        }

        public T GetPhase<T>() where T : GM_Phase
        {
            for (int i = 0; i < _phases.Count; i++)
            {
                if (_phases[i] is T)
                    return _phases[i] as T;

            }
            Dbg.Assert(false, "Requested phase type not found: " + typeof(T).ToString());
            return null;
        }

        public void QueuePhase( GM_Phase p )
        {
            Dbg.Assert(_queuedPhase == null);
            Dbg.Assert(p != null);
            _queuedPhase = p;
        }


        public void QueuePhase<T>() where T : GM_Phase
        {
            Debug.Log("Queing phase: " + typeof(T).GetType().ToString());
            _queuedPhase = GetPhase<T>();
        }

        IEnumerator SetActivePhase(GM_Phase newPhase)
        {
            Debug.Log("set active phase 1 : " + newPhase.GetType().ToString());


            UN_CameraFade.ClearAll();
            bool readyToSwitch1 = false;
            UN_CameraFade.FadeToBlack(() => { readyToSwitch1 = true; }, 2.0f);      //### PJS TO DO : remove time

            IEnumerator it;
            _previousPhase = _curPhase;
            if (_previousPhase != null)
            {
                it = _previousPhase.Exit(newPhase);
                while (it.MoveNext())
                {
                    yield return null;
                }

                UN.SetActive(_previousPhase, false);
            }

            // wait for fade to black to be done
            while (readyToSwitch1 == false)
            {
                yield return null;
            }

            _curPhase = newPhase;
            UN.SetActive(newPhase, true);
            it = newPhase.Enter(_previousPhase);
            while (it.MoveNext())
            {
                yield return null;
            }

            string newSceneName = newPhase.SceneName;
            if (!string.IsNullOrEmpty(newSceneName) && newSceneName != SceneManager.GetActiveScene().name)
            {
                AsyncOperation loadingOp = SceneManager.LoadSceneAsync(newSceneName);
                if (loadingOp != null)
                {
                    while (loadingOp.isDone == false)
                        yield return null;

                    Events.SendGlobal(new SceneChangedEvent() { Name = _curPhase.SceneName });
                }
            }

            Events.SendGlobal(new GM_GamePhaseChangedEvent()
            {
                NewPhase = _curPhase == null ? null : _curPhase.GetType(),
                OldPhase = _previousPhase == null ? null : _previousPhase.GetType()
            });

            {
                bool readyToSwitch2 = false;
                UN_CameraFade.FadeToTransparent(() => { readyToSwitch2 = true; }, 2.0f);    //### pjs todo fix time
                while (readyToSwitch2 == false)
                    yield return null;
            }
            _changingState = false;

            Debug.Log("set active phase 2 : " + newPhase.GetType().ToString());

        }
    }

}



#if USING_UNITY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLib.Game
{

    /// <summary>
    /// Sent when all of the transitions into a game phase are complete
    /// </summary>
    public class GamePhaseEnteredEvent : GameEvent
    {
        public GamePhase Phase;
    }

    public class GamePhaseMgr : BehaviourSingleton<GamePhaseMgr>
    {
        [SerializeField]
        List<GamePhase> _phases = new List<GamePhase>();


        GamePhase _currentPhase = null;

        // transition props
        GamePhase _leavingPhase = null;
        GamePhase _enteringPhase = null;

        Queue<GamePhase> _transitionQueue = new Queue<GamePhase>();


        void Awake()
        {
        }


        // Use this for initialization
        void Start()
        {
            if (_phases != null && _phases.Count > 0)
                _transitionQueue.Enqueue(_phases[0]);
        }

        // Update is called once per frame
        void Update()
        {
            if (_transitionQueue.Count != 0 && IsTransitioning() == false)
            {
                GamePhase newPhase = _transitionQueue.Dequeue();
                StartCoroutine(TransitionToPhase(newPhase));
            }
        }


        GamePhase FindPhaseByType<NewPhaseType>() where NewPhaseType : GamePhase
        {
            for (int i = 0; i < _phases.Count; i++)
            {
                if (_phases[i] is NewPhaseType)
                    return _phases[i];
            }

            Debug.Assert(false, "Cannot find phase of type " + typeof(NewPhaseType).ToString());
            return null;
        }


        GamePhase FindPhaseByName(string name) 
        {
            for (int i = 0; i < _phases.Count; i++)
            {
                if (_phases[i].gameObject.name == name)
                    return _phases[i];
            }

            Debug.Assert(false, "Cannot find phase of type " + name);
            return null;
        }


        public static void RequestPhaseChange(string name) 
        {

            Debug.Log("Phase change requested to " +name);

            Instance._transitionQueue.Enqueue(Instance.FindPhaseByName(name));
        }

        public static void RequestPhaseChange<NewPhaseType>() where NewPhaseType : GamePhase
        {
            Debug.Log("Phase change requested to " + typeof(NewPhaseType));

            Instance._transitionQueue.Enqueue(Instance.FindPhaseByType<NewPhaseType>());
        }

        bool IsTransitioning()
        {
            return _enteringPhase != null;
        }


        IEnumerator TransitionToPhase(GamePhase newPhase)
        {
            if (_currentPhase == newPhase)
            {
                Debug.Log("trying to transition to same phase: " + newPhase);
                yield break;
            }

            if (newPhase == null)
            {
                Debug.LogError("trying to transition to null phase ");
                yield break;
            }


            _leavingPhase = _currentPhase;
            _enteringPhase = newPhase;

            Debug.Log("Begin transitioning from phase " + _leavingPhase + " to " + newPhase);

            IEnumerator it;

            if (_leavingPhase != null)
            {

                it = _leavingPhase.StartLeavingPhase(newPhase);
                if (it != null)
                    yield return StartCoroutine(it);

                UnityUtil.SetActive(_leavingPhase.gameObject, false);
            }

            _currentPhase = _enteringPhase;

            UnityUtil.SetActive(_enteringPhase.gameObject, true);

            it = _enteringPhase.StartEnteringPhase(_leavingPhase);
            if (it != null)
                yield return StartCoroutine(it);


            Debug.Log("Finished transitioning from phase" + _leavingPhase + " to " + newPhase);

            _enteringPhase = null;
            _leavingPhase = null;

            Events.SendGlobal(new GamePhaseEnteredEvent() { Phase = _currentPhase });
        }
    }
}
#endif