using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MachineOutput : StackArea
{
    public GameObject ElementPrefab
    {
        get => elementPrefab;
    }

    private float collectTime = .1f;

    public void PlaceElement(GameObject go, Vector3 position)
    {
        go.transform.parent = transform;
        go.transform.forward = transform.forward;
        go.transform.localPosition = position;
        SpawnPosition();
        xCount++;
        stackedElements.Push(go);
    }

    private bool _isTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<ICanCollect>(out ICanCollect canCollect) && stackedElements.Count > 0)
        {
            _isTrigger = true;
            StartCoroutine(StartCollect(canCollect));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<ICanCollect>(out ICanCollect canCollect))
        {
            _isTrigger = false;
        }
    }

    IEnumerator StartCollect(ICanCollect canCollect)
    {
        if (canCollect.CanCollect(elementType) && stackedElements.Count > 0)
        {
            var obj = DestroyElement();
            obj.transform.parent = null;
            var target = canCollect.Target();
            obj.transform.DOJump(target.position, 1f, 1, collectTime).OnComplete(() => 
                { canCollect.Collect(obj); });
        }

        yield return new WaitForSeconds(collectTime);
        if (_isTrigger) StartCoroutine(StartCollect(canCollect));
    }
}