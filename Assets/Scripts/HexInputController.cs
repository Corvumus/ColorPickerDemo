using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class HexInputController : MonoBehaviour
{
    private TMP_InputField hexInputField;
    private OutputVisual outputVisual;

    //H, S, V
    public Action<float, float, float> OnHexEntered;

    private void Awake()
    {
        outputVisual = FindAnyObjectByType<OutputVisual>();
        hexInputField = GetComponent<TMP_InputField>();

        hexInputField.onValidateInput = ValidateAndConvertHex;
        hexInputField.onEndEdit.AddListener(OnTextInput);
    }

    private void OnEnable()
    {
        outputVisual.OnColorChanged += UpdateHex;
    }

    private void OnDisable()
    {
        outputVisual.OnColorChanged -= UpdateHex;
    }

    private char ValidateAndConvertHex(string text, int charIndex, char addedChar)
    {
        // Делаем заглавным
        char upperChar = char.ToUpper(addedChar);

        // Проверяем, шестнадцатеричный ли это символ
        if ((upperChar >= '0' && upperChar <= '9') ||
            (upperChar >= 'A' && upperChar <= 'F'))
            return upperChar;

        return '\0';
    }

    private void OnTextInput(string input)
    {
        if (input.Length != 6)
            return;

        if (ColorUtility.TryParseHtmlString($"#{input}", out Color newColor))
        {
            Color.RGBToHSV(newColor, out float H, out float S, out float V);

            OnHexEntered?.Invoke(H, S, V);
        }
    }

    private void UpdateHex(Color color)
    {
        hexInputField.text = ColorUtility.ToHtmlStringRGB(color);
    }
}
