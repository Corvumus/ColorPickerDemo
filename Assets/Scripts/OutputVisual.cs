using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OutputVisual : MonoBehaviour
{
    private Image outputImage;
    private PickerController pickerController;
    private SVVisual svVisual;

    private float currentHue, currentSaturation, currentValue;

    public Action<Color> OnColorChanged;

    private void Awake()
    {
        outputImage = GetComponent<Image>();
        pickerController = FindAnyObjectByType<PickerController>();
        svVisual = FindAnyObjectByType<SVVisual>();
    }

    private void OnEnable()
    {
        pickerController.OnPickerPosChanged += SetSV;
        svVisual.OnHueChanged += SetHue;
    }

    private void OnDisable()
    {
        pickerController.OnPickerPosChanged -= SetSV;
        svVisual.OnHueChanged -= SetHue;
    }

    public void SetSV(float s, float v)
    {
        currentSaturation = s;
        currentValue = v;

        UpdateOutputImage();
    }

    public void SetHue(float h)
    {
        currentHue = h;

        UpdateOutputImage();
    }

    private void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);

        OnColorChanged?.Invoke(currentColor);

        outputImage.color = currentColor;
    }
}
