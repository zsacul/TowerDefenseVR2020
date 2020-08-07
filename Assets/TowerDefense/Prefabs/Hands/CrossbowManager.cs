using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowManager : PropManager
{
    public GameObject Debree;
    public GameObject LoadedArrowPrefab;
    private GameObject HandManger;
    private GameObject LoadedArrow;
    public bool loaded = false;
    public bool control = true;

    private GameObject String;
    private GameObject Rest;
    private GameObject LoadedTgt;
    public override void GrabEvent(float input)
    {
        //Debug.Log($"overriden gevent {input}");
        if (input > 0.70f) // The object is grabbed, treat it as usual
            GizmoAnimation.SetFloat("GripFloat", input);
        else
            HandManger.GetComponent<HandDeployer>().DeployNth(0);
        /* Jeśli puściliśmy obiekt, to tylko powiedzmy o tym naszemu managerowi. On zadba żeby nas wyłączyć i zawołać nasz poweoff */
    }

    public GameObject projectile;
    private void pewpew()
    {
        Quaternion rotationForArrow = Quaternion.Euler(transform.localRotation.x + 61.42f, transform.localRotation.y - 209.02f, transform.localRotation.z + 112.03f);
        Vector3 positionForArrow = LoadedArrow.transform.position;
        Destroy(LoadedArrow);
        GameObject P = Instantiate(projectile, positionForArrow, rotationForArrow);
    }

    public override void PointEvent(float input)
    {
        if (input > 0.75)
        {
            if (control && loaded)
            {
                loaded = false;
                Rest.SetActive(true);
                pewpew();
            }
        }
    }

    public override void Respawn(GameObject Motivator)
    {
        transform.gameObject.SetActive(true); /* ofc chcemy też pokazać swoją rękę */
        Destroy(Motivator);
    }


    private int remcount = 0;
    public override void Remove(Vector3 DebreeVelocity)
    {
        transform.gameObject.SetActive(false);

        if (remcount > 0)
        {
            GameObject dispatched = Instantiate(Debree);
            dispatched.GetComponent<Transform>().position = transform.position;
            dispatched.GetComponent<Transform>().rotation = transform.rotation;
            dispatched.GetComponent<Rigidbody>().velocity = DebreeVelocity;
            dispatched.SetActive(true);
        }

        remcount++;
    }

    public void LoadedCrossbowArrow()
    {
        LoadedArrow = Instantiate(LoadedArrowPrefab, transform.position, transform.rotation);
        LoadedArrow.transform.SetParent(this.transform);
        Quaternion rotationForArrow = Quaternion.Euler(transform.localRotation.x + 90, transform.localRotation.y, transform.localRotation.z);
        Vector3 positionForArrow = new Vector3(transform.localPosition.x + 2.88f, transform.localPosition.y + 0.29f, transform.localPosition.z - 10.46f);
        LoadedArrow.transform.localPosition = positionForArrow;
        LoadedArrow.transform.localRotation = rotationForArrow;
        LoadedArrow.transform.localScale = LoadedArrowPrefab.transform.localScale;
        
    }

    public override void Initialize()
    {
        HandManger = transform.parent.gameObject; /* assume that the parent is the hand manager */
        Rest = transform.GetChild(0).gameObject;
        LoadedTgt = transform.GetChild(4).gameObject;

        GameObject CrossBowStripped = transform.GetChild(3).gameObject;
        GameObject Model = CrossBowStripped.transform.GetChild(0).gameObject;
        GameObject Retarded = Model.transform.GetChild(0).gameObject;
        String = Retarded.transform.GetChild(8).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(control)
        {
            if (loaded)
            {
                String.transform.position = LoadedTgt.transform.position;
            }
            else
            {
                String.transform.position = Rest.transform.position;
            }
        }
    }
}
