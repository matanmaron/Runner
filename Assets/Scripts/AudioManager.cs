using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceJump = null;
    [SerializeField] private AudioSource _audioSource2 = null;
    [SerializeField] private AudioSource _audioSource3 = null;
    [SerializeField] private AudioSource _audioSource4 = null;
    [SerializeField] private AudioSource _musicSource = null;

    private void PlayJumpAudio() => _audioSourceJump.Play();


    private void OnEnable()
    {
        Player_Controller.OnPlayerJump += PlayJumpAudio;
    }
    private void OnDisable()
    {
        Player_Controller.OnPlayerJump -= PlayJumpAudio;
    }
}
