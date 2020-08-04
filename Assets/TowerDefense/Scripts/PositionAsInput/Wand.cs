using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Wand : MonoBehaviour
{
    public Spell[] spells;
    public GameObject nodePrefab;
    public GameObject rightHand;
    public Transform head;
    public float distance;
    private List<Transform> pathedNodes;
    private Transform[] currentNodes;
    private SpellTree tree;
    private float charge;

    public UnityEvent onStartCast; 
    public UnityEvent onCastSucces; 
    public UnityEvent onCastFailure;
    private LineRenderer line;
    private bool casting;
    private int itemsInRightHand;
    private HandDeployer rightHandDeployer;
    IChargable current;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        tree = new SpellTree(spells);
        pathedNodes = new List<Transform>();
        rightHandDeployer = rightHand.GetComponent<HandDeployer>();
    }
    private void Update()
    {

        if (rightHandDeployer == null)
        {
            Debug.LogError("HandDeployer in RightControllerAlias not found");
        }
        else
        {
            itemsInRightHand = rightHandDeployer.listIterator;
        }

        if (casting)
        {
            line.enabled = true;
            line.positionCount = pathedNodes.Count;
            line.SetPositions(pathedNodes.ConvertAll(x => x.position).ToArray());
        }
        else
        {
            line.positionCount = 0;
            line.enabled = false;
        }
        if(!casting && (Input.GetAxis("VRTK_Axis10_RightTrigger") > 0.1f || Input.GetKey(KeyCode.LeftControl))
            && itemsInRightHand == 0)
        {
            CastStart();
        }
        if(casting && (Input.GetAxis("VRTK_Axis10_RightTrigger") < 0.1f && !Input.GetKey(KeyCode.LeftControl)))
        {
            CastEnd();
        }

    }
    private void CastStart()
    {
        pathedNodes.Clear();
        tree.BeginCast();
        onStartCast.Invoke();
        SpawnNodes();
        casting = true;
    }
    public void CastUpdate(Vector2Int pos)
    {
        if (current != null) return;
        (bool, Spell) n = tree.CastStep(pos);
        if(n.Item1)
        {
            pathedNodes.Add(currentNodes.First(x => x.GetComponent<NodeData>().nodePos == pos));
            ClearUnusedNodes();
            SpawnNodes();
            if(n.Item2 != null)
            {
                current = n.Item2.Cast(transform.position, transform.rotation, charge, transform);
                onCastSucces.Invoke();
            }
        }
        else
        {
            CastEnd();
        }
    }
    private void CastEnd()
    {
        casting = false;
        if(current != null)
        {
            current.Release();
            current = null;
        }
        else
        {
            onCastFailure.Invoke();
        }
        ClearNodes();
    }
    private void ClearNodes()
    {
        if(currentNodes != null)
        {
            foreach (Transform t in currentNodes)
            {
                Destroy(t.gameObject);
            }
            currentNodes = null;
        }
        if(pathedNodes != null)
        {
            foreach (Transform t in pathedNodes)
            {
                Destroy(t.gameObject);
            }
            pathedNodes.Clear();
        }
    }
    private void ClearUnusedNodes()
    {
        foreach(Transform t in currentNodes.Where(x => pathedNodes.Last() != x))
        {
            Destroy(t.gameObject);
        }
        currentNodes = null;
    }
    private void SpawnNodes()
    {
        Vector2Int[] nodes = tree.GetNodes();
        currentNodes = new Transform[nodes.Length];
        int i = 0;
        foreach(Vector2Int n in nodes)
        {
            Vector3 pos = transform.position + head.right * n.x * distance + head.up * n.y * distance;
            GameObject o = Instantiate(nodePrefab, pos, Quaternion.identity);
            NodeData d = o.GetComponent<NodeData>();
            d.nodePos = n;
            d.wand = this;
            currentNodes[i] = o.transform;
            i++;
        }
    }
}
