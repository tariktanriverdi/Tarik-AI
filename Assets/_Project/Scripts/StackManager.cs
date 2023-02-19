using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class StackManager : MonoBehaviour
{
    [SerializeField] private List<GameObject>  stackElement;
    [SerializeField] private int capacity=20;
    [SerializeField] private float increaseAmountY=.2f;
    [SerializeField] public float animSpeed = 1000;
    public ElementType stackedType =ElementType.Emty;

    
 [HideInInspector]   public List<GameObject> stackElements;

// public Stack<GameObject> stackElements;
    private float offset =0;

    private void Start()
    {
        offset = transform.position.y;
    }

    private void Update()
    {
#if UNITY_EDITOR
      //  DebugCreateDestroy();
#endif
        if (stackElements.Count > 0) StackAnimation();
    }
   
    private void StackAnimation()
    {
        SetFirstElementPosRot();
        for (int i = 1; i < stackElements.Count; i++)
        {
            var target = stackElements[i - 1].transform;
            var obj = stackElements[i].transform;
            float animTime = Time.deltaTime * animSpeed * (1f / i);
            EqualRotation(obj, target);
            LerpPosition(obj, target, animTime);
        }
    }

    private void SetFirstElementPosRot()
    {
        var o = gameObject;
        stackElements[0].transform.position = o.transform.position;
        stackElements[0].transform.rotation = o.transform.rotation;
    }

    private void EqualRotation(Transform obj, Transform target)
    {
        obj.transform.rotation = target.transform.rotation;
    }

    private void LerpPosition(Transform obj, Transform target, float animTime)
    {
        var targetPos = target.position;
        targetPos.y = obj.position.y;
        obj.localPosition = Vector3.Lerp(obj.localPosition, targetPos, animTime);
    }

    // private void DebugCreateDestroy()
    // {
    //     if (Input.GetKeyDown(KeyCode.E)) CreateStackElement(stackElement[0]);
    //     if (Input.GetKeyDown(KeyCode.R)) DestroyStackElement();
    //     if (Input.GetKeyDown(KeyCode.T)) CanStackByType(ElementType.Wood_1);
    //     if (Input.GetKeyDown(KeyCode.Y)) CanStackByType(ElementType.Wood_2);
    //     if (Input.GetKey(KeyCode.Q)) CreateStackElement(stackElement[1]);
    // }

    public GameObject DestroyStackElement()
    {
      //  if (stackElements.Count < 1) return;
        if (stackElements.Count == 1) stackedType = ElementType.Emty;
        
        var lastElement = stackElements.Last();
       // lastElement.SetActive(false);
        stackElements.Remove(lastElement);
        offset -= increaseAmountY;
        return lastElement;
    }

    public bool CanStackByType(ElementType type)
    {   
      
        return (type == stackedType || stackedType == ElementType.Emty) ? true : false;
        // Debug.Log($"Input:{type} output:{stackedType}{result}");
        // return result;
    }

    public bool CanDeploy(ElementType type)
    {
    return (type == stackedType) ? true : false;
    }
    public void CreateStackElement(GameObject objToInstantiate)
    {  
        //Delete
      // if(! CanStackByType( Enum.Parse<ElementType>(objToInstantiate.transform.tag) )) return;
        //
        var go = objToInstantiate;
        var spawnPosition = transform.position;
        if (stackElements.Count > 0)
        {
            spawnPosition = stackElements.Last().transform.position;
            spawnPosition.y = offset;
            go.transform.localPosition = spawnPosition;
        }

        offset += increaseAmountY;
        stackElements.Add(go);
        stackedType = Enum.Parse<ElementType>(go.transform.tag);
    }

    public GameObject LastElement()
    {
        GameObject lastElement;
        if (stackElements.Count < 1) lastElement = gameObject;
       else lastElement= stackElements.Last();
        return lastElement;
    }
}