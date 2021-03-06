﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChestRewardMenu : MonoBehaviour
{
    private float sToWait { get; set; }

    [SerializeField] private Text rewardTimer = null;
    private UnityEngine.UI.Button rewardButton;
    private ulong lastChestOpen;

    public string fortuneRoullete;
    public bool isInMenu;

    // Start is called before the first frame update
    void Start()
    {
        sToWait = PlayerPrefs.GetFloat("SecondsToWaitReward");

        rewardButton = GetComponent<UnityEngine.UI.Button>();
        lastChestOpen = ulong.Parse(PlayerPrefs.GetString("LastRewardOpen"));

        if (!isRewardReady())
        {
            rewardButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!rewardButton.IsInteractable())
        {
            if (isRewardReady())
            {
                rewardButton.interactable = true;
                return;
            }

            // Set the timer
            ulong diff = ((ulong)DateTime.Now.Ticks - lastChestOpen);
            ulong m = diff / TimeSpan.TicksPerSecond;

            float secondsLeft = (float)(sToWait - m);

            string t = "";
            // Hours
            t += ((int)secondsLeft / 3600).ToString() + "h:";
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            // Minutes
            t += ((int)secondsLeft / 60).ToString("00") + "m:";
            // Seconds
            t += (secondsLeft % 60).ToString("00") + "s";

            rewardTimer.text = t;
        }
    }
    private bool isRewardReady()
    {
        lastChestOpen = ulong.Parse(PlayerPrefs.GetString("LastRewardOpen"));

        ulong diff = ((ulong)DateTime.Now.Ticks - lastChestOpen);
        ulong m = diff / TimeSpan.TicksPerSecond;

        float secondsLeft = (float)(sToWait - m);

        //Debug.Log(secondsLeft);

        if (secondsLeft < 0)
        {
            rewardTimer.text = "Let's Go!";
            return true;
        }
        else
        {
            return false;
        }
    }
    public void rewardClick()
    {
        GameManager.Instance.setIsInMenu(isInMenu);
        rewardButton.interactable = false;
        // Open fortune roullete
        GameManager.Instance.unloadLevel(GameManager.Instance.getCurrentLevel());
        GameManager.Instance.loadLevel(fortuneRoullete);
    }

}
