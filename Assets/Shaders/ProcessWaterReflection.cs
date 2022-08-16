using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessWaterReflection : MonoBehaviour
{
    public RenderTexture renderTexture;
    private Material material;
    public Texture2D test;

    void Start()
    {
        this.material = this.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        this.material.SetTexture("weeb", test);
    }
}
