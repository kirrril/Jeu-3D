using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IHM_startGame : MonoBehaviour
{
    // public static IHM_startGame instance;

    // void Awake()
    // {
    //     instance = this;
    // }

    [SerializeField]
    private TMP_InputField nameInputField;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }

    public void FocusInputField()
    {
        nameInputField.text = "";
        nameInputField.Select();
        nameInputField.ActivateInputField();
    }

    public void GetInput()
    {   
        if (nameInputField.text != "")
        {
            UserHolder.instance.userProfile.userName = nameInputField.text;
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
