using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress lP = null;

    void Awake()
    {
        if (lP == null)
        {
            lP = this;
            DontDestroyOnLoad(this);
        }
        else if (this != lP)
        {
            Destroy(gameObject);
        }
    }

    [Header("Images")]
    public Image stageOneFillImage;
    public Image stageTwoFillImage;

    [Header("Texts")]
    public Text currentLevel;
    public Text nextLevel;

    private void Start()
    {
        stageOneFillImage.fillAmount = 0f;
        stageTwoFillImage.fillAmount = 0f;
    }

    // This function fills level progress images in order to the current level.
    // "fill" parameter comes from "GameManager" script, "CalculateFillAmount()" function
    public void FillImage(float fill)
    {
        if (Game.firstStage)
        {
            stageOneFillImage.fillAmount = fill;
        }else if (Game.secondStage)
        {
            stageOneFillImage.fillAmount = 1;
            stageTwoFillImage.fillAmount = fill;
        }
        else
        {
            stageOneFillImage.fillAmount = fill;
            stageTwoFillImage.fillAmount = fill;
        }
    }

    public void SetLevelTexts()
    {
        currentLevel.text =""+(Game.level + 1);
        nextLevel.text = "" + (Game.level + 2);
    }
}
