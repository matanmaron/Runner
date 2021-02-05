using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceJump = null;
    [SerializeField] private AudioSource _audioSourceDead = null;
    [SerializeField] private AudioSource _musicSource = null;

    private void PlayJumpAudio() => _audioSourceJump.Play();
    private void PlayJumpDead() => _audioSourceDead.Play();


    private void OnEnable()
    {
        Player_Controller.OnPlayerJump += PlayJumpAudio;
        Player_Controller.OnPlayerDead += PlayJumpDead;
    }
    private void OnDisable()
    {
        Player_Controller.OnPlayerJump -= PlayJumpAudio;
        Player_Controller.OnPlayerDead -= PlayJumpDead;
    }
}
