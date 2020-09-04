using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeColors : MonoBehaviour
{
    public Material unobtainableObjMat; // Unobtainable object's material
    public Material groundBasicMat;  // Ground, gate and pillars' material
    public Material holeMaterial;  // Hole's material (for changing border color) 
    public Material backgroundMat; // Background platform's material

    float r;
    float g;
    float b;

    private void Start()
    {
        r = Random.Range(.4f , 1f);
        g = Random.Range(.4f, 1f);
        b = Random.Range(.4f, 1f);

        unobtainableObjMat.color = new Color(1 - r, 1 - g, 1 - b); // There is a contrast between the ground and unobtainable objects.
        backgroundMat.color = new Color(1 - (r / 2), 1 - (g / 2), 1 - (b / 2));
        groundBasicMat.color = new Color(r,g,b);
        holeMaterial.SetColor("Color_12918664", new Color(r-0.2f, g-0.2f, b-0.2f));
        
        
    }

}
