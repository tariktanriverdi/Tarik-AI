using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanCollect
{
    public bool CanCollect(ElementType type);
    public void Collect(GameObject element);
    public Transform Target();

}
