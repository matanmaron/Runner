using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    [SerializeField] private ParticleSystem playerSpawnEffectPrefab = null;
    //[SerializeField] private ParticleSystem playerLandEffectPrefab = null;
    //private Camera camera;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerSpawnVFX(Vector3 spawnPos)
    {
        //ParticleSystem ps = Instantiate(playerSpawnEffectPrefab, pos, Quaternion.identity);
        Instantiate(playerSpawnEffectPrefab, spawnPos, Quaternion.identity);
    }

    /*
    public void PlayerLandEffect(Vector3 landPos)
    {
        Instantiate(playerLandEffectPrefab, landPos, Quaternion.identity);
        
    }
    */
}
