using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player currentPlayer;

    public float treadmillTraining;
    public float bikeTraining;
    public float jumpboxTraining;
    public float barbellTraining;
    public float chest1Training;
    public float chest2Training;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        LaunchGame();
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        TrainingManagement();

        LifeManagement();

        if (Input.GetKey(KeyCode.Escape))
        {
            YouWin();
        }
    }


    void LaunchGame()
    {
        currentPlayer = new Player(UserHolder.instance.userProfile.userName);
    }

    public void LifeManagement()
    {
        if (currentPlayer.life < 1) YouLoose();
    }

    public void TrainingManagement()
    {
        currentPlayer.legsTraining = treadmillTraining + bikeTraining + jumpboxTraining;
        // currentPlayer.chestTraining = barbellTraining + chest1Training + chest2Training;
    }

    public void YouWin()
    {
        StartCoroutine(YouWinTransitionCorout());
    }

    IEnumerator YouWinTransitionCorout()
    {
        PersistantData.instance.GetData();

        StartCoroutine(IHM.instance.ColorGradientCorout());

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("YouWin");
    }

    public void YouLoose()
    {
        Destroy(instance);

        SceneManager.LoadScene("YouLose");
    }
}
