using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.Events;
using System.Net.WebSockets;
using UnityEditor.UI;
using Zinnia.Extension;

public class NboxSelectorNewBuilding : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject LookAtGJ;
    public GameObject StepParent;
    public GameObject EffectorHandHook;
    public GameEvent panelRequired;
    public bool CurrentlyBuilding = false;
    private int selectedItem = 0;
    public Material Red;
    public Material Green;

    public GameObject TowerName;
    public GameObject TowerCost;

    public GameObject TDL1;
    public GameObject TDL2;
    public GameObject TDL3;

    public GameObject HbackDesc;
    public GameObject Hbackdots;
    public List<GameObject> NBOXstate;
    public List<GameObject> NBOXselector;
    [SerializeField]
    public List<NBOXselectable_t> NBOXselectable;
    public int PanelFlvr = 1; // this identifies that we are a panel that upgrades towers
    public int cash = 0;

    // 5 - 35 with 4 spaces.
    // 5, 15, 25, 35
    public void DisplayBoard(int board)
    {
        foreach (GameObject Sent in NBOXstate)
        {
            Sent.SetActive(false);
        }

        if (board == -1) return;

        if (board > NBOXstate.Count)
        {
            Debug.Log("NBOXSELECTOR illegal page selected!");
            return;
        }

        NBOXstate[board].SetActive(true);
        selectedItem = board - 1;
    }

    public void CurrentlyBuildingS(bool state)
    {
        CurrentlyBuilding = state;
        for (int i = 0; i < NBOXselectable.Count; i++)
            NBOXselector[i].SetActive(!state);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject Sent in NBOXstate)
        {
            Sent.SetActive(false);
        }

        for (int i = 0; i < NBOXselectable.Count; i++)
        {
            GameObject tmp = NBOXselectable[i].Miniature;
            Debug.Log(tmp.name);
            GameObject ins = Instantiate(tmp, NBOXselector[i].transform);
            Debug.Log("setting scale!");
            ins.transform.localScale = new Vector3(0.01f, 0.5f, 0.01f);
            ins.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
            ins.transform.localPosition = new Vector3(0.0f, 0.0f, 0.3f);
            NBOXselectable[i].MiniatureInstance = ins;
        }

        NBOXstate[0].SetActive(true);

        Respawn();
        //transform.gameObject.SetActive(false);
    }

    private bool first = true;
    public GameObject invokerButton;
    public void Respawn()
    {
        transform.position = StepParent.transform.position;

        if (!first)
        {
            GameObject inv = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.0f);
            float closest = 100.0f;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Debug.Log($"{i} gość tag {hitColliders[i].gameObject.tag} nazwa {hitColliders[i].gameObject.name}");
                if (hitColliders[i].gameObject.tag == "VRButton")
                {
                    if (Vector3.Distance(transform.position, hitColliders[i].gameObject.transform.position) < closest)
                    {
                        closest = Vector3.Distance(transform.position, hitColliders[i].gameObject.transform.position);
                        inv = hitColliders[i].gameObject;
                    }
                }
            }

            Debug.Log("przeszukani");
            if (closest < 99.0f)
            {
                invokerButton = inv.gameObject;
                Debug.Log($"ZNALEZIONY TO {invokerButton}");
                if (invokerButton != null)
                    invokerButton.GetComponent<UpgradeCylinderPlayerInteractionGovernor>().Enter();
            }
        }

        first = false;
        cash = GameManagerGO.GetComponent<BuildManager>().Money;
        for (int i = 0; i < NBOXselectable.Count; i++)
        {
            if (NBOXselectable[i].Cost <= cash)
            {
                NBOXselector[i].GetComponent<MeshRenderer>().material = Green;
                NBOXselector[i].tag = "Grababble";
            }
            else
            {
                NBOXselector[i].GetComponent<MeshRenderer>().material = Red;
                NBOXselector[i].tag = "Untagged";
            }
        }
    }

    public GameObject getSelectedMiniature()
    {
        return NBOXselectable[selectedItem].Miniature;
    }

    // Update is called once per frame
    void displayQuitPrompt(float mindist)
    {
        DisplayBoard(0);
        TowerName.GetComponent<TextMesh>().text = " ";
        TowerCost.GetComponent<TextMesh>().text = " ";
        TDL1.GetComponent<TextMesh>().text = " ";
        TDL2.GetComponent<TextMesh>().text = " ";
        TDL3.GetComponent<TextMesh>().text = " ";
        HbackDesc.SetActive(true);
        StringBuilder sb = new StringBuilder("[", 15);
        for (float i = 0.25f; i < 1.25f; i += 0.1f)
            if (i < mindist)
                sb.Append("|");
            else
                sb.Append(" ");
        sb.Append("]");
        Hbackdots.SetActive(true);
        Hbackdots.GetComponent<TextMesh>().text = sb.ToString();
    }

    void displaySelectScreen(int chosen)
    {
        DisplayBoard(chosen + 1);
        HbackDesc.SetActive(false);
        Hbackdots.SetActive(false);
        TowerName.GetComponent<TextMesh>().text = NBOXselectable[chosen].ItemName;
        TowerCost.GetComponent<TextMesh>().text = NBOXselectable[chosen].Cost.ToString();
        TDL1.GetComponent<TextMesh>().text = NBOXselectable[chosen].ItemDescriptionLine1;
        TDL2.GetComponent<TextMesh>().text = NBOXselectable[chosen].ItemDescriptionLine2;
        TDL3.GetComponent<TextMesh>().text = NBOXselectable[chosen].ItemDescriptionLine3;
    }

    void displayInProgressBuild()
    {
        DisplayBoard(0);
        TDL1.GetComponent<TextMesh>().text = "Żeby zbudować wypuść ulepszenie w <eeeeeee>s";
        TDL2.GetComponent<TextMesh>().text = "Żeby anulować wypuść ulepszenie w powietrzu";
        TDL3.GetComponent<TextMesh>().text = "";

    }
    void Update()
    {

        transform.position = StepParent.transform.position;

        if (LookAtGJ != null)
        {
            transform.LookAt(LookAtGJ.transform.position);
            transform.Rotate(-90.0f, 0.0f, 90.0f);
        }

        float mindist = 1000.0f;
        int chosen = 0;
        for (int i = 0; i < NBOXselector.Count; i++)
        {
            float dist = Vector3.Distance(NBOXselector[i].transform.position, EffectorHandHook.transform.position);
            //Debug.Log($"{i} distance {dist}");
            if (dist < mindist)
            {
                mindist = dist;
                chosen = i;
            }

        }

        if (CurrentlyBuilding)
        {
            displayInProgressBuild();
        }
        else
        {
            if (mindist > 1.25f)
            {
                if (invokerButton != null)
                {
                    invokerButton.GetComponent<UpgradeCylinderPlayerInteractionGovernor>().Exit();
                    invokerButton = null;
                }
                else
                    Debug.Log("The upgrade menu has been closed without a selected invoker. This might be a problem if this log displays more than once!");

                gameObject.SetActive(false);
            }
            else if (mindist > 0.25f)
            {
                displayQuitPrompt(mindist);
            }
            else
            {
                displaySelectScreen(chosen);
            }
        }
    }
}
