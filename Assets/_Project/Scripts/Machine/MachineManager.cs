using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> elementsPrefabs;
    private void OnEnable()
    {
        EventManager.getElementsPrefabs += ReturnElementsPrefabs;
    }

    private void OnDisable()
    {
        EventManager.getElementsPrefabs -= ReturnElementsPrefabs;
    }

    private List<GameObject> ReturnElementsPrefabs()
    {
        return elementsPrefabs;
    }
}
