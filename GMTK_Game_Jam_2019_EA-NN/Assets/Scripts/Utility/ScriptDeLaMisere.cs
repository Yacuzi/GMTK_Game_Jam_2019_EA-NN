using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDeLaMisere : MonoBehaviour
{
    public Character_Move theActualCharacter;

    //Called by animation
    public void resetPosChara()
    {
        theActualCharacter.resetPosChara();
    }

    public void generateTrail()
    {
        theActualCharacter.generateTrail();
    }

    public void setUndead ()
    {
        theActualCharacter.setUndead();
    }
}
