using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IHM_StartGame : MonoBehaviour
{
    public static string playersName;

    [SerializeField]
    private TMP_InputField nameInputField;

    [SerializeField]
    private Button startGameButton;

    void Start()
    {
        FocusInputField();
    }

    public void FocusInputField()
    {
        nameInputField.text = "";
        nameInputField.Select();
        nameInputField.ActivateInputField();
    }

    public void GetInput()
    {
        playersName = nameInputField.text;

        Debug.Log($"{playersName}");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Levels");
    }
}
