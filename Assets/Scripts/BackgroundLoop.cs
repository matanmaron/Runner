using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] private List<GameObject> backgroundObjects = new List<GameObject>();
    [SerializeField] float backgroundSpeed = 0.02f;
    private Camera mainCamera;
    private Vector2 screenBounds;

    private void Awake() {
        mainCamera = GetComponent<Camera>();
    }
    
    void Start()
    {
        EnvironmentController.OnMoveEnvironemt += OnMoveEnvironemt;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        foreach(GameObject obj in backgroundObjects)
        {
            foreach (Transform item in obj.GetComponentInChildren<Transform>())
            {
                loadChildObjects(item.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        EnvironmentController.OnMoveEnvironemt -= OnMoveEnvironemt;
    }

    private void LateUpdate() {
        foreach(GameObject obj in backgroundObjects)
        {
            foreach (Transform item in obj.GetComponentInChildren<Transform>())
            {
                RepositionChildObjects(item.gameObject);
            }
        }
    }

    private void OnMoveEnvironemt()
    {
        float i = 0.00f;
        foreach (GameObject obj in backgroundObjects)
        {
            obj.transform.Translate(Vector3.left * (backgroundSpeed + i));
            i += 0.04f;
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

        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

            if (transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
        }
    }
}
