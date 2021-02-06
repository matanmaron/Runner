using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlatform : Platform
{
    
    protected override void Awake() 
    {
        base.Awake();
        GetComponent<SpriteRenderer>().sprite = envController.regularPlatformSprite;
        type = PlatformType.Regular;


    }
    
    protected override void Start()
    {
        SetColliderSize();
    }
    
    void Update()
    {
        
    }
}
