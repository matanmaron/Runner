using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceJump = null;
    [SerializeField] private AudioSource _audioSourceDead = null;
    [SerializeField] private AudioSource _audioSourceHit = null;
    [SerializeField] private AudioSource _musicSource = null;

    private void PlayJumpAudio() => _audioSourceJump.Play();
    private void PlayDeadAudio() => _audioSourceDead.Play();
    private void PlayHitAudio() => _audioSourceHit.Play();

    private void OnEnable()
    {
        _musicSource.Play();
        Player_Controller.OnPlayerJump += PlayJumpAudio;
        Player_Controller.OnPlayerDead += PlayDeadAudio;
        Player_Controller.OnPlayerGrounded += PlayHitAudio;
    }

    private void OnDisable()
    {
        Player_Controller.OnPlayerJump -= PlayJumpAudio;
        Player_Controller.OnPlayerDead -= PlayDeadAudio;
        Player_Controller.OnPlayerGrounded -= PlayHitAudio;
    }
}
