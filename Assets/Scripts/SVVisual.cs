using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class SVVisual : MonoBehaviour
{
    [SerializeField] private Slider hueSlider;

    private HexInputController hexInputController;

    private Texture2D svTexture;
    private int width, height;

    public Action<float> OnHueChanged;

    private void Awake()
    {
        hueSlider.onValueChanged.AddListener(UpdateSVImage);
        hexInputController = FindAnyObjectByType<HexInputController>();
    }

    private void Start()
    {
        svTexture = (Texture2D)GetComponent<RawImage>().mainTexture;

        // Получаем размеры текстуры
        width = svTexture.width;
        height = svTexture.height;
    }

    private void OnEnable()
    {
        hexInputController.OnHexEntered += (h, _, _) => hueSlider.value = h;
    }

    private void OnDisable()
    {
        hexInputController.OnHexEntered -= (h, _, _) => hueSlider.value = h;
    }

    private void UpdateSVImage(float h)
    {
        OnHueChanged?.Invoke(h);

        // Создаем массив цветов для каждого пикселя
        Color32[] pixels = new Color32[width * height];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                //Обязательно 1f, а не просто 1. Иначе результат деления будет int. 
                float s = x / (width - 1f);
                float v = y / (height - 1f);

                pixels[y * width + x] = Color.HSVToRGB(h, s, v);
            }

        svTexture.SetPixels32(pixels);
        svTexture.Apply();
    }
}
