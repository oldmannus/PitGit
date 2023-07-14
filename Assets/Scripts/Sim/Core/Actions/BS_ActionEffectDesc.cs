using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pit.Sim;

public class BS_ActionEffectDesc
{
    public float StartTime;
    List<BS_ActionConditional> _endConditions = new List<BS_ActionConditional>();
    public GameObject Subject;
    public Vector3 Destination;

    public virtual void Apply()
    {
    }
}


public class BS_ActionEffect_Pawn : BS_ActionEffectDesc
{
    public Pawn SubjectPawn;
}

/// <summary>
/// Pawn moves from one place to another
/// </summary>
public class BS_ActionEffect_PawnMove : BS_ActionEffect_Pawn{ }

public class BS_ActionEffect_PawnFly : BS_ActionEffect_PawnMove{}
public class BS_ActionEffect_PawnJump : BS_ActionEffect_PawnMove{}
public class BS_ActionEffect_PawnSprint : BS_ActionEffect_PawnMove{}
public class BS_ActionEffect_PawnRun: BS_ActionEffect_PawnMove{}

public class BS_ActionEffect_MoveObject : BS_ActionEffectDesc {  }
public class BS_ActionEffect_DestroyObject : BS_ActionEffectDesc { }
public class BS_ActionEffect_InstantiateObject : BS_ActionEffectDesc { }



