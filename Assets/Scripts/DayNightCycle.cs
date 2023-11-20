using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float time;//1 = 24h
    [SerializeField]
    private float fullDayLenght; //In seconds
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float startTime = 0.5f; //noon
    private float timeRate;
    private Vector3 noon;

    [Header("Sun")]
    [SerializeField]
    private Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;
    //variable where we will store the HD lighting data
    private HDAdditionalLightData sunLightData;

    [Header("Moon")]
    [SerializeField]
    private Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;
    //variable where we will store the HD lighting data
    private HDAdditionalLightData moonLightData;

    private void Start()
    {
        timeRate = 1f;
        time = startTime;
        sunLightData = sun.GetComponent<HDAdditionalLightData>();
        moonLightData = moon.GetComponent<HDAdditionalLightData>();
    }

    private void Update()
    {
        //increment time
        time += timeRate * Time.deltaTime;

        //When day finish, restart
        if (time >= 1f)
        {
            time = 0f;
        }

        //Light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4f;

        //light intensity
        sunLightData.intensity = sunIntensity.Evaluate(time) * 10000f;
        moonLightData.intensity = moonIntensity.Evaluate(time) * 10000f;

        //light color
        sunLightData.color = sunColor.Evaluate(time);
        moonLightData.color = moonColor.Evaluate(time);

        //enable / disable
        if (sunLightData.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(true);
        } else if(sunLightData.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
            sun.gameObject.SetActive(true);
        }
    }
}
