﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BuildTutorialManager : MonoBehaviour, IQuest
{
    public UnityEvent TutorialFinished;
    public GameObject InstructionsPanel;

    bool state;
    bool started;
    [SerializeField]
    BuildTutorialQuest firstQuest;
    BuildTutorialQuest currentQuest;

    private static BuildTutorialManager instance;

    public static BuildTutorialManager Instance {
        get {
            if(instance == null)
            {
                Debug.LogError("Missing BuildTutorialStart");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        started = false;
    }

    public BuildTutorialQuest CurrentQuest()
    {
        return currentQuest;
    }

    public void StartTutorial()
    {
        if (started)
        {
            TurnOffCurrentQuest();
            started = false;
            state = false;
            InstructionsPanel.SetActive(true);
        }
        else
        {
            SetCurrentQuest(firstQuest);
            started = true;
            InstructionsPanel.SetActive(false);
            
        }
    }

    public void SetCurrentQuest(BuildTutorialQuest nextQuest)
    {
        TurnOffCurrentQuest();
        currentQuest = nextQuest;

        if (!nextQuest.gameObject.activeSelf)
        {
            nextQuest.gameObject.SetActive(true);
            nextQuest.gameObject.GetComponent<BuildTutorialQuest>().enabled = true;
            nextQuest.EnterQuest();
        }
    }

    public void EndTutorial()
    {
        TurnOffCurrentQuest();
        TutorialFinished.Invoke();
        state = true;
        started = false;
        //Debug.Log("Tutorial ended");
        InstructionsPanel.SetActive(true);

    }

    private void TurnOffCurrentQuest()
    {
        if (currentQuest != null)
        {
            currentQuest.gameObject.SetActive(false);
            currentQuest.gameObject.GetComponent<BuildTutorialQuest>().enabled = false;
            currentQuest.gameObject.GetComponent<BuildTutorialQuest>().InstructionsPanel.SetActive(false);
            currentQuest = null;
        }
    }

    public void AddListenerFinish(UnityAction action)
    {
        TutorialFinished.AddListener(action);
    }

    public bool GetState()
    {
        return state;
    }
}