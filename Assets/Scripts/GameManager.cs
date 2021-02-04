using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game parameters")]
    [Space(10)]
    [Header ("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerInitPos;
    [SerializeField] private GameObject environmentPrefab;
    //[SerializeField] private GameObject staticObjectsPrefab;

    public GameObject BackgroundObject;
    private UI_Manager uiManager;
    [SerializeField] private GameObject uiManagerPrefab;
    
    
    [Header("Player parameters")]
    public float playerInitSpeed;
    public float playerSpeedIncremetPerSec;
    public float playerJumpForce;
    public float playerMaxJumpDuration;
    

   
    [Header("Environment parameters")]
    public float platformMinXDistance;
    public float platformMaxXDistance;
    public float platformMaxYDistance;
    public float platformMinYPosition;
    public float platformMaxYPosition;
    public float platformMinLength;
    public float platformMaxLength;
    public float regularPlatformChance;
    public float greenPlatformChance;
    public float redPlatformChance;
    public float blackPlatformChance;
    public float greenPlatformJumpMultiplier;
    public float redPlatformJumpMultiplier;
    public float blackPlatformTimeDuration;
    
    [Header("Background parameters")]
    public float backgroundSpeed;
    
    //score
    private int platformsJumped = 0;
    private float timePassed;

    [SerializeField] private float topScore = 0f;

    [HideInInspector] public List<float> platformTypesProbs = new List<float>();
    [SerializeField] private EnvironmentController envController;
    [SerializeField] private Player_Controller player;
    [SerializeField] public Transform platformsSpawnPoint;

    //[SerializeField] private Text platformsText;
    [SerializeField] private Text timeText;
    
    private void Awake()
    {
        LoadPlatformProbs();
        //LoadPlayerAndEnvironment();
        ResetScene();

    }
    
    private void Start() {
        
        CreateInitialObjects();
    }
    private void LoadPlatformProbs()
    {
        platformTypesProbs.Add(regularPlatformChance);
        platformTypesProbs.Add(greenPlatformChance);
        platformTypesProbs.Add(redPlatformChance);
        platformTypesProbs.Add(blackPlatformChance);
    }
    
    private void CreateInitialObjects()
    {
        uiManager = Instantiate(uiManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Manager>();
        //Instantiate(staticObjectsPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            player.StartRunning();
            envController.StartMoving();
            uiManager.HideGameOverPanel();
            uiManager.HideStartLabel();
        }

        //KeepTimeScore();
        
    }

    private void KeepTimeScore()
    {
        Debug.Log("1");
        if (envController.isMoving)
        {
            timePassed += Time.deltaTime;
            uiManager.UpdateTime(timePassed);
        }
    }

    private void LoadPlayerAndEnvironment()
    {
        player = FindObjectOfType<Player_Controller>();
        envController = FindObjectOfType<EnvironmentController>();
        /*
        if (envController == null)
        {
        envController = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity).GetComponent<EnvironmentController>();  
        }
        */
    }

    public void addPlatformToScore()
    {
        platformsJumped++;
        //platformsText.text = platformsJumped.ToString();
    }

    public void GameOver()
    {
        
        if (timePassed > topScore)
        {
            topScore = timePassed;
        }

        uiManager.DisplayGameOverPanel(timePassed, topScore);

        ResetScene();

    }

    private void ResetScene()
    {
        platformsJumped = 0;
        timePassed = 0;
        
        //platformsText.text = "00";
        timeText.text = "00";
        
        if (envController != null)
        {
            Destroy(envController.gameObject);
        }
        
        if (player != null)
        {
            Destroy(player.gameObject);
        }
        
        if (envController == null)
        {
        var env = Instantiate(environmentPrefab, Vector2.zero, Quaternion.identity);
        envController = env.GetComponent<EnvironmentController>();
        //envController.backgroundObject = BackgroundObject;
        }

        if (player == null)
        {
        var plyr = Instantiate(playerPrefab, playerInitPos.position, Quaternion.identity);
        player = plyr.GetComponent<Player_Controller>();
        }
        
        LoadPlayerAndEnvironment();

    }
}
