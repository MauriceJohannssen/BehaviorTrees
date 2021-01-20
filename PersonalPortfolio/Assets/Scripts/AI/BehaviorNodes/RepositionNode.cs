using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionNode : Node
{
    public RepositionNode()
    {
        
    }

    public override State EvaluateState()
    {
        return State.Running;
    }
}
