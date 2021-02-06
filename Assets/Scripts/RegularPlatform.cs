using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularPlatform : Platform
{

    protected override void Awake() 
    {
        base.Awake();
        setColor();
        type = PlatformType.Regular;

    }
    
    protected override void Start()
    {
        base.Start();
    }

    private void setColor()
    {
        GetComponent<SpriteRenderer>().sprite = envController.regularPlatformSprite;
    }


    void Update()
    {
        
    }
}
