using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Follow : MonoBehaviour {

    public float cameraSpeed = 5;

    public float zOffset = 9;
    public float xOffset = 10;

    private Transform playerObj;

	void Start () {
        if (playerObj == null)
            playerObj = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {
        if (playerObj.position.z < zOffset && playerObj.position.z > -zOffset)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, playerObj.position.z), Time.deltaTime * cameraSpeed);

        if (playerObj.position.x < xOffset && playerObj.position.x > -xOffset)
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerObj.position.x, transform.position.y, transform.position.z), Time.deltaTime * cameraSpeed);
    }
}
