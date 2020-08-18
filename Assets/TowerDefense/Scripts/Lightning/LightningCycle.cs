using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCycle : GameEventListener
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

    public Gradient skyColorLost;
    public Gradient groundColorLost;

    private bool gameLost;
    private float changingSkyBoxTime;
    GradientColorKey[] skyKey;
    GradientColorKey[] groundKey;
    GradientAlphaKey[] alphaKey;

    int count;
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
        gameLost = false;
        count = 0;
        changingSkyBoxTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLost)
        {
            changingSkyBoxTime += Time.deltaTime / 8.0f;
            if (changingSkyBoxTime <= 1)
            {
                UpdateSkyboxAfterDefeat();
            }
        }
        else
        {
            UpdateLight();
            currentTime += (Time.deltaTime / SecondsInAFullDay) * timeMultiplier;

            if (currentTime >= 1)
            {
                currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
            }
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
        //Debug.Log("odpalamy nocke");
        float currentDayDuration = SecondsInAFullDay;
        SecondsInAFullDay = 12f;
        yield return new WaitUntil(() => currentTime >= 0.75);
        SecondsInAFullDay = currentDayDuration;
    }

    public IEnumerator ChangeToDay()
    {
        //Debug.Log("odpalamy dzionek");
        float currentDayDuration = SecondsInAFullDay;
        SecondsInAFullDay = 12f;
        yield return new WaitUntil(() => currentTime <= 0.25);
        yield return new WaitUntil(() => currentTime >= 0.25);
        SecondsInAFullDay = currentDayDuration;
    }

    public override void OnEventRaised(Object data)
    {
        gameLost = true;

        skyKey = new GradientColorKey[2];
        skyKey[0].color = skyColor.Evaluate(currentTime);
        skyKey[0].time = 0.0f;
        skyKey[1].color = Color.red;
        skyKey[1].time = 1.0f;

        groundKey = new GradientColorKey[2];
        groundKey[0].color = groundLightningColor.Evaluate(currentTime);
        groundKey[0].time = 0.0f;
        groundKey[1].color = Color.red;
        groundKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        skyColorLost.SetKeys(skyKey, alphaKey);
        groundColorLost.SetKeys(groundKey, alphaKey);
    }

    private void UpdateSkyboxAfterDefeat()
    {
        RenderSettings.ambientGroundColor = groundColorLost.Evaluate(changingSkyBoxTime);
        RenderSettings.skybox.SetColor("_SkyTint", skyColorLost.Evaluate(changingSkyBoxTime));
    }
}
