using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

    public float parallaxSpeed = 5;

	void Update () {
        transform.position += Vector3.left * Time.deltaTime * parallaxSpeed;

        if (transform.position.x < -400)
        {
            transform.position += new Vector3(600, 0, 0);
        }
	}
}
