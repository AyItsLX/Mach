using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSystem : MonoBehaviour {

    public static bool isInScore = true;

    public int curLevel = 0;
    public static string captainCallSign;
    private int curButton;

    [Header("Stats")]
    public float finalTime;
    public float finalCombo = 1;
    public float finalScore;
    public float calculatedScore;
    private float m, s;
    private float curM, curS;
    private float curCombo;
    private float curScore;
    private float lerpSpeed = 1;

    public TMP_Text timeCounter;
    public TMP_Text comboCounter;
    public TMP_Text scoreCounter;

    public GameObject scoreUI;
    public GameObject levelUI;

    public List<Image> buttonGlow;
    public List<AudioSource> buttonAudio;
    public List<Image> buttonStack;
    public List<AudioSource> audioStack;

    private float glowSpeed;
    private float glowRate = 0;

    private float selectTime;
    private bool playOnce = true;

    #region Start
    void Start () {
        if (isInScore)
        {
            finalTime = GameManager.gametime;
            finalCombo = GameManager.highestCombo;
            finalScore = GameManager.gameScore;

            curScore = 0;

            UpdateAttribute();
            scoreUI.SetActive(true);
            levelUI.SetActive(false);
        }
        else if (!isInScore)
        {
            UpdateAttribute();
            scoreUI.SetActive(false);
            levelUI.SetActive(true);
        }
    }
    #endregion

    void Update () {
        if (isInScore)
            CalculateScore();

        #region Input
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.L))
        {
            if (curButton == -1)
                curButton = 0;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (curButton == 0)
                curButton = 1;
            else
                --curButton;

            if (curButton == 0)
                buttonAudio[0].Play();
            if (curButton == 1)
                buttonAudio[1].Play();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (curButton == 1)
                curButton = 0;
            else
                ++curButton;

            if (curButton == 0)
                buttonAudio[0].Play();
            if (curButton == 1)
                buttonAudio[1].Play();
        }
        #endregion

        #region Disable Button
        if (curButton != 0)
        {
            StopGlow(buttonGlow[0]);
        }
        else if (curButton != 1)
        {
            StopGlow(buttonGlow[1]);
        }
        #endregion

        #region Enable Button
        if (curButton == 0)
        {
            PlayGlow(buttonGlow[0]);
            if (isInScore)
                OnLevelsPressed();
            else if (!isInScore)
                OnMenuPressed();
        }
        else if (curButton == 1)
        {
            PlayGlow(buttonGlow[1]);

            if (isInScore)
                OnNextPressed();
            else if (!isInScore)
                OnQuitPressed();
        }
        #endregion
    }

    #region Calculate Score
    void CalculateScore() {

        calculatedScore = finalScore / finalTime * finalCombo * 100;
        curM = Mathf.FloorToInt(finalTime / 60f);
        curS = Mathf.FloorToInt(finalTime - m * 60);
        curS = Mathf.Clamp(curS, 0, Mathf.FloorToInt(finalTime - Mathf.FloorToInt(finalTime / 60f) * 60));

        if (m < curM)
            m = Mathf.Lerp(m, curM, Time.deltaTime * .5f);
        if (m >= curM - .5f && finalTime % 60 != 0)
            s = Mathf.Lerp(s, curS, Time.deltaTime * lerpSpeed);

        timeCounter.text = m.ToString("00") + ":" + s.ToString("00");

        if (s >= curS - 5)
        {
            curScore = Mathf.Lerp(curScore, calculatedScore, Time.deltaTime * 1.5f);
            scoreCounter.text = curScore.ToString("0000");
        }

        curCombo = finalCombo;

        if (curCombo == 1)
            comboCounter.text = "C";
        else if (curCombo == 2)
            comboCounter.text = "B";
        else if (curCombo == 3)
            comboCounter.text = "A";
        else if (curCombo == 4)
            comboCounter.text = "S";
        else if (curCombo == 5)
            comboCounter.text = "SS";
        else if (curCombo == 6)
            comboCounter.text = "SSS";
    }
    #endregion

    #region Update Attribute
    void UpdateAttribute()
    {
        if (isInScore)
        {
            buttonGlow[0] = buttonStack[0];
            buttonGlow[1] = buttonStack[1];
            buttonGlow[2] = buttonStack[2];
            buttonGlow[3] = buttonStack[3];

            buttonAudio[0] = audioStack[0];
            buttonAudio[1] = audioStack[1];
            buttonAudio[2] = audioStack[2];
        }
        else if (!isInScore)
        {
            buttonGlow[0] = buttonStack[4];
            buttonGlow[1] = buttonStack[5];
            buttonGlow[2] = buttonStack[6];
            buttonGlow[3] = buttonStack[7];

            buttonAudio[0] = audioStack[3];
            buttonAudio[1] = audioStack[4];
            buttonAudio[2] = audioStack[5];
        }
    }
    #endregion

    #region Pressed Methods
    void OnLevelsPressed() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
        {
            buttonGlow[2].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true)
            {
                buttonAudio[2].Play();
                playOnce = false;
            }

            buttonGlow[2].fillAmount = selectTime / 3;

            if (selectTime >= 3)
            {
                buttonGlow[2].enabled = false;

                isInScore = false;
                UpdateAttribute();
                curButton = -1;

                Highscores.AddNewHighscore(captainCallSign, Mathf.RoundToInt(calculatedScore));

                scoreUI.SetActive(false);
                levelUI.SetActive(true); // Turn on Level UI.
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L))
        {
            playOnce = true;
            selectTime = 0;
            buttonGlow[2].enabled = false;
        }
    }

    void OnNextPressed() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
        {
            buttonGlow[3].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true)
            {
                buttonAudio[2].Play();
                playOnce = false;
            }

            buttonGlow[3].fillAmount = selectTime / 3;

            if (selectTime >= 3)
            {
                buttonGlow[3].enabled = false;

                isInScore = true; // Reset static Variables.

                //if (curLevel == 0) // Switch to gameplay scene and activate next level.
                //    GameManager.chosenLevel = curLevel;
                //else if (curLevel == 1)
                //    GameManager.chosenLevel = curLevel;
                //else if (curLevel == 2)
                //    GameManager.chosenLevel = curLevel;

                SceneManager.LoadScene("Lv1");
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L))
        {
            playOnce = true;
            selectTime = 0;
            buttonGlow[3].enabled = false;
        }
    }

    void OnMenuPressed() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
        {
            buttonGlow[2].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true)
            {
                buttonAudio[2].Play();
                playOnce = false;
            }

            buttonGlow[2].fillAmount = selectTime / 3;

            if (selectTime >= 3)
            {
                buttonGlow[2].enabled = false;

                isInScore = true; // Reset static Variables.
                SceneManager.LoadScene("Menu");
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L))
        {
            playOnce = true;
            selectTime = 0;
            buttonGlow[2].enabled = false;
        }
    }

    void OnQuitPressed() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
        {
            buttonGlow[3].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true)
            {
                buttonAudio[2].Play();
                playOnce = false;
            }

            buttonGlow[3].fillAmount = selectTime / 3;

            if (selectTime >= 3)
            {
                buttonGlow[3].enabled = false;

                print("Application has quitted in Level Menu");

                Application.Quit();
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L))
        {
            playOnce = true;
            selectTime = 0;
            buttonGlow[3].enabled = false;
        }
    }
    #endregion

    #region Glow
    void PlayGlow(Image glowSprite)
    {
        glowRate = Mathf.PingPong(Time.time * 3, 1);

        glowSprite.color = new Color(glowRate, glowRate, glowRate, glowRate);
    }

    void StopGlow(Image glowSprite)
    {
        if (glowSprite.color.r >= 0)
        {
            glowSprite.color -= new Color(Time.deltaTime, Time.deltaTime, Time.deltaTime, Time.deltaTime);
        }
    }
    #endregion
}
