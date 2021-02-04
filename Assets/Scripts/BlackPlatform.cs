using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPlatform : Platform
{
    
     protected override void Awake() 
    {
        base.Awake();
        setColor();
        type = platformType.Black;

    }

    protected override void Start()
    {
        base.Start();
    }

    private void setColor()
    {
        GetComponent<SpriteRenderer>().sprite = envController.blackPlatformSprite;
    }

    void Update()
    {
        
    }
}
