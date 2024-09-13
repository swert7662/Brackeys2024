using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerTwo : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;

    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;

    [SerializeField] private Light globalLight;
    private float lightRotationSpeed = 2.0f; // Speed at which the light rotates smoothly
    private Quaternion targetRotation;

    [SerializeField, Range(0, 10)] private float daySpeed = 1.0f;

    private int minutes;

    public int Minutes
    { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }

    private int hours = 5;

    public int Hours
    { get { return hours; } set { hours = value; OnHoursChange(value); } }

    private int days;

    public int Days
    { get { return days; } set { days = value; } }

    private float tempSecond;

    public void Update()
    {
        // Adjust the time progression based on the daySpeed multiplier
        tempSecond += Time.deltaTime * daySpeed;

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond -= 1;
        }

        globalLight.transform.rotation = Quaternion.Slerp(globalLight.transform.rotation, targetRotation, Time.deltaTime * lightRotationSpeed);
    }

    private void OnMinutesChange(int value)
    {
        //globalLight.transform.Rotate(Vector3.up, (1f / (1440f / 4f)) * 360f, Space.World);

        // Calculate the percentage of the day that has passed (in 24 hours)
        float timePercent = (hours * 60f + minutes) / 1440f; // 1440 = 24 * 60 (total minutes in a day)

        // Set the sun's rotation around the X-axis for sunrise/set effect, with 170 degrees Y offset
        float rotationX = timePercent * 360f;
        targetRotation = Quaternion.Euler(new Vector3(rotationX - 90f, 170f, 0));

        if (value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 20f));
            StartCoroutine(LerpLight(graddientNightToSunrise, 20f));
        }
        else if (value == 9)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 20f));
            StartCoroutine(LerpLight(graddientSunriseToDay, 20f));
        }
        else if (value == 18)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 20f));
            StartCoroutine(LerpLight(graddientDayToSunset, 20f));
        }
        else if (value == 20)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 20f));
            StartCoroutine(LerpLight(graddientSunsetToNight, 20f));
        }
    }

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}