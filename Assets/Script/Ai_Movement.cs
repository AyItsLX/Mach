using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EZObjectPools;
using EZCameraShake;

public class Ai_Movement : MonoBehaviour {

    public Vector3 destination;
    public bool hasReached;

    public float randomShotTime = 3;

    public List<AudioClip> explodeAudio;
    public GameObject destroyParticle;

    public GameObject arrowPivot;

    private GameManager gameManager;
    private Transform projectileSpawn;
    private EZObjectPool objectPool;
    private NavMeshAgent nma;

	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        projectileSpawn = transform.GetChild(2).transform;
        objectPool = GameObject.Find("ObjectPool").GetComponent<EZObjectPool>();
        nma = GetComponent<NavMeshAgent>();

        StartCoroutine(Spawn());

        destination = new Vector3(Random.Range(-28, 28), 0, Random.Range(-22, 22));
	}

	void Update () {
        #region Arrow Pivot
        Vector3 relativePos = transform.position - arrowPivot.transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.forward);
        arrowPivot.transform.rotation = rotation;
        #endregion

        if (!hasReached)
        {
            nma.SetDestination(destination);

            if (Vector3.Distance(transform.position, destination) < 5)
            {
                hasReached = true;
            }
        }
        else if (hasReached)
        {
            RandomDestination();
        }
    }

    IEnumerator Spawn()
    {
        while (randomShotTime > 0)
        {
            randomShotTime -= Time.deltaTime;
            objectPool.TryGetNextObject(projectileSpawn.position, projectileSpawn.rotation);
            yield return new WaitForSeconds(3);
        }
    }

    void RandomDestination()
    {
        destination = new Vector3(Random.Range(-29, 29), 0, Random.Range(-23, 23));
        hasReached = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.popUpSize = 5;
            gameManager.comboCounter.transform.localScale = new Vector3(1, 1, 0);
            gameManager.recentKill = true;

            ++gameManager.comboKills;
            gameManager.comboTime = gameManager.maxComboTime;
            gameManager.comboCounter.text = gameManager.comboKills + " Combo";
            GameManager.gameScore += 10;
            --gameManager.unitAlive;

            Destroy(arrowPivot);

            CameraShaker.Instance.ShakeOnce(3, 3, 0, 2);

            GameObject temp = Instantiate(destroyParticle, transform.position, Quaternion.identity);
            AudioSource tempAS = temp.GetComponent<AudioSource>();

            tempAS.clip = explodeAudio[Random.Range(0,2)];
            tempAS.Play();
            tempAS.pitch = Random.Range(1, 3);

            Destroy(temp.transform.GetChild(0).gameObject, 1);
            Destroy(temp.transform.GetChild(1).gameObject, 1);
            Destroy(temp, 5);
            Destroy(gameObject);
        }
    }
}
