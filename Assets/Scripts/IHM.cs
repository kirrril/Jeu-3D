using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IHM : MonoBehaviour
{
    public static IHM instance;

    [SerializeField]
    public TMP_Text proteinAmount;

    [SerializeField]
    public TMP_Text defeatesEnemies;

    [SerializeField]
    public TMP_Text alphaCoeff;

    [SerializeField]
    public Button stopTrainingButton;

    [SerializeField]
    public Image lifeImage;

    [SerializeField]
    public Image waterImage;

    [SerializeField]
    public Image legsImage;

    [SerializeField]
    public Image chestImage;

    [SerializeField]
    public Image backImage;

    [SerializeField]
    public TMP_Text mainMessage;

    [SerializeField]
    public TMP_Text contextMessage;

    [SerializeField]
    public Image blackOutPanel;

    [SerializeField]
    private GameObject legCanvas;

    public Coroutine mainMessageCorout;

    public Coroutine contextMessageCorout;





    public List<Sprite> remainingLives;




    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DisplayData();
        DisplayWelcomeMessage();
    }


    void Update()
    {
        DisplayData();
    }


    public void FlipLegCanvas()
    {
        legCanvas.transform.Rotate(0, 180, 0);
    }

    public void DisplayData()
    {
        waterImage.fillAmount = GameManager.instance.currentPlayer.water;
        legsImage.fillAmount = GameManager.instance.currentPlayer.legsTraining;
        chestImage.fillAmount = GameManager.instance.currentPlayer.chestTraining;
        backImage.fillAmount = GameManager.instance.currentPlayer.backTraining;
        lifeImage.sprite = remainingLives[GameManager.instance.currentPlayer.life];
        proteinAmount.text = Convert.ToString(GameManager.instance.currentPlayer.protein);
        defeatesEnemies.text = Convert.ToString(GameManager.instance.currentPlayer.defeatedEnemies);
        alphaCoeff.text = Convert.ToString(PersistantData.instance.data.alphaCoeff);
    }


    public void DisplayWelcomeMessage()
    {
        contextMessageCorout = StartCoroutine(WelcomeMessage());
    }

    IEnumerator WelcomeMessage()
    {
        mainMessage.text = $"GO!";

        yield return new WaitForSeconds(0.5f);

        mainMessage.text = $"GO!\nGO!";

        yield return new WaitForSeconds(0.5f);

        mainMessage.text = $"GO!\nGO!\nGO {GameManager.instance.currentPlayer.playersName.ToUpper()}!";

        yield return new WaitForSeconds(0.5f);

        mainMessage.text = "";
    }


    public void DisplayWaterWarning()
    {
        contextMessageCorout = StartCoroutine(WaterWarning());
    }

    public IEnumerator WaterWarning()
    {
        contextMessage.text = $"WATCH THE WATER LEVEL";

        yield return new WaitForSeconds(1f);

        contextMessage.text = "";
    }


    public void DisplayDefeatMessage()
    {
        mainMessageCorout = StartCoroutine(DefeatMessage());
    }

    public IEnumerator DefeatMessage()
    {
        mainMessage.text = $"TAKE IT EASY {GameManager.instance.currentPlayer.playersName.ToUpper()}";

        yield return new WaitForSeconds(2f);

        mainMessage.text = $"KEEP TRAINING";

        yield return new WaitForSeconds(2f);

        mainMessage.text = $"YOU WILL HIT THEM BACK";

        yield return new WaitForSeconds(2f);

        mainMessage.text = "";
    }


    public void DisplayVictoryMessage()
    {
        mainMessageCorout = StartCoroutine(VictoryMessage());
    }

    public IEnumerator VictoryMessage()
    {
        mainMessage.text = $"YEAAAAH!!!";

        yield return new WaitForSeconds(2f);

        mainMessage.text = $"KICK THEIR ASSES";

        yield return new WaitForSeconds(2f);

        mainMessage.text = $"BEAT THEM UP";

        yield return new WaitForSeconds(2f);

        mainMessage.text = "";
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeCoroutine(0f, 1f, 1.8f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(1f, 0f, 1f));
    }

    IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = blackOutPanel.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            blackOutPanel.color = color;
            yield return null;
        }
        color.a = endAlpha;
        blackOutPanel.color = color;
    }


    public void Quit()
    {
#if UNITY_EDITOR

        EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        
        Application.Quit();

#endif
    }
}