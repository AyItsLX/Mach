using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int chosenLevel = 0;

    public static float gameScore;
    public static float gametime;
    public static float highestCombo;
    public bool recentKill = false;
    public bool lastUnit = false;

    public int noOfUnit = 50;
    public int noOfUnitSpawned;
    public int unitAlive;

    public float comboKills;
    public float highestComboKills;
    public float comboTime;
    public float maxComboTime = 10;
    public float popUpSize = 5;

    public GameObject enemy1;
    public Transform arrowPivot;
    public GameObject pivotPrefab;
    public GameObject enemyGroup;

    public TMPro.TMP_Text comboCounter;

    private bool playComboPopUpVFX = false;

    void Start()
    {
        enemyGroup = GameObject.Find("EnemyGroup");

        gameScore = 0;
        gametime = 0;
        highestCombo = 0;

        comboTime = maxComboTime;
        noOfUnitSpawned = noOfUnit;
        comboCounter.gameObject.SetActive(false);
    }

    void Update () {
        gametime = Time.timeSinceLevelLoad;

        if (noOfUnit > 0)
        {
            noOfUnit--;
            unitAlive++;
            GameObject temp = Instantiate(enemy1, new Vector3(Random.Range(-29, 29), 0, Random.Range(-23, 23)), Quaternion.identity, enemyGroup.transform);
            Ai_Movement tempScript = temp.GetComponent<Ai_Movement>();

            tempScript.arrowPivot = Instantiate(pivotPrefab, arrowPivot.position, arrowPivot.rotation, arrowPivot);
        }

        if (comboKills > 0)
        {
            comboCounter.gameObject.SetActive(true);
            comboTime -= Time.deltaTime;
            
            if (comboKills > highestComboKills)
            {
                highestComboKills = comboKills;
            }

            print("SSS Rank: " + noOfUnitSpawned * .75f);
            UpdateComboAndSwitch();

            if (comboTime <= 0)
            {
                comboCounter.gameObject.SetActive(false);
                comboTime = maxComboTime;
                comboKills = 0;
                comboCounter.text = comboKills + " Combo";
            }
        }

        if (recentKill)
        {
            if (comboKills % 10 == 0) {
                playComboPopUpVFX = true;
            }

            if (playComboPopUpVFX) {
                ComboPopUpVfx();
            }
        }
        else
        {
            popUpSize = 5;
            comboCounter.transform.localScale = new Vector3(1, 1, 0);
        }
	}

    void UpdateComboAndSwitch()
    {
        if (!lastUnit && unitAlive <= 0)
        {
            if (highestComboKills >= noOfUnitSpawned * .75f)
                highestCombo = 6;
            else if (highestComboKills >= noOfUnitSpawned * .5f)
                highestCombo = 5;
            else if (highestComboKills >= noOfUnitSpawned * .25f)
                highestCombo = 4;
            else if (highestComboKills >= noOfUnitSpawned * .1f)
                highestCombo = 3;
            else if (highestComboKills >= noOfUnitSpawned * .05f)
                highestCombo = 2;

            print("HighestCombo : " + highestCombo);

            lastUnit = true;
        }

        if (lastUnit)
        {
            lastUnit = false;

            SceneManager.LoadScene("WinScreen");
        }
    }

    public void UpdateComboThenSwitch()
    {
        if (highestComboKills >= noOfUnitSpawned * .75f)
            highestCombo = 6;
        else if (highestComboKills >= noOfUnitSpawned * .5f)
            highestCombo = 5;
        else if (highestComboKills >= noOfUnitSpawned * .25f)
            highestCombo = 4;
        else if (highestComboKills >= noOfUnitSpawned * .1f)
            highestCombo = 3;
        else if (highestComboKills >= noOfUnitSpawned * .05f)
            highestCombo = 2;

        print("HighestCombo : " + highestCombo);

        SceneManager.LoadScene("WinScreen");
    }

    void ComboPopUpVfx() {
        if (popUpSize >= 1)
            popUpSize -= Time.deltaTime * 10;

        popUpSize = Mathf.Clamp(popUpSize, .5f, 5);

        comboCounter.transform.localScale = new Vector3(popUpSize, popUpSize, 0);

        if (popUpSize <= 1) {
            playComboPopUpVFX = false; 
            recentKill = false;
            popUpSize = 5;
            comboCounter.transform.localScale = new Vector3(1, 1, 0);
        }
    }
}
