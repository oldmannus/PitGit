using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pit.Utilities;

namespace Pit.Flow.UIHelpers
{
    public class TabControl : MonoBehaviour
    {
        [SerializeField]
        List<TabControlPage> _pages = new List<TabControlPage>();

        [SerializeField]
        bool _looping = false;

        [SerializeField]
        bool _returnToDefault = false;

        //[SerializeField]
        //int _defaultNdx = 0;


        //[SerializeField]
        //string _leftInputName = "TabControlL";

        //[SerializeField]
        //string _rightInputName = "TabControlR";

        int _selectedIndex = -1;


        // Start is called before the first frame update
        void Start()
        {
            if (_pages.Count > 0)
            {
                _selectedIndex = 0;
                for (int i = 0; i < _pages.Count; i++)
                {
                    _pages[i].SetTabControl(this);
                    _pages[i].gameObject.SetActive(true);
                    if (i == 0)
                        _pages[i].ActivatePage(1);
                    else
                        _pages[i].DeactivatePage(0);
                }
            }
        }

        private void OnEnable()
        {

            if (_selectedIndex < 0 || _returnToDefault)
            {
                _selectedIndex = 0;
                SelectPage(_selectedIndex);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_pages.Count > 0)
            {
                //if (Input.GetButtonDown(_leftInputName))
                //{
                //    ChangeSelection(-1);
                //}
                //else if (Input.GetButtonDown(_rightInputName))
                //{
                //    ChangeSelection(1);
                //}
            }
        }

        void ChangeSelection(int amount)
        {
            int curNdx = _selectedIndex;
            int newNdx = curNdx + amount;
            if (newNdx < 0)
            {
                if (_looping)
                    newNdx = _pages.Count - 1;
                else
                    return; // don't do anything, can't go left when non-looping
            }
            else if (newNdx >= _pages.Count)
            {
                if (_looping)
                    newNdx = 0;
                else
                    return; // do do anything can't go right when non-looping
            }

            SelectPage(newNdx);
        }

        public void SelectPage(TabControlPage page)
        {
            Dbg.Assert(page != null);
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i] == page)
                {
                    SelectPage(i);
                    return;
                }
            }
            Dbg.Assert(false);
        }

        void SelectPage(int newNdx)
        {
            Dbg.Assert(newNdx >= 0 && newNdx < _pages.Count, "New Ndx : " + newNdx);
            int oldNdx = _selectedIndex;

            _pages[oldNdx].DeactivatePage(newNdx);
            _selectedIndex = newNdx;
            _pages[_selectedIndex].ActivatePage(oldNdx);
        }
    }

}