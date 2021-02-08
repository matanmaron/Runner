using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] public bool isMoving;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] public Sprite regularPlatformSprite;
    [SerializeField] public Sprite blackPlatformSprite;
    [SerializeField] public Sprite redPlatformSprite;
    [SerializeField] public Sprite greenPlatformSprite;
    [SerializeField] public GameObject lastPlatform;
    [SerializeField] private Transform platformsSpawnPoint;
    [SerializeField] private GameObject newPlatform;
    public List<Platform> platforms = new List<Platform>();
    public GameObject backgroundObject;
    public static event Action OnMoveEnvironemt;

    void Start()
    {
        isMoving = false;

        playerSpeed = GameManager.Instance.playerInitSpeed;

        platformsSpawnPoint = GameManager.Instance.platformsSpawnPoint;
        

        platforms.Add(FindObjectOfType<FirstPlatform>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPlatform();
        }
    }

    private void FixedUpdate() 
    {
        if (isMoving)
        {
            MoveEnvironemt();
        }
    }
    private void MoveEnvironemt()
    {
        transform.Translate(Vector3.left * playerSpeed);
        OnMoveEnvironemt?.Invoke();
    }

    public void StartMoving()
    {
        isMoving = true;
        StartCoroutine("IncreaseSPeed");
    }

        public void StopMoving()
    {
        isMoving = false;
    }

    public void SpawnPlatform()
    {
        //instantiate new platform from prefab
        newPlatform = Instantiate(platformPrefab, platformsSpawnPoint.position, Quaternion.identity, gameObject.transform);

        // randomize the type of the new platform (regular / green / red / black)
        SetPlatformType();
        
        // random the new platform's length (x)
        var newPlatformLength = Random.Range(GameManager.Instance.platformMinLength, GameManager.Instance.platformMaxLength);
        
        // new platform Height (y) remains unchanged.
        var newPlatformHeight = newPlatform.GetComponent<SpriteRenderer>().size.y;

        // change the new platform's Sprite Renderer size to the new random size
        newPlatform.GetComponent<SpriteRenderer>().size =  new Vector2(newPlatformLength, newPlatformHeight);

        // Set new platform collider's size to the Sprite renderer size
        newPlatform.GetComponent<Platform>().SetColliderSize();

        // Distance between Platforms
        float yPosMin;
        float yPosMax;

        var maxYDistance = GameManager.Instance.platformMaxYDistance;

        if (lastPlatform.GetComponent<Platform>().type == Platform.PlatformType.Red) // if the last platform is red (weaker jump)
        {
            maxYDistance /= 2;
        }
        
        // Y Distance
        if (lastPlatform.transform.position.y+GameManager.Instance.platformMaxYDistance > GameManager.Instance.platformMaxYPosition) //if last platform's position + the desired distance exceeds the available space.
            {
                yPosMax = GameManager.Instance.platformMaxYPosition;
            }
        else
            {
                yPosMax = lastPlatform.transform.position.y + 1 + maxYDistance;
            }

        yPosMin = GameManager.Instance.platformMinYPosition; // bottom of the screen
        
        var newYPos = Random.Range(yPosMin, yPosMax);
        

        // X Distance
        // distance is a random between min and max in GameManager 
        var xDistance = Random.Range(GameManager.Instance.platformMinXDistance, GameManager.Instance.platformMaxXDistance);


        // new platform X position is the rightmost x of the last platform + half the length of the new platform + the random distance
        var newXPos = lastPlatform.GetComponent<SpriteRenderer>().bounds.max.x + newPlatform.GetComponent<SpriteRenderer>().bounds.extents.x + xDistance;


        newPlatform.transform.position = new Vector2(newXPos, newYPos);

        platforms.Add(newPlatform.GetComponent<Platform>());

        lastPlatform = newPlatform; //the new platform is now the last in preparation to the next one
    }
    
    private void SetPlatformType()
    {
        int selectedType = RandomizeType();

        switch (selectedType)
        {
            case 0:
                newPlatform.AddComponent<RegularPlatform>();
                break;
            case 1:
                newPlatform.AddComponent<GreenPlatform>();
                break;
            case 2:
                newPlatform.AddComponent<RedPlatform>();
                break;
            case 3:
                newPlatform.AddComponent<BlackPlatform>();
                break;
            default:
                break;
        }

    }

    private int RandomizeType()
    {
        float totalProb = 0f;

        foreach (float elem in GameManager.Instance.platformTypesProbs)
        {
            totalProb += elem;
        }

        float randomPoint = Random.value * totalProb;

        for (int i=0; i < GameManager.Instance.platformTypesProbs.Count ; i++)
        {
            if (randomPoint < GameManager.Instance.platformTypesProbs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= GameManager.Instance.platformTypesProbs[i];
            }
        }
        return GameManager.Instance.platformTypesProbs.Count -1;
    }


    public IEnumerator BlackenPlatforms()
    {
        foreach (Platform platform in platforms)
            {
                platform.GetComponent<SpriteRenderer>().sprite = blackPlatformSprite;
            }

        yield return new WaitForSeconds(GameManager.Instance.blackPlatformTimeDuration);

        foreach (Platform platform in platforms)
            {
                if (platform.type == Platform.PlatformType.Regular)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = regularPlatformSprite;
                }
                else if (platform.type == Platform.PlatformType.Green)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = greenPlatformSprite;
                }
                else if (platform.type == Platform.PlatformType.Red)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = redPlatformSprite;
                }
                else if (platform.type == Platform.PlatformType.Black)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = blackPlatformSprite;
                }
            }
    }

    IEnumerator IncreaseSPeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            playerSpeed += GameManager.Instance.playerSpeedIncremetPerSec;
        }
    }
}
