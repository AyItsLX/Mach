using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHitEffect : MonoBehaviour {

    public float fadeNum = 100;
    public float rValue = 255;
    public float gValue = 58;
    public float bValue = 0;

    public Color defaultColor;

    public bool fadeOnce = false;
    private bool fadeHealth = false;
    private bool setRed = false;

    public Image onHitFade;
    public Image healthUI;

	void Update () {
        if (fadeOnce)
        {
            fadeHealth = true;

            fadeNum -= Time.deltaTime * 50;
            onHitFade.color = new Color(1, 0, 0, fadeNum / 100);

            if (fadeNum <= 0)
            {
                fadeOnce = false;
                fadeNum = 100;
            }
        }

        if (fadeHealth)
        {
            if (setRed)
            {
                setRed = false;
                healthUI.color = new Color(255, 58, 0);
            }

            if (rValue > 126)
                rValue -= Time.deltaTime * 100;
            if (gValue < 255)
                gValue += Time.deltaTime * 100;
            if (bValue < 112)
                bValue += Time.deltaTime * 100;

            healthUI.color = new Color(rValue / 255, gValue / 255, bValue / 255);

            if (rValue <= 126 && gValue >= 255 && bValue >= 112)
            {
                fadeHealth = false;
                setRed = true;
                healthUI.color = defaultColor;
            }
        }
    }

    public void ResetValue()
    {
        fadeOnce = false;
        fadeNum = 100;

        fadeHealth = false;
        setRed = true;
        rValue = 255;
        gValue = 58;
        bValue = 0;
        healthUI.color = new Color(255, 58, 0);

        fadeOnce = true;
    }
}
