using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    [SerializeField] private ParticleSystem playerSpawnEffectPrefab = null;


    public void PlayerSpawnVFX(Vector3 spawnPos)
    {
        Instantiate(playerSpawnEffectPrefab, spawnPos, Quaternion.identity);
    }

}