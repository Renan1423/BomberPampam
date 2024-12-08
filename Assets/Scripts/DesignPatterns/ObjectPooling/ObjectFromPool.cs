using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectFromPool : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private float timeToUnspawn = 0.5f;
    [SerializeField]
    private bool unspawnAutomatically = true;

    [HideInInspector]
    public UnityEvent OnDisablePooledObject;

    public virtual void OnObjectSpawn()
    {
        if(unspawnAutomatically)
            StartCoroutine(UnspawnCoroutine());
    }

    private IEnumerator UnspawnCoroutine() 
    {
        yield return new WaitForSeconds(timeToUnspawn);

        Unspawn();
    }

    public virtual void Unspawn()
    {
        OnDisablePooledObject?.Invoke();
        gameObject.SetActive(false);
    }
}
