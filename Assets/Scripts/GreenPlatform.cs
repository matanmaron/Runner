using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlatform : Platform
{
    
    protected override void Awake() 
    {
        base.Awake();
        setColor();
        type = platformType.Green;

    }
    
    protected override void Start()
    {
        base.Start();
    }
    
    private void setColor()
    {
        GetComponent<SpriteRenderer>().sprite = envController.greenPlatformSprite;
    }

    void Update()
    {
        
    }
}
