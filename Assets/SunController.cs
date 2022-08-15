using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    Light sun;
    public Color highSunColor = new Color(219, 129, 59, 255);
    public Color lowSunColor = new Color(255, 49, 0, 255);

    // Start is called before the first frame update
    void Start()
    {
        sun = this.GetComponent<Light>();
        //sun.color = highSunColor;
        RenderSettings.skybox.SetColor("_Tint", lowSunColor);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sun.transform.Rotate(new Vector3(-0.01f, 0, 0));
        if (sun.transform.eulerAngles.x <= 10)
        {
            
            //sun.color = Color.Lerp(lowSunColor, highSunColor, sun.transform.eulerAngles.x / 10);
            //RenderSettings.skybox.SetColor("_Tint", Color.red);
        }
        //Debug.Log(sun.transform.rotation);
    }
}
