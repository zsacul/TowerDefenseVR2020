using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BTW, to jest pierwszy raz jak Pan Drozd używa dziedziczenia w czymkolwiek, więc jak to się spektakularnie 
 * nie rozkurwi, to proszę o owacje na stojąco oraz przesłanie mi 1000zł na konto bankowe. Osobiste gratulacje
 * składane przez córki prezydenta będę przyjmował po przerwie świątecznej */
public class M1911InHandManager : PropManager
{
    public GameObject Debree;
    private GameObject HandManger;
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
        GameObject P = Instantiate(projectile, transform.position, transform.rotation);
    }

    public bool TriggerState = false;
    public override void PointEvent(float input)
    {
        GizmoAnimation.SetFloat("PointFloat", input);

        if (input < 0.5f)
            TriggerState = false;
        else if (!TriggerState)
        {
            TriggerState = true;
            pewpew();
        }
    }

    public override void Initialize()
    {
        HandManger = transform.parent.gameObject; /* assume that the parent is the hand manager */
    }

    public override void Respawn(GameObject Motivator)
    {
        transform.gameObject.SetActive(true); /* ofc chcemy też pokazać swoją rękę */
        Destroy(Motivator);
    }

    private int remcount = 0;
    public override void Remove(Vector3 DebreeVelocity)
    {
        
        Debug.Log(DebreeVelocity);
        transform.gameObject.SetActive(false); /* ofc chcemy też pokazać schować rękę */

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
}
