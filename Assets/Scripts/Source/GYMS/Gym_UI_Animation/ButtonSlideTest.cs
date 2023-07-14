using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSlideTest : MonoBehaviour
{
    [SerializeField]
    Animator _btn1Animator = null;
    bool on = true;
    float time = Time.time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()   
    {
        if (time < Time.time + 3.0f)
        {
            on = !on;
            _btn1Animator.SetBool("IsVisible", on);
        }    time = Time.time;
        
    }
}
