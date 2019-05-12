using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Movement : MonoBehaviour {

    public float bulletSpeed;

	void Start () {}

	void Update () {
        transform.position -= transform.forward * Time.deltaTime * bulletSpeed;
	}
}
