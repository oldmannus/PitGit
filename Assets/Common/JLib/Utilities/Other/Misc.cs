using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

static class Misc
{
    public static IEnumerator WaitThenCallback(YieldInstruction wait, System.Action callback)
    {
        yield return wait;
        callback();
    }

    public static IEnumerator WaitThenCallback(IEnumerator wait, System.Action callback)
    {
        yield return wait;
        callback();
    }
}
