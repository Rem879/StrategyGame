using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CarAttack : MonoBehaviour
{
    [NonSerialized] public int _health = 100;
    public float radius = 70f;
    public GameObject bullet;
    private Coroutine _coroutine = null;

    private void Update()
    {
        DetectCollistion();
    }

    private void DetectCollistion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        if (hitColliders.Length == 0 && _coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            if (gameObject.CompareTag("Enemy"))

                GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(gameObject.transform.position);
        }
        
        foreach (var el in hitColliders)
        {
            if ((gameObject.CompareTag("Player") && el.gameObject.CompareTag("Enemy")) ||
                (gameObject.CompareTag("Enemy") && el.gameObject.CompareTag("Player")))
            {
                if (gameObject.CompareTag("Enemy"))
                    GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(el.transform.position);
                
                if(_coroutine == null)
                    _coroutine = StartCoroutine(StartAttack(el));
            }
        }
    }

    IEnumerator StartAttack(Collider enemyPos)
    {
       
            GameObject obj = Instantiate(bullet, transform.GetChild(1).position, Quaternion.identity);
            obj.GetComponent<BulletController>().position = enemyPos.transform.position;
            yield return new WaitForSeconds(1f);
            StopCoroutine(_coroutine);
            _coroutine = null;
    }
}
