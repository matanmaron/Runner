using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{

    [HideInInspector] public EnvironmentController envController;
    [HideInInspector] public enum PlatformType 
    {
        Regular, 
        Green, 
        Red, 
        Black
    }
    [HideInInspector] public PlatformType type;

    [SerializeField] private bool isLast = true;
    protected virtual void Awake()
    {
        envController = FindObjectOfType<EnvironmentController>();
    }
    protected virtual void Start() 
    {

       
    }
    
    void Update()
    {
        
    }


    public void SetColliderSize()
    {
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if (other.gameObject.CompareTag("PlatformShredder"))
            {
                envController.platforms.Remove(this);
                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("SpawnTrigger") && isLast)
            {
            
                envController.SpawnPlatform();
                isLast = false;
            } 
    }
}
