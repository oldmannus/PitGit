using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BS_ActionInstance
{
    List<BS_ActionConditional> _conditions = new List<BS_ActionConditional>();
    List<BS_ActionEffectDesc>      _effects = new List<BS_ActionEffectDesc>();


    public bool AreConditionsSatisfied()
    {
        for (int i = 0; i < _conditions.Count; i++)
        {
            if (!_conditions[i].IsTrue())
                return false;
        }
        return true;
    }


    public void ApplyEffects()
    {
        for (int i = 0; i < _effects.Count; i++)
        {
            _effects[i].Apply();
        }
    }
}
