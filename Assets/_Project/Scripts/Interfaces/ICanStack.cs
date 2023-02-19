using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanStack
{
    public bool CanStack(ElementType type);
    public GameObject Stack();
    
}
