using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChunkClaimHandler : MonoBehaviour
{
    public bool DebugMode;
    private Renderer OwnRenderer;
    private bool State;
    private UnityEngine.AI.NavMeshObstacle NavmapBlocker;
    // Start is called before the first frame update
    void Start() {
        NavmapBlocker = this.gameObject.transform.GetChild(0).gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>();
        OwnRenderer = this.GetComponent<Renderer>();
        UnClaim();
    }

    void Claim() {
        State = true;
        if (DebugMode)
            OwnRenderer.material.SetColor("_Color", Color.red);
        NavmapBlocker.enabled = true;
    }

    void UnClaim() {
        State = false;
        if (DebugMode)
            OwnRenderer.material.SetColor("_Color", Color.green);
        NavmapBlocker.enabled = false;
    }

    public void ToggleClaimed() {
        if (State)
            UnClaim();
        else
            Claim();
    }

    void OnMouseDown() {
        Debug.Log("wduszono");
        ToggleClaimed();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
