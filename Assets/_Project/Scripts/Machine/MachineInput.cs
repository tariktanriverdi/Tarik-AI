
using System.Collections;

using DG.Tweening;
using UnityEngine;

public class MachineInput : StackArea
{
    private float playerDeployTime = 0.1f;
    
    private bool isTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<ICanStack>(out ICanStack canStack))
        {
            isTrigger = true;
            StartCoroutine(StartStack(canStack));
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<ICanStack>(out ICanStack canStack))
        {
            isTrigger = false;
        }
    }

    private IEnumerator StartStack(ICanStack canStack)
    {
        if (canStack.CanStack(elementType))
        {
            var stackedObj = canStack.Stack();
            stackedObj.transform.parent = transform;
            var position = SpawnPosition();
            stackedObj.transform.DORotate(transform.rotation.eulerAngles, playerDeployTime );
            stackedObj.transform.DOLocalJump(position,1f,1,playerDeployTime).OnComplete(() =>
            {
                PlaceElement(stackedObj,position);
            });
            
        }

        yield return new WaitForSeconds(playerDeployTime );
        if(isTrigger)  StartCoroutine(StartStack(canStack));
    }
    
    public void PlaceElement(GameObject go,Vector3 position)
    {
       
        go.transform.parent = transform;
        go.transform.localPosition = position;
        SpawnPosition();
        xCount++;
        stackedElements.Push(go);
    }
}
