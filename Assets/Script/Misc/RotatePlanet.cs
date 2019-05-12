using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour {
    
	void Update () {
        transform.Rotate(new Vector3(.5f, .5f, 0) * Time.deltaTime * 2);
	}
}
