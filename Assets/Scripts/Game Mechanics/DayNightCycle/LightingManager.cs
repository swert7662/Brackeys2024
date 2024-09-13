
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset lightingPreset;

    [SerializeField, Range(0, 24)] private float timeOfDay;

    private void Update()
    {
        if (lightingPreset == null)
            return;

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = lightingPreset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = lightingPreset.FogColor.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = lightingPreset.DirectionalColor.Evaluate(timePercent);

            float rotation = timePercent * 360f;
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(rotation - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}
