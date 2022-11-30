using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentRoad : MonoBehaviour
{
    //Variables
    private Renderer rend;
    //readonly List<Color> materialColors = new List<Color>();
    //Color[] materialColors;
    private bool transparent = false;

    private Color color0;
    private Color color1;

    private float changeRate = 5f;

    private void Start()
    {
        //Get the renderer of the object
        rend = GetComponent<MeshRenderer>();
        //Get the material color
        Material[] mats = rend.materials;

        color0 = mats[0].color;
        color1 = mats[1].color;

        //foreach (Material mat in mats)
        //    materialColors.Add(mat.color);
    }

    public void ChangeTransparency(bool transparent)
    {
        //Avoid to set the same transparency twice
        if (this.transparent == transparent) return;

        //Set the new configuration
        this.transparent = transparent;

        //Check if should be transparent or not
        if (transparent)
        {
            StartCoroutine(SetTransparent());
        }
        else
        {
            StartCoroutine(SetOpaque());
        }
        //Set the new Color
        //rend.material.color = materialColor;
    }

    private IEnumerator SetTransparent()
    {
        while (color0.a > 0.3f)
        {
            color0.a -= Time.deltaTime * changeRate;
            rend.materials[0].color = color0;
            color1.a -= Time.deltaTime * changeRate;
            rend.materials[1].color = color1;
            yield return null;
        }
        color0.a = 0.3f;
        rend.materials[0].color = color0;
        color1.a = 0.3f;
        rend.materials[1].color = color1;
    }

    private IEnumerator SetOpaque()
    {
        while (color0.a < 1f)
        {
            color0.a += Time.deltaTime * changeRate;
            rend.materials[0].color = color0;
            color1.a += Time.deltaTime * changeRate;
            rend.materials[1].color = color1;
            yield return null;
        }
        color0.a = 1f;
        rend.materials[0].color = color0;
        color1.a = 1f;
        rend.materials[1].color = color1;
    }
}
