using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] List<Sprite> running = null;
    [SerializeField] Image player = null;
    [SerializeField] float RunningSpeed = 0.02f;

    [SerializeField] GameObject panelPlayer = null;
    [SerializeField] GameObject panelButtons = null;
    [SerializeField] GameObject panelHelp = null;

    private int currentImage = 0;

    private void Start()
    {
        if (!panelPlayer.activeSelf) { panelPlayer.SetActive(true); }
        if (!panelButtons.activeSelf) { panelButtons.SetActive(true); }
        if (panelHelp.activeSelf) { panelHelp.SetActive(false); }
        StartCoroutine(PlayerRun());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnQuit();
            return;
        }
        if (panelHelp.activeSelf && Input.anyKey)
        {
            OnHelp(false);
            return;
        }
    }

    public void OnHelp(bool show)
    { 
        panelPlayer.SetActive(!show); 
        panelButtons.SetActive(!show); 
        panelHelp.SetActive(show); 
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    IEnumerator PlayerRun()
    {
        player.sprite = GetNextImage();
        yield return new WaitForSeconds(RunningSpeed);
        StartCoroutine(PlayerRun());
    }

    private Sprite GetNextImage()
    {
        if (currentImage+1>running.Count)
        {
            currentImage = 0;
        }
        return running[currentImage++];
    }
}
