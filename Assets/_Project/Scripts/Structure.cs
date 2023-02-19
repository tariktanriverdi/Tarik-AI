using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [SerializeField] private ElementType elementType;
    [SerializeField] private GameObject preBrickPrefab;
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private List<GameObject> preBricks;
    [SerializeField] private Vector3 preBrickSize = new Vector3(0.5f, .2f, .2f);
    [SerializeField] private int floor = 10;
    [SerializeField] private GameObject finalModel;
    private List<GameObject> _stackedObjs;
    private Queue<GameObject> _createdPreBricks;
    private float deployTime = 0.5f;

    private bool _isFinish = false;

    private void Awake()
    {
        Initialize();
        CreatePreBricks();
    }


    private void Initialize()
    {
        finalModel.SetActive(false);
        _createdPreBricks = new();
        _stackedObjs = new();
    }


    private bool isTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<ICanStack>(out ICanStack canStack))
        {
            isTrigger = true;
            StartCoroutine(StartStack(canStack));
        }
    }

    private IEnumerator StartStack(ICanStack canStack)
    {
        if (canStack.CanStack(elementType) && _createdPreBricks.Count > 0)
        {
            var stackedObj = canStack.Stack();
            stackedObj.transform.parent = transform;
            // stackedObj.transform.forward = transform.forward;
            var replacePreBrick = ReplacePreBrick();
            stackedObj.transform.DORotate(replacePreBrick.rotation.eulerAngles, deployTime);
            _stackedObjs.Add(stackedObj);
            stackedObj.transform.DOJump(replacePreBrick.position, 1f, 1, deployTime).OnComplete(() =>
            {
                SetActivityFalse(replacePreBrick.gameObject);
            });
        }

        yield return new WaitForSeconds(deployTime / 5f);
        if (isTrigger) StartCoroutine(StartStack(canStack));
    }

    private void SetActivityFalse(GameObject go)
    {
        go.SetActive(false);
        if (_createdPreBricks.Count == 0 && !_isFinish)
        {
            _isFinish = true;
            OpenFinalObject();
        }
    }

    private Transform ReplacePreBrick()
    {
        var result = _createdPreBricks.Dequeue();
        return result.transform;
    }

    private void OpenFinalObject()
    {
        finalModel.SetActive(true);
        finalModel.transform.DOPunchScale(Vector3.one * .3f, 1f, 10, 10f);
        _stackedObjs.ForEach((x) => x.SetActive(false));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<ICanStack>(out ICanStack canStack))
        {
            isTrigger = false;
        }
    }


    private void CreatePreBricks()
    {
        for (int createdFloor = 0; createdFloor < floor; createdFloor++)
        {
            for (int i = 0; i < spawnPositions.Count - 1; i++)
            {
                Vector3 distance = spawnPositions[i].position - spawnPositions[i + 1].position;
                var count = (distance.magnitude / preBrickSize.x) + 1;

                for (int j = 0; j < count; j++)
                {
                    float t = (float)j / (float)(count - 1);
                    Vector3 pos = Vector3.Lerp(spawnPositions[i].position, spawnPositions[i + 1].position, t);
                    var obj = Instantiate(preBrickPrefab, pos, Quaternion.identity, transform);
                    obj.transform.forward = spawnPositions[i].forward;
                    obj.transform.position += obj.transform.forward * preBrickSize.x * 0.5f;
                    obj.transform.position += Vector3.up * createdFloor * preBrickSize.y;
                    _createdPreBricks.Enqueue(obj);
                }
            }
        }
    }
}