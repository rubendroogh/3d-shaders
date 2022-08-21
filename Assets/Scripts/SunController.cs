using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SunController : MonoBehaviour
{
    //lightsource
    private Light sun;

    // Lensflare
    private LensFlare lensFlare;
    private Flare lensFlareTrail;
    //private Color lensFlareOn = new Vector4(1,1,1,0);
    //private Color lensFlareOff = new Vector4(0, 0, 0, 0);
    //private Vector4 test = new Vector4(1, 1, 1, 0);

    // Skybox
    [Space]
    [Header("- Skybox has to be a martbox")]
    public Material skybox;
    [Range(0.0f, 360.0f)]
    public float sunriseAngle;
    [Range(0.0f, 360.0f)]
    public float sunAngle;
    [Range(0.0f, 0.5f)]
    public float sunRotationSpeed;

    [Space]
    [Header("------------ Sun phases")]
    public SunPhase[] sunPhases;

    private int sunPhaseIndex = 0;

    private Color skyTint;
    private Color groundTint;
    private Color HorizonColor;
    private float skyTintStrength;
    private float groundTintStrength;

    private Color sunColor;
    private float sunIntensity;
    private float sunRadius;

    private Color sunGlowColor;
    private float sunGlowRadius;
    private float sunGlowIntensity;
    private float sunGLowAltitudeSupressor;

    private float lightingTemperature;
    private float lightingIntensity;

    void Start()
    {
        RenderSettings.skybox = skybox;
        this.sun = this.GetComponent<Light>();
        this.lensFlare = this.GetComponent<LensFlare>();
        this.sun.flare = this.lensFlareTrail;
        this.sunPhaseIndex = findSunPhase();
        UpdateSunPhase(true);
    }

    private void Update()
    {
        if (this.sunPhases.Length > 0)
        {
            this.UpdateSunPhase();
            this.SetSunData();
        }
    }

    void FixedUpdate()
    {
        this.MoveSun();
    }

    /// <summary>
    /// This function moves all the skybox/sun data over to the shader and directional light.
    /// </summary>
    void SetSunData()
    {
        RenderSettings.skybox.SetFloat("_SkyColorTopModifier", this.skyTintStrength);
        RenderSettings.skybox.SetFloat("_SkyColorBottomModifier", this.groundTintStrength);
        RenderSettings.skybox.SetFloat("_SunIntensity", this.sunIntensity);
        RenderSettings.skybox.SetFloat("_SunRiseAngle", Mathf.Deg2Rad * this.sunriseAngle);
        RenderSettings.skybox.SetFloat("_SunBlend", this.sunGlowRadius);
        RenderSettings.skybox.SetFloat("_SunRadius", this.sunRadius);
        RenderSettings.skybox.SetFloat("_SunGlowIntensity", this.sunGlowIntensity);
        RenderSettings.skybox.SetFloat("_SunAngle", Mathf.Deg2Rad * this.sunAngle);
        RenderSettings.skybox.SetFloat("_AltitudeGlowModifier", this.sunGLowAltitudeSupressor);
        RenderSettings.skybox.SetColor("_SkyColorTop", this.skyTint);
        RenderSettings.skybox.SetColor("_SkyColorBottom", this.groundTint);
        RenderSettings.skybox.SetColor("_SunColor", this.sunColor);
        RenderSettings.skybox.SetColor("_SunGlowColor", this.sunGlowColor);
        RenderSettings.skybox.SetColor("_SkyColorHorizon", this.HorizonColor);
        this.sun.colorTemperature = lightingTemperature;
        this.sun.intensity = lightingIntensity;
        this.sun.transform.rotation = Quaternion.Euler(this.sunAngle, this.sunriseAngle + 180,0);
    }

    /// <summary>
    /// Finds the current sun phase based on angle of the sun
    /// </summary>
    /// <returns></returns>
    int findSunPhase()
    {
        int i = 0;
        foreach (SunPhase sunPhase in this.sunPhases)
        {
            if (this.sunAngle >= sunPhase.startAngle && this.sunAngle <= sunPhase.endAngle) {
                return i;
            }
            i++;
        }

        return 0;
    }

    /// <summary>
    /// This Function moves the sun by the the sunrotationspeed.
    /// </summary>
    void MoveSun()
    {
        if (this.sunAngle > 360)
        {
            this.sunAngle = this.sunAngle - 360;
        }
        this.sunAngle = this.sunAngle + this.sunRotationSpeed;
    }

    /// <summary>
    /// This Function checks where the sun is and changes its effects accordingly.
    /// </summary>
    void UpdateSunPhase(bool force = false)
    {
        float startAngle = this.sunPhases[this.sunPhaseIndex].startAngle;
        float endAngle = this.sunPhases[this.sunPhaseIndex].endAngle;
        float fadeInLength = this.sunPhases[this.sunPhaseIndex].fadeInLength;


        if (this.sunAngle > endAngle || this.sunAngle < startAngle + fadeInLength || !Application.isPlaying || force == true)
        {
            float fadeIndex = (this.sunAngle - startAngle) / fadeInLength;

            if (this.sunPhaseIndex >= this.sunPhases.Length)
            {
                this.sunPhaseIndex = 0;
            }

            SunPhase prevSunPhase;
            SunPhase sunPhase = this.sunPhases[this.sunPhaseIndex];

            if (this.sunPhaseIndex == 0)
            {
                prevSunPhase = this.sunPhases[this.sunPhases.Length - 1];
            }
            else
            {
                prevSunPhase = this.sunPhases[this.sunPhaseIndex - 1];
            }

               
            this.skyTintStrength = Mathf.Lerp(prevSunPhase.skyTintStrength, sunPhase.skyTintStrength, fadeIndex);
            this.groundTintStrength = Mathf.Lerp(prevSunPhase.groundTintStrength, sunPhase.groundTintStrength, fadeIndex);
            this.sunIntensity = Mathf.Lerp(prevSunPhase.sunIntensity, sunPhase.sunIntensity, fadeIndex);
            this.sunGlowRadius = Mathf.Lerp(prevSunPhase.sunGlowRadius, sunPhase.sunGlowRadius, fadeIndex);
            this.sunRadius = Mathf.Lerp(prevSunPhase.sunRadius, sunPhase.sunRadius, fadeIndex);
            this.sunGlowIntensity = Mathf.Lerp(prevSunPhase.sunGlowIntensity, sunPhase.sunGlowIntensity, fadeIndex);
            this.sunGLowAltitudeSupressor = Mathf.Lerp(prevSunPhase.sunGLowAltitudeSupressor, sunPhase.sunGLowAltitudeSupressor, fadeIndex);
            this.skyTint = Color.Lerp(prevSunPhase.skyTint, sunPhase.skyTint, fadeIndex);
            this.groundTint = Color.Lerp(prevSunPhase.groundTint, sunPhase.groundTint, fadeIndex);
            this.sunColor = Color.Lerp(prevSunPhase.sunColor, sunPhase.sunColor, fadeIndex);
            this.sunGlowColor = Color.Lerp(prevSunPhase.sunGlowColor, sunPhase.sunGlowColor, fadeIndex);
            this.HorizonColor = Color.Lerp(prevSunPhase.HorizonColor, sunPhase.HorizonColor, fadeIndex);
            this.lightingTemperature = Mathf.Lerp(prevSunPhase.lightingTemperature, sunPhase.lightingTemperature, fadeIndex);
            this.lightingIntensity = Mathf.Lerp(prevSunPhase.lightingIntensity, sunPhase.lightingIntensity, fadeIndex);
            this.sunPhaseIndex = this.findSunPhase();
        }
    }
}

[System.Serializable]
public class SunPhase
{
    public float startAngle;
    public float endAngle;

    [Space]
    [Header("Amount of degrees untill the fade in is complete")]
    public float fadeInLength;

    [Header("-Skybox options -")]
    public Color skyTint;
    public Color groundTint;
    public Color HorizonColor;
    [Range(0.0f, 40.0f)]
    public float skyTintStrength;
    [Range(0.0f, 40.0f)]
    public float groundTintStrength;

    [Space]
    [Header("-Sun options -")]
    public Color sunColor;
    [Range(0.0f, 50.0f)]
    public float sunIntensity;
    [Range(0.0f, 1.0f)]
    public float sunRadius;
    [Space]
    public Color sunGlowColor;
    [Range(0.0f, 1.0f)]
    public float sunGlowRadius;
    [Range(0.0f, 20.0f)]
    public float sunGlowIntensity;
    [Range(0.0f, 5.0f)]
    public float sunGLowAltitudeSupressor;
    [Space]
    [Header("-Lighting options -")]
    [Range(0.0f, 20.0f)]
    public float lightingIntensity;
    [Range(1500.0f, 20000.0f)]
    public float lightingTemperature;
}