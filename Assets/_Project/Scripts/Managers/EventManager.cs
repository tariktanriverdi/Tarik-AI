using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static Func<List<GameObject>> getElementsPrefabs;
    public static Action gameStart;
}
