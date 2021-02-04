using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlatform : Platform
{
    
     protected override void Awake() 
    {
        base.Awake();
        setColor();
        type = platformType.Red;


    }

    protected override void Start()
    {
        base.Start();
    }

    private void setColor()
    {
        GetComponent<SpriteRenderer>().sprite = envController.redPlatformSprite;
    }


    void Update()
    {
        
    }
}
