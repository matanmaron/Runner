using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlatform : Platform
{
    
    protected override void Awake() 
    {
        base.Awake();
        //GetComponent<MeshRenderer>().material = envControler.greenPlatformMaterial;
        GetComponent<SpriteRenderer>().sprite = envController.regularPlatformSprite;
        type = platformType.Regular;


    }
    
    protected override void Start()
    {
        SetColliderSize();
    }
    
    void Update()
    {
        
    }
}
