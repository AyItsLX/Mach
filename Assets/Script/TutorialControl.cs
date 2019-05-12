using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour {

    [Header("Player Stats")]
    public float playerEnergy = 100;
    public float recoverSpeed = 20;
    public float moveSpeed = 5;
    public float rotSpeed = 200;

    [Header("Reference")]
    public float boostAlpha = .25f;
    public float particleAlpha = 1;
    public GameObject boostEffect;
    public Transform energyTrans;
    public MeshRenderer boostMaterial;
    public ParticleSystem speedParticle;
    public ParticleSystem boostParticle;
    public List<TrailRenderer> bodyTrails;

    private bool increaseBoost = false;
    private float boostTime = .25f;
    private float boostSpeed = .5f;

    private bool rightInput = false, leftInput = false;
    private bool rightTurn = false, leftTurn = false;

    private Rigidbody rb;
    private GameObject mainCamera;
    private GameObject meshBody;

    void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        meshBody = transform.GetChild(1).gameObject;
        rb = GetComponent<Rigidbody>();
    }
	
	void Update () {
        #region Map Locked
        if (transform.position.x >= 10.5f) {
            StartCoroutine(TemporaryDisable());
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            mainCamera.transform.position = new Vector3(-mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        else if (transform.position.x <= -10.5f) {
            StartCoroutine(TemporaryDisable());
            transform.position = new Vector3(Mathf.Abs(transform.position.x), transform.position.y, transform.position.z);
            mainCamera.transform.position = new Vector3(Mathf.Abs(mainCamera.transform.position.x), mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        else if (transform.position.z >= 6) {
            StartCoroutine(TemporaryDisable());
            transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -mainCamera.transform.position.z);
        }
        else if (transform.position.z <= -6) {
            StartCoroutine(TemporaryDisable());
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Abs(transform.position.z));
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, Mathf.Abs(mainCamera.transform.position.z));
        }
        #endregion

        #region Movement
        transform.position += transform.forward * Time.deltaTime * moveSpeed; // Auto movement player is unable to control forward

        float angle = meshBody.transform.localEulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;

        #region A & L Input
        if (!rightInput && !leftInput && Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.L)) // Double pressed input A & L for functions
        {
            rb.velocity = transform.forward * 5f; // Dash
            increaseBoost = true;

            if (playerEnergy > 5) {
                playerEnergy -= 5;
                SetBoost(.5f, .5f, 1, .01f); // Boost Method
            }
        }
        #endregion

        #region A Input
        if (!rightInput && Input.GetKey(KeyCode.A)) // Long pressed input A for functions
        {
            leftInput = true;
            leftTurn = true;
            rightTurn = false;

            transform.Rotate(Vector3.up * Time.deltaTime * -rotSpeed); // A for rotating left

            if (leftInput && Input.GetKeyDown(KeyCode.L)) {
                rb.velocity = transform.forward * 5f; // Dash
                increaseBoost = true;

                if (playerEnergy > 5) {
                    playerEnergy -= 5;
                    SetBoost(.1f, .5f, 1, .01f); // Boost Method
                }
            }
            if (angle < 70) {
                meshBody.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 100);
            }
        }
        else if (leftInput && !Input.GetKey(KeyCode.A)) {
            leftInput = false;
        }
        else if (leftTurn && !Input.GetKey(KeyCode.A) && angle > 1) {
            meshBody.transform.localEulerAngles -= new Vector3(0, 0, Time.deltaTime * 75);
            if (angle > 0 && angle < 5) {
                meshBody.transform.localEulerAngles = new Vector3(0, 0, 0);
                leftTurn = false;
            }
        }
        #endregion

        #region L Input
        if (!leftInput && Input.GetKey(KeyCode.L)) // Long pressed input L for functions
        {
            rightInput = true;
            rightTurn = true;
            leftTurn = false;

            transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed); // L for rotating left

            if (rightInput && Input.GetKeyDown(KeyCode.A)) {
                rb.velocity = transform.forward * 5f; // Dash
                increaseBoost = true;

                if (playerEnergy > 5) {
                    playerEnergy -= 5;
                    SetBoost(.1f, .5f, 1, .01f); // Boost Method
                }
            }
            if (angle > -70) {
                meshBody.transform.localEulerAngles -= new Vector3(0, 0, Time.deltaTime * 100);
            }
        }
        else if (rightInput && !Input.GetKey(KeyCode.L)) {
            rightInput = false;
        }
        else if (rightTurn && !Input.GetKey(KeyCode.L) && angle < -1) {
            meshBody.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 75);
            if (angle < 0 && angle > -5) {
                meshBody.transform.localEulerAngles = new Vector3(0, 0, 0);
                rightTurn = false;
            }
        }
        #endregion

        #endregion

        #region Boost and Default
        ParticleSystem.MainModule sPMain = speedParticle.main;
        ParticleSystem.MainModule bPMain = boostParticle.main;

        if (increaseBoost) {
            boostTime -= Time.deltaTime;

            if (boostAlpha < 1)
                boostAlpha += Time.deltaTime;

            if (particleAlpha < 1)
                particleAlpha += Time.deltaTime;

            boostMaterial.material.color = new Color(156, 253, 243, boostAlpha);
            sPMain.startColor = new Color(255, 255, 255, particleAlpha);
            bPMain.startColor = new Color(255, 255, 255, particleAlpha);

            boostEffect.transform.localScale += new Vector3(Time.deltaTime * boostSpeed * 0.25f, 0, Time.deltaTime * boostSpeed);

            if (boostTime < 0) {
                increaseBoost = false;
            }
        }
        else {
            SetDefault(.25f, 5, 200, .5f);

            if (boostAlpha > .1f)
                boostAlpha -= Time.deltaTime * .1f;

            if (particleAlpha > .1f)
                particleAlpha -= Time.deltaTime * .1f;

            boostMaterial.material.color = new Color(156, 253, 243, boostAlpha);
            sPMain.startColor = new Color(255, 255, 255, particleAlpha);
            bPMain.startColor = new Color(255, 255, 255, particleAlpha);

            if (playerEnergy < 100) {
                playerEnergy += Time.deltaTime * recoverSpeed;
            }

            if (boostEffect.transform.localScale.z > 1.5f) {
                boostEffect.transform.localScale -= new Vector3(0, 0, Time.deltaTime * boostSpeed * 1.5f);
                if (boostEffect.transform.localScale.z < 1.5f) {
                    boostEffect.transform.localScale = new Vector3(0.5f, 1, 1.5f);
                }
            }
            if (boostEffect.transform.localScale.x > 0.5f) {
                boostEffect.transform.localScale -= new Vector3(Time.deltaTime * boostSpeed * 0.5f, 0, 0);
                if (boostEffect.transform.localScale.x < 0.5f) {
                    boostEffect.transform.localScale = new Vector3(0.5f, 1, boostEffect.transform.localScale.z);
                }
            }
        }

        energyTrans.localScale = new Vector3(1, 1, playerEnergy / 100);
        #endregion
    }

    #region Disable Temporary
    IEnumerator TemporaryDisable() {
        bodyTrails[0].time = 0;
        bodyTrails[1].time = 0;
        yield return new WaitForSeconds(.05f);
        bodyTrails[0].time = 5;
        bodyTrails[1].time = 5;
    }
    #endregion

    #region Set Boost / Default
    void SetBoost(float boostTime, float moveSpeed, float rotSpeed, float boostSpeed) {
        this.boostTime += boostTime;
        this.moveSpeed += moveSpeed;
        this.rotSpeed += rotSpeed;
        this.boostSpeed += boostSpeed;
    }

    void SetDefault(float boostTime, float moveSpeed, float rotSpeed, float boostSpeed) {
        this.boostTime = boostTime;

        if (this.moveSpeed > moveSpeed) {
            this.moveSpeed -= Time.deltaTime * boostSpeed * 3;
        }
        if (this.rotSpeed > rotSpeed) {
            this.rotSpeed -= Time.deltaTime * boostSpeed * 3;
        }
        if (this.boostSpeed > boostSpeed) {
            this.boostSpeed -= Time.deltaTime * boostSpeed * 3;
        }
    }
    #endregion
}
