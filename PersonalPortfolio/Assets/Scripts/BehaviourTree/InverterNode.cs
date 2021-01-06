using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterNode : Node
{
    Node node;

    public InverterNode(Node node)
    {
        this.node = node;
    }

    public override State EvaluateStatus()
    {
        switch (node.EvaluateStatus())
        {
            case State.Running:
                nodeState = State.Running;
                break;

            case State.Success:
                nodeState = State.Failure;
                break;

            case State.Failure:
                nodeState = State.Success;
                break;

            default:
                break;
        }

        return nodeState;
    }
}
