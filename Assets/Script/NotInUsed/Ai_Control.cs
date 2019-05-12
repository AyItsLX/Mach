using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class Ai_Control : MonoBehaviour {

    public GameObject[] spawners;
    public float randomShotTime = 2;

    EZObjectPool objectPool;

    void Start () {
        objectPool = GetComponent<EZObjectPool>();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (randomShotTime > 0)
        {
            randomShotTime -= Time.deltaTime;
            objectPool.TryGetNextObject(spawners[0].transform.position, spawners[0].transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
