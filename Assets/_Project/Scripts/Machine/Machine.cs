using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] private MachineInput machineInput;
    [SerializeField] private MachineOutput machineOutput;
    [SerializeField] private Transform outputElementSpawnPoint;
    [SerializeField] private Transform outputBandPoint;
    [SerializeField] private Transform inputElementJumpPoint;
    [SerializeField] private int inputElementCount = 2;
    [SerializeField] private int outputElementCount = 1;
    [SerializeField] private float productionTime = 1f;
    private GameObject outputElementPrefab;
    private ElementType _machineOutputElementTypeType;
    private bool _isGameStart = false;


    private void OnEnable()
    {
        EventManager.gameStart += GameStart;
    }

    private void OnDisable()
    {
        EventManager.gameStart -= GameStart;
    }

    protected void Start()
    {
        outputElementPrefab = machineOutput.ElementPrefab;
        _machineOutputElementTypeType = machineOutput.ElementType;
        //Debug
        GameStart();
    }

    private void GameStart()
    {
        StartCoroutine(Production());
        _isGameStart = true;
    }

    private IEnumerator Production()
    {
        if (machineInput.CanCollect(inputElementCount) && machineOutput.CanStack())
        {
            for (int i = inputElementCount; i > 0; i--)
            {
                var obj = machineInput.DestroyElement();
                JumpAnimation(obj, inputElementJumpPoint.position, () => { obj.gameObject.SetActive(false); });
                yield return new WaitForSeconds(productionTime);
            }

            for (int i = outputElementCount; i > 0; i--)
            {
                OutputAnimation(CreateOutputElement());

                yield return new WaitForSeconds(productionTime);
            }
        }

        yield return new WaitForSeconds(productionTime * 2f);
        StartCoroutine(Production());
    }

    private void OutputAnimation(GameObject createdOutputElement)
    {
        createdOutputElement.transform.DOMove(outputBandPoint.transform.position, productionTime / 5)
            .OnComplete(() => { JumpAnimationOutput(createdOutputElement); });
    }

    private GameObject CreateOutputElement()
    {
        var go = ObjectPooler.SharedInstance.GetPooledObject((int)_machineOutputElementTypeType);
        go.transform.parent = machineOutput.transform;
        go.SetActive(true);

        //  var go = Instantiate(outputElementPrefab, machineOutput.transform, true);
        go.transform.position = outputElementSpawnPoint.position;
        go.transform.forward = machineOutput.transform.forward;
        return go;
    }

    private void JumpAnimation(GameObject obj, Vector3 target, Action onComplete)
    {
        obj.transform.DOJump(target,
                1f, 1, productionTime / 2f)
            .OnComplete(() => { onComplete?.Invoke(); });
    }

    private void JumpAnimationOutput(GameObject obj)
    {
        var target = machineOutput.SpawnPosition();
        obj.transform.DOLocalJump(target,
                1f, 1, productionTime / 10f)
            .OnComplete(() => { machineOutput.PlaceElement(obj, target); });
    }
}