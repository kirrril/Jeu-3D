using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IHM_StartGame : MonoBehaviour
{

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
        UserHolder.instance.userProfile.userName = nameInputField.text;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
