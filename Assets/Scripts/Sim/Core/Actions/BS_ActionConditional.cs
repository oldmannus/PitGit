using UnityEngine;
using System.Collections;




/// <summary>
/// this is a base class for all Conditionals in the action system. 
/// A conditional has one purpose - to report if it's true or false
/// Subclasses do the actual work
/// </summary>
public interface BS_ActionConditional
{

    bool IsTrue();
}

/// <summary>
/// Used for pawn related conditionals
/// </summary>
public class BS_AC_Pawn : BS_ActionConditional
{
    public bool IsTrue() { return true; }
}

// when current match is over, then this returns true
public class BS_AC_EndOfMatch : BS_ActionConditional
{
    //### TODO implen
    public bool IsTrue() { return true; }
}


public class BS_AC_PointInRange
{
    //Vector3 _point1;
    //Vector3 _point2;
    //float _rangeSqrd;

    //public bool IsTrue()
    //{
    //    Vector3 pt = _point1;
    //    pt -= _point2;
    //    return Vector3.SqrMagnitude(pt) < _rangeSqrd;
    //}
}

#if false

A) Every pawn needs list of actions it can perform. Either directly (attached to the pawn) or a mix of directly and indirectly (like some pawns can always do X)
B) Should be able to know if a given action is possibly allowable currently, so we can dim buttons or otherwise signal the user
C) Pawn decides to do action, designates parameters of action - who, when, what, where etc. 
D) 

Move action: 

  A) Need to be able to quer




#endif 

