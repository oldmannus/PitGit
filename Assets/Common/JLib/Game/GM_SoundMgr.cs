using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;


namespace JLib.Game
{
    public class GM_SoundMgr : MonoBehaviour
    {
        // --------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// This only plays for the local player. One wouldn't want to hear the cheer for winning for everyone around 
        /// you for example. 
        /// </summary>
        /// <param name="src"></param>
        public void PlayForSelf(AudioSource src, bool isLocallyControlledObject)
        // --------------------------------------------------------------------------------------------------------------------
        {
            if (CanPlay(src, false, true, isLocallyControlledObject))
            {
                if (_tracePlayRequests)
                    Debug.Log("PlayForSelf: " + src.gameObject.name + ":" + src.clip.name);

                src.Play();
            }
        }

        // --------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// This starts something playing if it's not currently playing
        /// </summary>
        /// <param name="src"></param>
        public void PlayIfNot(AudioSource src)
        // --------------------------------------------------------------------------------------------------------------------
        {
            if (CanPlay(src, true, false))
            {
                if (_tracePlayRequests)
                    Debug.Log("PlayIfNot: " + src.gameObject.name + ":" + src.clip.name);
                src.Play();
            }
        }

        // --------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// This starts something playing, if it can
        /// </summary>
        /// <param name="src"></param>
        public void Play(AudioSource src)
        // --------------------------------------------------------------------------------------------------------------------
        {
            if (CanPlay(src, false, false))   // in this case, just checks for null and such
            {
                if (_tracePlayRequests)
                    Debug.Log("Play: " + src.gameObject.name + ":" + src.clip.name + " " + src.gameObject.GetInstanceID());

                src.Play();
            }
        }



        // --------------------------------------------------------------------------------------------------------------------
        public void Stop(AudioSource src)
        // --------------------------------------------------------------------------------------------------------------------
        {
            if ( CanStop(src))
            {
                src.Stop();
            }
        }


        // --------------------------------------------------------------------------------------------------------------------
        public AudioSource InstantiateSource(string name, Transform attachObj)
        // --------------------------------------------------------------------------------------------------------------------
        {
            Debug.Assert(attachObj);
            if (attachObj == null)
            {
                Debug.LogError("AudioDB.InstantiateSource given no object to attach sound to!");
                return null;
            }

            AudioSource template;
            AudioSource newSource;
            if (_sources.TryGetValue(name, out template) == false)
            {
                if (_warnOfMissingSounds)
                    Debug.LogError("Unable to find audio source template named " + name + " Making blank one. ");
                GameObject go = new GameObject(name + " replacement without sound");
                newSource = go.AddComponent<AudioSource>();
            }
            else
            {
                Debug.Assert(template != null);
                newSource = GameObject.Instantiate(template);
            }


            // attach source to that object
            Debug.Assert(newSource != null);
            newSource.transform.SetParent(attachObj);
            newSource.transform.localPosition = Vector3.zero;
            newSource.gameObject.SetActive(true);

            return newSource;
        }


        #region Internals
        [SerializeField]
        bool _tracePlayRequests = false;
        [SerializeField]
        bool _warnOfMissingSounds = false;
        [SerializeField]
        bool _warnOfDisabledSounds = false;
        [SerializeField]
        bool _warnOfAlreadyPlayingSounds = false;
        [SerializeField]
        bool _warnOfSoundsWithoutAuthority = false;
        [SerializeField]
        bool _warnOfSoundsWithoutClip = false;

        Dictionary<string, AudioSource> _sources = new Dictionary<string, AudioSource>();

        void Awake()
        {
            foreach (AudioSource v in GetComponentsInChildren(typeof(AudioSource), true))
            {
                if (_sources.ContainsKey(v.gameObject.name))
                {
                    Debug.LogError("AudioDB has two audio sources named " + v.gameObject.name);
                }
                else
                {
                    _sources.Add(v.gameObject.name, v);
                }
            }
        }


        private void PlayOneShot(AudioSource src, AudioClip clip)
        {
            if (src != null && clip != null)
            {
                src.PlayOneShot(clip);
            }
        }


        private void PlayLoop(AudioSource src, AudioClip clip)
        {
            if (src != null && clip != null)
            {
                src.clip = clip;
                src.Play();
            }
        }



        bool CanPlay(AudioSource src, bool checkForPlaying, bool checkForAuthority, bool? srcHasAuthority = null)
        {
            if (IsSourceValid(src) == false)
                return false;


            if (src.clip == null)
            {
                if (_warnOfSoundsWithoutClip)
                {
                    Debug.LogError("Trying to play sound with no clip " + src.clip);
                }
                return false;
            }

            if (checkForPlaying)
            {
                if (src.isPlaying)
                {
                    if (_warnOfAlreadyPlayingSounds)
                    {
                        Debug.LogError("Trying to play AudioSource that's already playing. " + src.name);
                    }
                    return false;
                }
            }

            if (checkForAuthority)
            {
                Debug.Assert(srcHasAuthority != null);

                if (srcHasAuthority == false)
                {
                    if (_warnOfSoundsWithoutAuthority)
                    {
                        Debug.LogError("Trying to play AudioSource that has no authority. " + src.name);
                        return false;
                    }
                }
            }

            return true;
        }

        bool CanStop(AudioSource src)
        {
            if (IsSourceValid(src) == false)
                return false;

            return src.isPlaying;
        }

        bool IsSourceValid(AudioSource src)
        {
            if (src == null)
            {
                if (_warnOfMissingSounds)
                    Debug.LogError("Trying to access AudioSource that's null. Did you forget to create it through AudioDB.InstantiateSource?");
                return false;
            }
            if (src.enabled == false)
            {
                if (_warnOfDisabledSounds)
                    Debug.LogError("Trying to access AudioSource that's disabled. " + src.name);
                return false;
            }
            return true;
        }


        #endregion

    }

}

