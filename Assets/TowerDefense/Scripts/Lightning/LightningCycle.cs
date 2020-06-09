using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCycle : MonoBehaviour
{
    public AnimationCurve lightIntensityCurve;

    public AnimationCurve atmosphereThicknessCurve;
    public AnimationCurve skyboxExposureCurve;
    public AnimationCurve skyCurve;
    public Gradient dirLightningColors;
    public Gradient skyColor;
    public Gradient skyLightningColor;
    public Gradient equatorColor;
    public Gradient groundLightningColor;
    public ReflectionProbe probe;

    public Light directionalLight;
    public float SecondsInAFullDay = 120f;


    [Tooltip(" 1 = 24:00 (środek nocy) \n 0.5 = 12:00 (środek dnia)\n 0.75 = 18:00 (zachód)\n 0.25 = 6:00 (wschód)")]
    [Range(0, 1)]
    public float currentTime = 0.3f;
    public float timeMultiplier = 1f;
    public float lightIntensityAtNight = 0.15f;

    float shadowIntensity;
    public float shadowIntAtNight = 0.3f;
    float lightIntensity;

    bool isNight = false;
    public float sunSize = 0.06f;

    void renderProbe()
    {
        probe.RenderProbe();
    }

    // Start is called before the first frame update
    void Start()
    {
        lightIntensity = directionalLight.intensity;
        shadowIntensity = directionalLight.shadowStrength;
        Invoke("renderProbe", 0.05f);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLight();
        currentTime += (Time.deltaTime / SecondsInAFullDay) * timeMultiplier;
        
        if (currentTime >= 1)
        {
            currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
        }
    }

    void UpdateLight()
    {
       
        if (currentTime <= 0.25f || currentTime >= 0.75f)
        {
            probe.intensity = 0.45f;
            isNight = true;
            RenderSettings.skybox.SetFloat("_SunSize", 0.0f);
            directionalLight.intensity = lightIntensityAtNight;
            directionalLight.shadowStrength = shadowIntAtNight;
            directionalLight.transform.localRotation = Quaternion.Euler(-(1.0f - (currentTime * 360f) - 90), 170, 0);
        }
        else
        {
            probe.intensity = 0.8f;
            RenderSettings.skybox.SetFloat("_SunSize", sunSize);
            directionalLight.shadowStrength = shadowIntensity;
            directionalLight.intensity = lightIntensityAtNight + lightIntensityCurve.Evaluate(currentTime);
            directionalLight.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90, 170, 0);
            isNight = false;
        }
        RenderSettings.skybox.SetColor("_SkyTint", skyColor.Evaluate(currentTime));

        RenderSettings.skybox.SetFloat("_AtmosphereThickness", atmosphereThicknessCurve.Evaluate(currentTime));
        RenderSettings.skybox.SetFloat("_Exposure",1.0f + skyboxExposureCurve.Evaluate(currentTime));
        RenderSettings.ambientSkyColor = skyLightningColor.Evaluate(currentTime);
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(currentTime);
        RenderSettings.ambientGroundColor = groundLightningColor.Evaluate(currentTime);
        directionalLight.color = dirLightningColors.Evaluate(currentTime);
        
    }

    
    public IEnumerator ChangeToNight()
    {
        float currentDayDuration = SecondsInAFullDay;
        SecondsInAFullDay = 24f;
        yield return new WaitUntil(() => currentTime >= 0.75);
        SecondsInAFullDay = currentDayDuration;
    }

    public IEnumerator ChangeToDay()
    {
        float currentDayDuration = SecondsInAFullDay;
        SecondsInAFullDay = 24f;
        yield return new WaitUntil(() => currentTime >= 0.25);
        SecondsInAFullDay = currentDayDuration;
    }

}
