using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;

[System.Serializable]
public class NBOXselectable_t
{
    public GameObject Miniature;
    public GameObject MiniatureInstance;
    public int Cost;
}

public class GenericNBoxSelector : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject LookAtGJ;
    public GameObject StepParent;
    public GameObject EffectorHandHook;
    public List<GameObject> NBOXstate;
    public List<GameObject> NBOXselector;

    public Material Red;
    public Material Green;


    [SerializeField]
    private List<NBOXselectable_t> NBOXselectable;

    // 5 - 35 with 4 spaces.
    // 5, 15, 25, 35
    void DisplayBoard(int board)
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
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject Sent in NBOXstate)
        {
            Sent.SetActive(false);
        }

        for(int i = 0; i < NBOXselectable.Count; i++)
        {
            GameObject tmp = NBOXselectable[i].Miniature;
            GameObject ins = Instantiate(tmp, NBOXselector[i].transform);
            ins.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ins.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
            ins.transform.localPosition = new Vector3(0.0f, 0.0f, -0.3f);
            NBOXselectable[i].MiniatureInstance = ins;
        }

        NBOXstate[0].SetActive(true);

        Respawn();
    }

    void Respawn()
    {
        int cash = GameManagerGO.GetComponent<BuildManager>().Money;
        for(int i = 0; i < NBOXselectable.Count; i++)
        {
            if (NBOXselectable[i].Cost <= cash)
            {
                NBOXselector[i].GetComponent<MeshRenderer>().material = Green;
            } 
            else
            {
                NBOXselector[i].GetComponent<MeshRenderer>().material = Red;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = StepParent.transform.position;

        if (LookAtGJ != null)
        {
            transform.LookAt(LookAtGJ.transform.position);
            transform.Rotate(-90.0f, 0.0f, 90.0f);
        }

        bool shown = false;
        float mindist = 1000.0f;
        int chosen = 0;
        for(int i = 0; i < NBOXselector.Count; i++)
        {
            float dist = Vector3.Distance(NBOXselector[i].transform.position, EffectorHandHook.transform.position);
            Debug.Log($"{i} distance {dist}");
            if(dist < mindist && dist < 0.25f)
            {
                shown = true;
                mindist = dist;
                chosen = i;
            }

        }

        
        if(!shown)
        {
            DisplayBoard(0);
        } else {
            DisplayBoard(chosen + 1);
        }
    }
}
