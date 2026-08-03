using UnityEngine;
using UnityEngine.UI;

public class ColorPaletteInitializer : MonoBehaviour
{
    [SerializeField] private RawImage hueImage, SVImage;
    private Texture2D hueTexture, SVTexture;
    private float currentHue;

    private void Awake()
    {
        CreateHueTexture();
        CreateSVTexture();
    }

    private void CreateHueTexture()
    {
        int height = 16;
        currentHue = 0;

        hueTexture = new Texture2D(1, height)
        {
            wrapMode = TextureWrapMode.Clamp,
            name = "HueTexture"
        };

        Color32[] pixels = new Color32[height];

        for (int i = 0; i < height; i++)
        {
            float t = (float)i / (height - 1);
            pixels[i] = Color.HSVToRGB(t, 1f, 1f);
        }

        hueTexture.SetPixels32(pixels);
        hueTexture.Apply();

        hueImage.texture = hueTexture;
    }

    private void CreateSVTexture()
    {
        int size = 16;

        SVTexture = new(size, size)
        {
            wrapMode = TextureWrapMode.Clamp,
            name = "SVTexture"
        };

        Color32[] pixels = new Color32[size * size];

        for (int y = 0; y < SVTexture.height; y++)
            for (int x = 0; x < SVTexture.width; x++)
                pixels[y * size + x] = Color.HSVToRGB(
                    currentHue,
                    x / (SVTexture.width - 1f),
                    y / (SVTexture.height - 1f));

        SVTexture.SetPixels32(pixels);
        SVTexture.Apply();

        SVImage.texture = SVTexture;
    }

    private void OnDestroy()
    {
        Destroy(SVTexture);
        Destroy(hueTexture);
    }
}
