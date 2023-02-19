using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackController
{
    private StackManager _stackManager;

    public void Init(StackManager stackManager)
    {
        _stackManager = stackManager;
    }


    public bool CanStack(ElementType type)
    {
        return _stackManager.CanDeploy(type);
    }

    public GameObject Stack()
    {
        return _stackManager.DestroyStackElement();
    }

    public bool CanCollect(ElementType type)
    {
        return _stackManager.CanStackByType(type);
    }

    public void Collect(GameObject element)
    {
        _stackManager.CreateStackElement(element);
    }

    public Transform Target()
    {
        return _stackManager.LastElement().transform;
    }
}