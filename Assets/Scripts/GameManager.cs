using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;
    [HideInInspector] public bool isPlay{get; private set;} = false; //???????? ????? ??? ???

    [HideInInspector] public string currentTeam = "Blue"; //??????? ?? ??????? ?? ?????? 

    [Space(5)]
    [Header("Opposing teams")]
    [SerializeField] public string tagTeamOne = "Blue";
    [SerializeField] public string tagTeamTwo = "Red";

    [Space(5)]
    [Header("UI")]
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelGameWin;

    private bool isPause = false;


    public delegate void StartGameDelegate();
    public event StartGameDelegate startGameFite;
    public event StartGameDelegate gameOver;


    public GameObject[] teamBlue {get; private set;} = null;
    public GameObject[] teamRed {get; private set; } = null;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("GameMaster instance!!!");
        }
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            isPause = !isPause;
        }
    }
    public string SetTagForMob(int numTeam) //?????? ??? ???? ? ??????????? ?? ???? ????? ?? ?????? ???????
    {
        var tag = "";
        switch (numTeam)
        {
            case 0: {tag = tagTeamOne; break; }
            case 1: { tag = tagTeamTwo; break; }
            default: {tag = tagTeamOne; break; }
        }

        return tag;
    }
    public void StartGameFite() //?????? ????? (?? ??????? ?????? "??????")
    {
        isPlay = true;
        SearchTargetTeams();

        startGameFite(); //???????? ??????? ?????? ????
    }

    private void SearchTargetTeams()
    {
        teamBlue = GameObject.FindGameObjectsWithTag(tagTeamOne);
        teamRed = GameObject.FindGameObjectsWithTag(tagTeamTwo);
    }

    public void GameOver(string tagWinners)
    {
        isPlay = false;
        Debug.Log("------Game over. Win " +tagWinners);

      //  Time.timeScale = 0;
        if(tagWinners == tagTeamOne)
        {
            panelGameWin.SetActive(true);
        }
        else
        {
            panelGameOver.SetActive(true);
        }
        
        gameOver();
    }

}
