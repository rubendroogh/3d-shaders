using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    //lightsource
    private Light sun;

    // Lensflare
    private LensFlare lensFlare;
    private Flare lensFlareTrail;
    private Color lensFlareOn = new Vector4(1,1,1,0);
    private Color lensFlareOff = new Vector4(0, 0, 0, 0);

    // Skybox
    public Material skybox;
    public Color SkyTintUp;
    public Color SkyTintDown;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skybox;
        this.sun = this.GetComponent<Light>();
        this.lensFlare = this.GetComponent<LensFlare>();
        this.sun.flare = this.lensFlareTrail;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //moving the sun
        sun.transform.Rotate(new Vector3(-0.002f, 0, 0));

        // changing colors and flares based on position
        this.SunSetRise();
    }

    /// <summary>
    /// This Function checks where the sun is and changes its effects accordingly.
    /// </summary>
    void SunSetRise()
    {
        float interpolateIndex = 0;

        if (sun.transform.eulerAngles.x <= 10 || sun.transform.eulerAngles.x > 170 && sun.transform.eulerAngles.x < 180)
        {
            if (sun.transform.eulerAngles.x <= 10) interpolateIndex = sun.transform.eulerAngles.x / 10;
            else if (sun.transform.eulerAngles.x <= 180) interpolateIndex = (sun.transform.eulerAngles.x - 170) / 10;

            this.sun.color = Color.Lerp(SkyTintDown, SkyTintUp, interpolateIndex);
            RenderSettings.skybox.SetColor("_SkyTint", sun.color);
            this.lensFlare.color = Color.Lerp(lensFlareOff, lensFlareOn, interpolateIndex / 0.7f);
        }
        else if (sun.transform.eulerAngles.x > 350 && sun.transform.eulerAngles.x < 360)
        {
            interpolateIndex = (sun.transform.eulerAngles.x - 350) / 10;
           
            this.sun.color = Color.Lerp(Color.black, SkyTintDown, interpolateIndex);
            RenderSettings.skybox.SetColor("_SkyTint", sun.color);
            this.lensFlare.color = lensFlareOff;
        }
        else if (sun.transform.eulerAngles.x > 180)
        {
            interpolateIndex = (sun.transform.eulerAngles.x - 180) / 10;

            this.sun.color = Color.Lerp(SkyTintDown, Color.black, interpolateIndex);
            RenderSettings.skybox.SetColor("_SkyTint", sun.color);
            this.lensFlare.color = lensFlareOff;
        }
        else
        {
            this.sun.color = SkyTintUp;
            RenderSettings.skybox.SetColor("_SkyTint", SkyTintUp);
            this.lensFlare.color = lensFlareOn;
        }
    }
}
