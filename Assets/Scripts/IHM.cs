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
    public TMP_Text leg;

    [SerializeField]
    public TMP_Text chest;

    [SerializeField]
    public TMP_Text back;

    [SerializeField]
    public TMP_Text youWin;

    [SerializeField]
    public TMP_Text zone1;

    [SerializeField]
    public TMP_Text zone2;

    [SerializeField]
    public TMP_Text zone3;

    [SerializeField]
    public TMP_Text zone4;

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
    public string contextMessageCoroutName = "";

    [SerializeField]
    private Image colorGradient;
    private float colorGradientDuration = 2f;
    private Material colorGradientMaterial;

    [SerializeField]
    private Image thirstyGradient;
    private float thirstyGradientDuration = 3f;
    private Material thirstyGradientMaterial;

    private int loopCount = 2;

    public List<Sprite> remainingLives;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DisplayData();
        DisplayWelcomeMessage();

        blackOutPanel.enabled = false;

        colorGradientMaterial = colorGradient.material;

        thirstyGradientMaterial = thirstyGradient.material;
    }


    void Update()
    {
        DisplayData();
        ManageRoomCanvases();
    }


    public void ManageRoomCanvases()
    {
        if (GameManager.instance.currentPlayer.level == 1)
        {
            leg.color = Color.white;
            leg.color = Color.white;
            chest.color = new Color(0.8f, 0.8f, 0.8f);
            zone2.color = new Color(0.8f, 0.8f, 0.8f);
            back.color = new Color(0.8f, 0.8f, 0.8f);
            zone3.color = new Color(0.8f, 0.8f, 0.8f);
            youWin.color = new Color(0.8f, 0.8f, 0.8f);
            zone4.color = new Color(0.8f, 0.8f, 0.8f);
            legCanvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (GameManager.instance.currentPlayer.level == 2)
        {
            leg.color = new Color(0.8f, 0.8f, 0.8f);
            zone1.color = new Color(0.8f, 0.8f, 0.8f);
            chest.color = Color.white;
            zone2.color = Color.white;
            back.color = new Color(0.8f, 0.8f, 0.8f);
            zone3.color = new Color(0.8f, 0.8f, 0.8f);
            youWin.color = new Color(0.8f, 0.8f, 0.8f);
            zone4.color = new Color(0.8f, 0.8f, 0.8f);
            legCanvas.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (GameManager.instance.currentPlayer.level == 3)
        {
            leg.color = new Color(0.8f, 0.8f, 0.8f);
            zone1.color = new Color(0.8f, 0.8f, 0.8f);
            chest.color = new Color(0.8f, 0.8f, 0.8f);
            zone2.color = new Color(0.8f, 0.8f, 0.8f);
            back.color = Color.white;
            zone3.color = Color.white;
            youWin.color = new Color(0.8f, 0.8f, 0.8f);
            zone4.color = new Color(0.8f, 0.8f, 0.8f);
            legCanvas.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void DisplayData()
    {
        waterImage.fillAmount = GameManager.instance.currentPlayer.water;
        legsImage.fillAmount = GameManager.instance.currentPlayer.legsTraining / 1.05f;
        chestImage.fillAmount = GameManager.instance.currentPlayer.chestTraining / 1.05f;
        backImage.fillAmount = GameManager.instance.currentPlayer.backTraining / 1.05f;
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
        contextMessageCoroutName = "WaterWarning";
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
        blackOutPanel.enabled = true;
        StartCoroutine(FadeCoroutine(0f, 1f, 1.8f));
    }

    public void FadeOut()
    {
        blackOutPanel.enabled = false;
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


    public IEnumerator ColorGradientCorout()
    {
        colorGradient.gameObject.SetActive(true);

        for (int i = 0; i < loopCount; i++)
        {
            float elapsedTime = 0f;
            float startOffset = 1f;
            float endOffset = -1f;

            while (elapsedTime < colorGradientDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / colorGradientDuration;
                float currentOffset = Mathf.Lerp(startOffset, endOffset, t);
                colorGradientMaterial.SetFloat("_Offset", currentOffset);
                yield return null;
            }
        }
    }


    public IEnumerator ThirstyDeathCorout()
    {
        thirstyGradient.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float startOffset = 1f;
        float endOffset = -1f;

        while (elapsedTime < colorGradientDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / thirstyGradientDuration;
            float currentOffset = Mathf.Lerp(startOffset, endOffset, t);
            thirstyGradientMaterial.SetFloat("_Offset", currentOffset);
            yield return null;
        }
        yield return new WaitForSeconds(2f);

        thirstyGradient.gameObject.SetActive(false);
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