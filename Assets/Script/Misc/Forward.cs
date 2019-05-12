using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : BulletMovement {

    void Update () {
        transform.position += transform.forward * Time.deltaTime * 7.5f;
	}
}
