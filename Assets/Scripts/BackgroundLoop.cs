using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private EnvironmentController envController;
    [SerializeField] private Sprite[] backgroundSprites;
    
    [SerializeField] private List<GameObject> backgroundObjects = new List<GameObject>();

    private Camera mainCamera;
    private Vector2 screenBounds;

    [SerializeField] private string backgroundSortingLayerName;

    //public GameObject backgroundObject;


    private void Awake() {
        mainCamera = GetComponent<Camera>();
        envController = FindObjectOfType<EnvironmentController>();
    }
    
    void Start()
    {
        
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        CreateBackgroundObjects();
        
        foreach(GameObject obj in backgroundObjects)
        {
            loadChildObjects(obj);
        }
    }
    private void LateUpdate() {
        foreach(GameObject obj in backgroundObjects)
        {
            RepositionChildObjects(obj);
        }
    }

    private void loadChildObjects(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;

        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth) + 2;

        GameObject clone = Instantiate(obj) as GameObject;

        for (int i=0 ; i < childsNeeded ; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    private void RepositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();

        if (children.Length>1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length-1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

            if (transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            /*else if(transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2 , firstChild.transform.position.y, firstChild.transform.position.z);
            }*/
        }
    }
    
    private void CreateBackgroundObjects()
    {
        var backgroundObj = new GameObject("Background");
        backgroundObj.transform.position = Vector3.zero;

        for (int i=0 ; i<backgroundSprites.Length ; i++)
        {
            var backgroundLevel = new GameObject("BG_level_" + i.ToString());
            backgroundLevel.transform.SetParent(backgroundObj.transform);
            SpriteRenderer sr = backgroundLevel.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = backgroundSortingLayerName;
            sr.sortingOrder = i;
            sr.sprite = backgroundSprites[i];
            backgroundObjects.Add(backgroundLevel);
        }

        GameManager.Instance.BackgroundObject = backgroundObj;
        //envController.backgroundObject = backgroundObj;
    }

}
