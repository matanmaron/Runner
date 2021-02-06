using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header ("Prefabs and Positions")]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject environmentPrefab = null;
    [SerializeField] private Transform playerInitPos = null;
    [SerializeField] public Transform platformsSpawnPoint = null;
    [SerializeField] UI_Manager UIManager = null;
    [SerializeField] VFX_Manager vfxManager = null;
    
    [HideInInspector] public GameObject BackgroundObject = null;
    
    [Header("Player parameters")]
    public float playerInitSpeed = 0.15f;
    public float playerSpeedIncremetPerSec = 0.002f;
    public float playerJumpForce = 5000f;
    public float playerMaxJumpDuration = 0.4f;
   
    [Header("Environment parameters")]
    public float platformMinXDistance = 1f;
    public float platformMaxXDistance = 4f;
    public float platformMaxYDistance = 2.5f;
    public float platformMinYPosition = -4f;
    public float platformMaxYPosition = 2.5f;
    public float platformMinLength = 0.4f;
    public float platformMaxLength = 1.5f;
    public float regularPlatformChance = 50f;
    public float greenPlatformChance = 20f;
    public float redPlatformChance = 10f;
    public float blackPlatformChance = 10f;
    public float greenPlatformJumpMultiplier = 1.5f;
    public float redPlatformJumpMultiplier = 0.5f;
    public float blackPlatformTimeDuration = 2f;
    
    [Header("Background parameters")]
    public float backgroundSpeed = 0.03f;
    private int platformsJumped = 0; //score

    [Header("keeping track")]
    [SerializeField] private float topScore = 0f;
    [SerializeField] private float timePassed = 0;
    
    [SerializeField] private EnvironmentController envController = null;
    [SerializeField] private Player_Controller player = null;
    
    [HideInInspector] public List<float> platformTypesProbs = new List<float>();

    public static GameManager Instance { get; private set; } //singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        LoadPlatformProbs();
        ResetScene();
    }

    private void LoadPlatformProbs()
    {
        platformTypesProbs.Add(regularPlatformChance);
        platformTypesProbs.Add(greenPlatformChance);
        platformTypesProbs.Add(redPlatformChance);
        platformTypesProbs.Add(blackPlatformChance);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            player.StartRunning();
            envController.StartMoving();
            UIManager.HideGameOverPanel();
            UIManager.HideStartLabel();
        }

        KeepTimeScore();
        
    }

    private void KeepTimeScore()
    {
        if (envController.isMoving)
        {
            timePassed += Time.deltaTime;
            UIManager.UpdateTime(timePassed);
        }
    }

    public void addPlatformToScore()
    {
        platformsJumped++;
    }

    public void GameOver()
    {
        if (timePassed > topScore)
        {
            topScore = timePassed;
        }
        if (envController != null)
        {
            Destroy(envController.gameObject);
        }
        if (player != null)
        {
            Destroy(player.gameObject);
        }
        UIManager.DisplayGameOverPanel(timePassed, topScore);
        ResetScene();
    }

    private void ResetScene()
    {
        platformsJumped = 0;
        timePassed = 0;
        
        var env = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity);
        envController = env.GetComponent<EnvironmentController>();

        var plyr = Instantiate(playerPrefab, playerInitPos.position, Quaternion.identity);
        player = plyr.GetComponent<Player_Controller>();
        vfxManager.PlayerSpawnVFX(playerInitPos.position);
        
    }
}
