using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using System;

public class IHM_youWin : MonoBehaviour
{
    [SerializeField]
    TMP_Text message;

    [SerializeField]
    TMP_Text alphaText;

    Coroutine greetingMessageCorout;


    void Start()
    {
        DisplayGreetingMessage();

        alphaText.text = Convert.ToString(PersistantData.instance.data.alphaCoeff);
    }

    public void DisplayGreetingMessage()
    {
        greetingMessageCorout = StartCoroutine(GreetingMessage());
    }

    public IEnumerator GreetingMessage()
    {
        yield return new WaitForSeconds(0.2f);

        message.text = $"YOU";

        yield return new WaitForSeconds(1f);

        message.text = $"YOU\nDID";

        yield return new WaitForSeconds(0.5f);

        message.text = $"YOU\nDID IT";

        yield return new WaitForSeconds(1f);

        message.text = $"YOU\nDID IT\nBRO";

        yield return new WaitForSeconds(1f);

        message.text = $"YOU\nDID IT\nBRO.";

        yield return new WaitForSeconds(0.1f);

        message.text = $"YOU\nDID IT\nBRO..";

        yield return new WaitForSeconds(0.1f);

        message.text = $"YOU\nDID IT\nBRO...";

        yield return new WaitForSeconds(2f);

        message.text = "";

        yield return new WaitForSeconds(1f);

        message.text = $"BIG CONGRATS,\n{UserHolder.instance.userProfile.userName.ToUpper()}!";

        yield return new WaitForSeconds(2f);

        message.text = $"YOU'RE\nA REAL ALPHA";

        yield return new WaitForSeconds(0.5f);

        message.text = $"YOU'RE\nA REAL ALPHA,\nDARWIN'S CERTIFIED!";

        yield return new WaitForSeconds(2f);

        message.text = "";
    }







    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");

        Destroy(PersistantData.instance);
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
