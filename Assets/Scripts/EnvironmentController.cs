using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    
    private GameManager _gm;

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

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        isMoving = false;

        playerSpeed = _gm.playerInitSpeed;

        platformsSpawnPoint = _gm.platformsSpawnPoint;
        
        StartCoroutine("IncreaseSPeed");

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
            MoveEnvironemtn();
        }
    }
    private void MoveEnvironemtn()
    {
        
        transform.Translate(Vector3.left * playerSpeed);
        //backgroundObject.transform.Translate(Vector3.left * _gm.backgroundSpeed);
        _gm.BackgroundObject.transform.Translate(Vector3.left * _gm.backgroundSpeed);

    }

    public void StartMoving()
    {
        Debug.Log("Start Moving");
        isMoving = true;
        Debug.Log(isMoving);
    }

    public void SpawnPlatform()
    {
        //instantiate new platform from prefab
        newPlatform = Instantiate(platformPrefab, platformsSpawnPoint.position, Quaternion.identity, gameObject.transform);

        // randomize the type of the new platform (regular / green / red / black)
        SetPlatformType();
        
        // random the nee platform's length (x)
        var newPlatformLength = Random.Range(_gm.platformMinLength, _gm.platformMaxLength);
        
        // new platform Height (y) remains unchanged.
        var newPlatformHeight = newPlatform.GetComponent<SpriteRenderer>().size.y;

        // change the new platform's Sprite Renderer size to the new random size
        newPlatform.GetComponent<SpriteRenderer>().size =  new Vector2(newPlatformLength, newPlatformHeight);

        // Set new platform collider's size to the Sprite renderer size
        newPlatform.GetComponent<Platform>().SetColliderSize();

        // Distance between Platforms
        float yPosMin;
        float yPosMax;

        // Y Distance
        if (lastPlatform.transform.position.y+_gm.platformMaxYDistance > _gm.platformMaxYPosition) //if last platform's position + the desired distance exceeds the available space.
            {
                yPosMax = _gm.platformMaxYPosition;
            }
        else
            {
                yPosMax = lastPlatform.transform.position.y + 1 + _gm.platformMaxYDistance;
            }

        yPosMin = _gm.platformMinYPosition; // bottom of the screen
        
        var newYPos = Random.Range(yPosMin, yPosMax);


        // X Distance
        // distance is a random between min and max in GameManager 
        var xDistance = Random.Range(_gm.platformMinXDistance, _gm.platformMaxXDistance);


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

        foreach (float elem in _gm.platformTypesProbs)
        {
            totalProb += elem;
        }

        float randomPoint = Random.value * totalProb;

        for (int i=0; i < _gm.platformTypesProbs.Count ; i++)
        {
            if (randomPoint < _gm.platformTypesProbs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= _gm.platformTypesProbs[i];
            }
        }
        return _gm.platformTypesProbs.Count -1;
    }


    public IEnumerator BlackenPlatforms()
    {
        foreach (Platform platform in platforms)
            {
                platform.GetComponent<SpriteRenderer>().sprite = blackPlatformSprite;
            }

        yield return new WaitForSeconds(_gm.blackPlatformTimeDuration);

        foreach (Platform platform in platforms)
            {
                if (platform.type == Platform.platformType.Regular)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = regularPlatformSprite;
                }
                else if (platform.type == Platform.platformType.Green)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = greenPlatformSprite;
                }
                else if (platform.type == Platform.platformType.Red)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = redPlatformSprite;
                }
                else if (platform.type == Platform.platformType.Black)
                {
                    platform.GetComponent<SpriteRenderer>().sprite = blackPlatformSprite;
                }
            }
    }

    IEnumerator IncreaseSPeed()
    {
        while (isMoving)
        {
            yield return new WaitForSeconds(1f);
            playerSpeed += _gm.playerSpeedIncremetPerSec;
        }
    }
}
