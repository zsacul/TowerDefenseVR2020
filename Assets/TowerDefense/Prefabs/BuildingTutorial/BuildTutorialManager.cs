﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum BuildingTutorialState
{
    notStarted,
    selectTower,
    buildTower,
    upgradeTower,
    selectObstacle,
    buildObstacle
}

public class BuildTutorialManager : MonoBehaviour, IQuest
{
    [NonSerialized]
    public BuildingTutorialState currentState;
    public UnityEvent TutorialFinished;
    bool state;
    bool started;
    [SerializeField]
    CrossbowMainNode CrossbowTutorial;
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
        currentState = BuildingTutorialState.notStarted;
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
        }
        else
        {
            Destroy(CrossbowTutorial.gameObject);
            SetCurrentQuest(firstQuest);
            started = true;
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
        //Debug.Log("EndTutorial() in BuildTutorial called");
        TurnOffCurrentQuest();
        TutorialFinished.Invoke();
        state = true;
        started = false;
    }

    private void TurnOffCurrentQuest()
    {
        if (currentQuest != null)
        {
            currentQuest.gameObject.SetActive(false);
            currentQuest.gameObject.GetComponent<BuildTutorialQuest>().enabled = false;
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

    public void Skip()
    {
        //Debug.Log("Skip() called");
        foreach (Transform child in transform)
        {
            var questScript = child.GetComponent<BuildTutorialQuest>();
            if (questScript)
            {
                //Debug.Log(questScript.gameObject + " tries to end");
                questScript.SetNextQuest();
            }
        }
    }

    void DestoryMe()
    {
        Destroy(this.gameObject);
    }
}
