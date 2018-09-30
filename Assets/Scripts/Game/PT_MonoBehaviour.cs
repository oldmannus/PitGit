using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Pit
{

    public class PT_MonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (PT_Game.Instance == null && SceneManager.GetActiveScene() != null && SceneManager.GetActiveScene().buildIndex != 0)
            {
                Destroy(this);
                //enabled = false;
            }
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnDestroy()
        {
        }
        protected virtual void Update()
        {
        }
    }



}