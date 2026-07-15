using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBaseStateBehaviour
{

    public abstract void InitializeState(); // acquire all necessary references from context and store them in state. Happens once

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void EndState();

    //public abstract bool TransitionConditionsMet(); // called by state manager when it is

}
