using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour {

    public enum Behaviour {
        Forward,
        RandomForward,
        ForwardCW,
    }

    public Behaviour behaviour;

    void Update()
    {
        if (behaviour == Behaviour.ForwardCW)
        {
            transform.eulerAngles += new Vector3(0, Time.deltaTime * 75, 0);
        }
        else if (behaviour == Behaviour.RandomForward)
        {
            transform.eulerAngles = new Vector3(0, Random.Range(160,200), 0);
        }
    }
}
