using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BTW, to jest pierwszy raz jak Pan Drozd używa dziedziczenia w czymkolwiek, więc jak to się spektakularnie 
 * nie rozkurwi, to proszę o owacje na stojąco oraz przesłanie mi 1000zł na konto bankowe. Osobiste gratulacje
 * składane przez córki prezydenta będę przyjmował po przerwie świątecznej */
public class M1911InHandManager : PropManager
{
    public GameObject Debree;
    public string GrabbableName;

    private GameObject HandManger;
    public GameObject Grabbable;
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

        /* https://knowyourmeme.com/photos/1384541-crying-cat */
        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rgo in allObjects)
           if (rgo.name == "VRTK Setup")
              Grabbable = rgo.gameObject.transform.Find($"CameraRigs/UnityXRCameraRig/HeadAnchor/{GrabbableName}").gameObject;

        if (Grabbable == null)
            Debug.LogError("IF YOU DARE TO MISS THE GRABBABLE VARIABLE, WEIRD SHIT WILL HAPPEN!");

        Grabbable.SetActive(false); /* gdy złapiemy pistolet to chcemy żeby zniknął ten w HUD */
    }

    public override void Respawn()
    {
        Grabbable.SetActive(false); /* gdy złapiemy pistolet to chcemy żeby zniknął ten w HUD */
        transform.gameObject.SetActive(true); /* ofc chcemy też pokazać swoją rękę */
    }


    public override void Remove()
    {
        transform.gameObject.SetActive(false); /* ofc chcemy też pokazać schować rękę */
      //Debug.Log("I am once again asking you to fuck off");
        Grabbable.SetActive(true); /* gdy puścimy pistolet, to chcemy żeby ten w HUD się pojawił */
        //Instantiate(Debree); /* oraz chcemy zrobić zinstantiowany pistolet który se spadnie na ziemię */
    }
}
