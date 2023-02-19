using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StackArea : MonoBehaviour
{
    [SerializeField] private bool aoutoCreate;
    [SerializeField] protected ElementType elementType;
    private List<GameObject> elementsPrefabs;
    protected GameObject elementPrefab;
    protected Stack<GameObject> stackedElements;
   
    [Header("Stack"), SerializeField] private Vector2 stackCount;
    [SerializeField] private Vector2 elementSize;
    [SerializeField] private float capacity = 20;
    [SerializeField] private Vector3 startPoint = new Vector3(0, 0, -.5f);
    public ElementType ElementType { get=>elementType; }
    protected void Start()
    {
        elementsPrefabs = EventManager.getElementsPrefabs?.Invoke();
        FillElementPrefab();
        stackedElements = new Stack<GameObject>();
        if (aoutoCreate) StartCoroutine(AoutoCreater());
    }

    private IEnumerator AoutoCreater()
    {   CreateElement();
        yield return new WaitForSeconds(2f);
        StartCoroutine(AoutoCreater());
    }
    private void FillElementPrefab()
    {
        elementsPrefabs.ForEach((go) =>
        {
            if (Enum.Parse<ElementType>(go.transform.tag) == elementType) elementPrefab = go;
        });
    }


    protected  void CreateElement()
    {
        var go = ObjectPooler.SharedInstance.GetPooledObject((int)elementType);
        go.SetActive(true);
        go.transform.parent = transform;
        go.transform.forward = transform.forward;
        go.transform.localPosition = SpawnPosition();
        xCount++;
        stackedElements.Push(go);
    }

    public bool CanCollect(int amount = 1)
    {
        return stackedElements.Count >= amount ? true : false;
    }

    public bool CanStack()
    {
        return capacity > stackedElements.Count ? true : false;
    }
    public GameObject DestroyElement()
    {
     
        var lastElement = stackedElements.Pop();

        //lastElement.gameObject.SetActive(false);
        if (xCount < 1)
        {
            yCount--;
            xCount = (int)stackCount.x;
        }

        xCount--;
        return lastElement;
    }

    protected int xCount = 0;
    private int yCount = 0;

    public Vector3 SpawnPosition()
    {
        if (xCount >= stackCount.x)
        {
            xCount = 0;
            yCount++;
        }

        var offsetX = xCount * elementSize.x;
        var offsetY = yCount * elementSize.y;
        return startPoint + new Vector3(0, offsetY, offsetX);
    }
}