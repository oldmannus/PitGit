using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace JLib.Unity
{
    public class FloatyTextMgr : MonoBehaviour
    {
        public class FloatyObjectBehavior
        {
            public bool IsTimed = true;
            public bool CustomColor = false;
            public bool RandomPositioning = true;
            public bool FloatUpwards = true;
            public bool AlphaFade = true;

            public float Duration = 1.0f; // if isTimed is on
            public Color Color;    // if CustomColor is on

        }


        class FloatyObject
        {
            public int Id;
            public Text Text;
            public Color InitialColor;
            public Color FinalColor;
            public float StartTime;
            public GameObject Object;

            public FloatyObjectBehavior Behavior;

            const float FloatSpeed = 0.5f;

            // returns true if time to die
            public bool Update()
            {
                Debug.Assert(Behavior.IsTimed == true);

                float elapsed = Time.time - StartTime;
                if (elapsed > Behavior.Duration)
                    return true;

                if (Behavior.AlphaFade)
                {
                    Text.color = Color.Lerp(InitialColor, FinalColor, elapsed / Behavior.Duration);
                }

                if (Behavior.FloatUpwards)
                {
                    Vector3 position = Object.transform.position;
                    position.y += Time.deltaTime * FloatSpeed;
                    Object.transform.position = position;
                }


                Vector3 v = Camera.main.transform.position - Object.transform.position;
                v.x = v.z = 0.0f;
                Object.transform.LookAt(Camera.main.transform.transform.position - v);
                Object.transform.Rotate(0, 180, 0);

                return false;
            }
        }

        public GameObject TemplateFloatyText;


        List<FloatyObject> _toBeKilled = new List<FloatyObject>();
        List<FloatyObject> _activeFloatyText = new List<FloatyObject>();
        Stack<FloatyObject> _floatyTextPool = new Stack<FloatyObject>();
        int _nextFloatyId = 1;
        FloatyObjectBehavior _defaultBehavior = new FloatyObjectBehavior();
        public Canvas WorldCanvas;
        public float RandomOffset = 0.6f;

        void OnEnable()
        {
        }

        void OnDisable()
        {
            for (int i = 0; i < _activeFloatyText.Count(); i++ )
            {
                _activeFloatyText[i].Object.SetActive(false);
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < _activeFloatyText.Count(); i++)
            {
                GameObject.Destroy(_activeFloatyText[i].Object);
            }
        }

        

        // -----------------------------------------------------------------------------
        FloatyObject GetFloatyText()
        // -----------------------------------------------------------------------------
        {
            Debug.Assert(_floatyTextPool != null);


            if (_floatyTextPool.Count() == 0)
                AddToFloatyPool();

            FloatyObject floaty = _floatyTextPool.Pop();
            floaty.StartTime = Time.time;
            return floaty;
        }


        void Update()
        {
            int count = _activeFloatyText.Count();
            for (int i = 0; i < count; i++)
            {
                if (_activeFloatyText[i].Update())
                {
                    _toBeKilled.Add(_activeFloatyText[i]);
                }
            }

            for (int i = 0; i < _toBeKilled.Count(); i++)
            {
                _floatyTextPool.Push(_toBeKilled[i]);
                _activeFloatyText.Remove(_toBeKilled[i]);
            }
            _toBeKilled.Clear();
        }

        // -----------------------------------------------------------------------------
        public int ShowFloatyText(Vector3 position, string val, FloatyObjectBehavior behavior = null)
        // -----------------------------------------------------------------------------
        {
            FloatyObject floaty = GetFloatyText();

            floaty.Text.text = val;

            if (behavior == null)
                behavior = _defaultBehavior;

            floaty.Behavior = behavior;

            StartFloatyAt(floaty, position);
            if (behavior.IsTimed)
                _activeFloatyText.Add(floaty);

            return floaty.Id;
        }


        // -----------------------------------------------------------------------------
        void AddToFloatyPool()
        // -----------------------------------------------------------------------------
        {
            for (int i = 0; i < 10; i++)
            {
                FloatyObject fo = new FloatyObject();
                fo.Object = GameObject.Instantiate(TemplateFloatyText);
                fo.Text = fo.Object.GetComponent<Text>();
                _floatyTextPool.Push(fo);

                fo.Object.transform.SetParent(WorldCanvas.transform, false);
                fo.Object.SetActive(false);
                fo.InitialColor = fo.Text.color;

                Color f = fo.Text.color;
                f.a = 0.0f;
                fo.FinalColor = f;

                fo.Id = _nextFloatyId++;
            }
        }

        // -----------------------------------------------------------------------------
        void StartFloatyAt(FloatyObject obj, Vector3 vector)
        // -----------------------------------------------------------------------------
        {
            if (obj.Behavior.RandomPositioning)
            {
                vector.x += (Random.value * RandomOffset) - (RandomOffset / 2);
                vector.y += (Random.value * RandomOffset) - (RandomOffset / 2);
                vector.z += (Random.value * RandomOffset) - (RandomOffset / 2);
            }

            obj.Object.transform.position = vector;
            obj.Object.SetActive(true);
        }
    }
}

