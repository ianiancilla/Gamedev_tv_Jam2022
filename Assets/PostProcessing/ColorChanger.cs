using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class ColorChanger : MonoBehaviour
{
    PostProcessVolume postProcessVolume;
    ColorGrading colorGrading;

    private void Awake()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);

    }

    public void ChangeColorGrading(Color newColor)
    {
        colorGrading.colorFilter.value = newColor;
    }


}
