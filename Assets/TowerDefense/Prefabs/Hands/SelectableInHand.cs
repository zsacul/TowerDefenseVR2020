using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SelectableInHand : PropManager
{
    private GameObject HandManger;
    private GameObject Miniature;
    private GameObject Panel;
    public override void GrabEvent(float input)
    {
        if (!retarded_controlls)
        {
            //Debug.Log($"overriden gevent {input}");
            if (input > 0.70f) // The object is grabbed, treat it as usual
                GizmoAnimation.SetFloat("GripFloat", input);
            else
                HandManger.GetComponent<HandDeployer>().DeployNth(0);
            /* Jeśli puściliśmy obiekt, to tylko powiedzmy o tym naszemu managerowi. On zadba żeby nas wyłączyć i zawołać nasz poweoff */
        }
        else
        {
            GizmoAnimation.SetFloat("GripFloat", 0.0f);
            GizmoAnimation.SetFloat("PointFloat", 0.0f);
        }
    }

    public override void RetardedChangeGrabState()
    {
        if (retarded_controlls)
        {
            HandManger.GetComponent<HandDeployer>().DeployNth(0);
            Panel.GetComponent<GenericNBoxSelector>().CurrentlyBuildingS(false);
            Destroy(Miniature);
        }
    }

    public override void Respawn(GameObject Motivator)
    {
        //GameObject ball = Panel.transform.GetChild
        // sprawdź czy wybór jest w ogóle legalny!

        transform.gameObject.SetActive(true); /* ofc chcemy też pokazać swoją rękę */
        GizmoAnimation.SetFloat("GripFloat", 0.0f);
        GizmoAnimation.SetFloat("PointFloat", 0.0f);

        //Panel.GetComponent<GenericNBoxSelector>().CurrentlyBuildingS(true);
        //GameObject Miniaturka = Panel.GetComponent<GenericNBoxSelector>().getSelectedMiniature();
        Miniature = Instantiate(Motivator, transform);
        Miniature.transform.localPosition = new Vector3(0.07f, -1.09f, -2.07f);
        Miniature.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
    }

    public override void Remove()
    {
        transform.gameObject.SetActive(false);
        Destroy(Miniature);
        Panel.GetComponent<GenericNBoxSelector>().CurrentlyBuildingS(false);
    }

    public override void Initialize()
    {
        HandManger = transform.parent.gameObject; /* assume that the parent is the hand manager */
        GizmoAnimation.SetFloat("GripFloat", 0.0f);
        GizmoAnimation.SetFloat("PointFloat", 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
