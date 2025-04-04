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

        // currentPlayer = new Player("Kirill");
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
        PersistantData.instance.GetData();

        SceneManager.LoadScene("YouWin");
    }

    public void YouLoose()
    {
        SceneManager.LoadScene("YouLose");
    }
}
