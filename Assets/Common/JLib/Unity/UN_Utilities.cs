using UnityEngine;
using UnityEngine.UI;
using JLib.Utilities;

namespace JLib.Unity
{
    static class UN
    {
        static bool AlertNulls = false;

        public static void SetInteractable(Selectable go, bool b )
        {
            if (AlertNulls)
                Dbg.Assert(go != null);
            if (go != null && go.interactable != b)
            {
                go.interactable = b;
            }
        }

        public static void SetActive(GameObject go, bool b )
        {
            if (AlertNulls)
                Dbg.Assert(go != null);

            if (go != null && go.activeSelf != b)
            {
                go.SetActive(b);
            }
        }

        public static void SetActive(Transform go, bool b )
        {
            if (AlertNulls)
                Dbg.Assert(go != null);
            if (go != null)
                SetActive(go.gameObject, b);
        }

        public static void SetActive(Behaviour go, bool b )
        {
            if (AlertNulls)
                Dbg.Assert(go != null);

            if (go != null && go.gameObject != null && go.gameObject.activeSelf != b)
            {
                go.gameObject.SetActive(b);
            }
        }

        public static void SetEnabled(Behaviour go, bool b )
        {
            if (AlertNulls)
                Dbg.Assert(go != null);
            if (go != null && go.enabled != b)
            {
                go.enabled = b;
            }
        }

        public static void SetText( Text field, string text)
        {
            if (field != null && text != null)
                field.text = text;
        }

        public static void SetFill(Image field, float amt)
        {
            if (field != null)
            {
                Dbg.Assert(amt >= 0 && amt <= 1);
                field.fillAmount = amt;
            }
        }

    }
}
