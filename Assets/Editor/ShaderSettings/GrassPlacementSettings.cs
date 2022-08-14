using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrassPlacementSettings", menuName = "Ruben/GrassPlacementSettings")]
public class GrassPlacementSettings : ScriptableObject
{
    public Mesh sourceMesh;
    public int sourceMeshIndex;
    public Vector3 scale;
    public Vector3 rotation;
    public float height;
}
