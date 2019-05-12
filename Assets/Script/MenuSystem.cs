using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour {

    public bool isInTutorial = true;
    public bool isChoosingTag = false;
    public bool letterSelected = false;
    public int curLetter = 65;
    public int curButton;
    public int curButton1;

    public TMPro.TMP_Text captainTag;
    public List<TMPro.TMP_Text> buttonLetter;
    public List<Image> letterButton;
    public List<AudioSource> letterAudio;

    public List<Image> buttonGlow;
    public List<AudioSource> buttonAudio;
    public GameObject creditOverlay;

    public GameObject gameTitle;
    public GameObject openingUI;
    public GameObject menuUI;
    public GameObject choosingTagUI;

    private float glowSpeed;
    private float glowRate = 0;

    private float selectTime;
    private bool playOnce = true;
    private bool recentlySelected = false;

    void Start() {
        isChoosingTag = false;

        gameTitle.SetActive(false);
        openingUI.SetActive(true);
        menuUI.SetActive(false);
        choosingTagUI.SetActive(false);
    }

    void Update() {
        if (isInTutorial) {
            if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.L)) {
                isInTutorial = false;
                gameTitle.SetActive(true);
                openingUI.SetActive(false);
                menuUI.SetActive(true);
                choosingTagUI.SetActive(false);
            }
        }
        else if (isChoosingTag && !isInTutorial)
        {
            #region Input
            if (!letterSelected) {
                if (Input.GetKeyDown(KeyCode.A)) {
                    if (curButton1 == 0)
                        curButton1 = 3;
                    else
                        --curButton1;

                    if (curButton1 == 0)
                        letterAudio[0].Play();
                    if (curButton1 == 1)
                        letterAudio[1].Play();
                    if (curButton1 == 2)
                        letterAudio[2].Play();
                    if (curButton1 == 3)
                        letterAudio[3].Play();
                }
                else if (Input.GetKeyDown(KeyCode.L)) {
                    if (curButton1 == 3)
                        curButton1 = 0;
                    else
                        ++curButton1;

                    if (curButton1 == 0)
                        letterAudio[0].Play();
                    if (curButton1 == 1)
                        letterAudio[1].Play();
                    if (curButton1 == 2)
                        letterAudio[2].Play();
                    if (curButton1 == 3)
                        letterAudio[3].Play();
                }
            }
            #endregion

            #region Disable Button
            if (curButton1 != 0)
            {
                StopGlow(letterButton[0]);
            }
            if (curButton1 != 1)
            {
                StopGlow(letterButton[1]);
            }
            if (curButton1 != 2)
            {
                StopGlow(letterButton[2]);
            }
            if (curButton1 != 3)
            {
                StopGlow(letterButton[3]);
            }
            #endregion

            #region Enable Button
            if (curButton1 == 0) {
                PlayGlow(letterButton[0]);
                LetterSelected(letterButton[4]);
                ChooseLetter(buttonLetter[0]);
            }
            else if (curButton1 == 1) {
                PlayGlow(letterButton[1]);
                LetterSelected(letterButton[5]);
                ChooseLetter(buttonLetter[1]);
            }
            else if (curButton1 == 2) {
                PlayGlow(letterButton[2]);
                LetterSelected(letterButton[6]);
                ChooseLetter(buttonLetter[2]);
            }
            else if (curButton1 == 3) {
                PlayGlow(letterButton[3]);
                StartGame();
            }
            #endregion

            LevelSystem.captainCallSign = captainTag.text;
        }
        else
        {
            #region Input
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (curButton == 0)
                    curButton = 2;
                else
                    --curButton;

                if (curButton == 0)
                    buttonAudio[0].Play();
                if (curButton == 1)
                    buttonAudio[1].Play();
                if (curButton == 2)
                    buttonAudio[2].Play();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                if (curButton == 2)
                    curButton = 0;
                else
                    ++curButton;

                if (curButton == 0)
                    buttonAudio[0].Play();
                if (curButton == 1)
                    buttonAudio[1].Play();
                if (curButton == 2)
                    buttonAudio[2].Play();
            }
            #endregion

            #region Disable Button
            if (curButton != 0)
            {
                StopGlow(buttonGlow[0]);
            }
            if (curButton != 1)
            {
                StopGlow(buttonGlow[1]);
            }
            if (curButton != 2)
            {
                StopGlow(buttonGlow[2]);
            }
            #endregion

            #region Enable Button
            if (curButton == 0)
            {
                PlayGlow(buttonGlow[0]);
                OnStartPressed();
            }
            else if (curButton == 1)
            {
                PlayGlow(buttonGlow[1]);
                OnCreditPreview();
            }
            else if (curButton == 2)
            {
                PlayGlow(buttonGlow[2]);
                OnQuitPressed();
            }
            #endregion
        }
    }

    #region Captain Tag Method
    void ChooseLetter(TMPro.TMP_Text letter) {
        if (letterSelected) {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (curLetter == 65)
                    curLetter = 90;
                else
                    --curLetter;
            }
            else if (Input.GetKeyDown(KeyCode.L)) {
                if (curLetter == 90)
                    curLetter = 65;
                else
                    ++curLetter;
            }

            letter.text = System.Convert.ToChar(curLetter).ToString();
            captainTag.text = buttonLetter[0].text + buttonLetter[1].text + buttonLetter[2].text;
        }
    }

    void LetterSelected(Image letterGlow) {
        if (!recentlySelected && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L)) {
            letterGlow.enabled = true;

            selectTime += Time.deltaTime * 4;

            if (playOnce == true) {
                letterAudio[4].Play();
                playOnce = false;
            }

            letterGlow.fillAmount = selectTime / 3;

            if (selectTime >= 3) {
                recentlySelected = true;
                selectTime = 0;
                letterGlow.enabled = false;

                if (!letterSelected) {
                    letterSelected = true;
                }
                else if (letterSelected) {
                    letterSelected = false;
                }
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L)) {
            recentlySelected = false;
            playOnce = true;
            selectTime = 0;
            letterGlow.enabled = false;
        }
    }

    void StartGame() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L)) {
            letterButton[7].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true) {
                letterAudio[4].Play();
                playOnce = false;
            }

            letterButton[7].fillAmount = selectTime / 3;

            if (selectTime >= 3) {
                letterButton[7].enabled = false;

                SceneManager.LoadScene("Lv1");
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L)) {
            playOnce = true;
            selectTime = 0;
            letterButton[7].enabled = false;
        }
    }
    #endregion

    #region Pressed Methods
    void OnStartPressed() {
        creditOverlay.SetActive(false);

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
        {
            buttonGlow[3].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true)
            {
                buttonAudio[3].Play();
                playOnce = false;
            }

            buttonGlow[3].fillAmount = selectTime / 3;

            if (selectTime >= 3)
            {
                buttonGlow[3].enabled = false;
                isChoosingTag = true;
                selectTime = 0;
                menuUI.SetActive(false);
                choosingTagUI.SetActive(true);
                //SceneManager.LoadScene("Lv1");
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L))
        {
            playOnce = true;
            selectTime = 0;
            buttonGlow[3].enabled = false;
        }
    }

    void OnCreditPreview()
    {
        creditOverlay.SetActive(true);
    }

    void OnQuitPressed()
    {
        creditOverlay.SetActive(false);
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
        {
            buttonGlow[5].enabled = true;

            selectTime += Time.deltaTime * 2;

            if (playOnce == true)
            {
                buttonAudio[3].Play();
                playOnce = false;
            }

            buttonGlow[5].fillAmount = selectTime / 3;

            if (selectTime >= 3)
            {
                buttonGlow[5].enabled = false;

                print("Application has quitted in Start Menu");

                Application.Quit();
            }
        }
        else if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.L))
        {
            playOnce = true;
            selectTime = 0;
            buttonGlow[5].enabled = false;
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
