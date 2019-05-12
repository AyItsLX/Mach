using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPivot : MonoBehaviour {

    public Transform playerTrans;

	void Update () {
        transform.position = playerTrans.position;
    }
}
