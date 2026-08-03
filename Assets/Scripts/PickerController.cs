using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PickerController : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    [SerializeField] private Image picker;
    private HexInputController hexInputController;

    private RectTransform svTransform, pickerTransform;
    private Canvas canvas;
    private Camera camera;
    
    private Vector2 halfSVSize;

    //S, V
    public Action<float, float> OnPickerPosChanged;

    private void OnEnable()
    {
        hexInputController.OnHexEntered += (_, s, v) => SetPickerPositionFromSV(s, v);
    }

    private void OnDisable()
    {
        hexInputController.OnHexEntered -= (_, s, v) => SetPickerPositionFromSV(s, v);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MovePicker(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MovePicker(eventData.position);
    }

    private void Awake()
    {
        svTransform = GetComponent<RectTransform>();
        pickerTransform = picker.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        camera = Camera.main;
        hexInputController = FindAnyObjectByType<HexInputController>();

        halfSVSize = svTransform.sizeDelta * 0.5f;

        pickerTransform = picker.rectTransform;
    }

    private void Start()
    {
        //Перемещаем в нижний левый угол
        MovePicker(new(-(halfSVSize.x), -(halfSVSize.y)));
    }

    private void MovePicker(Vector2 pointerPos)
    {
        Vector2 position;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            position = svTransform.InverseTransformPoint(pointerPos);
        else
            RectTransformUtility.ScreenPointToLocalPointInRectangle(svTransform, pointerPos, camera, out position);

        position.x = Mathf.Clamp(position.x, -halfSVSize.x, halfSVSize.x);
        position.y = Mathf.Clamp(position.y, -halfSVSize.y, halfSVSize.y);

        pickerTransform.localPosition = position;

        Vector2 normalizedPos = GetNormalizedPos(position);

        //Меняем цвет, чтобы указатель всегда было хорошо видно
        picker.color = normalizedPos.y > 0.5f ? Color.black : Color.white;

        OnPickerPosChanged?.Invoke(normalizedPos.x, normalizedPos.y);
    }

    private void SetPickerPositionFromSV(float S, float V)
    {
        float x = Mathf.Lerp(-halfSVSize.x, halfSVSize.x, S);
        float y = Mathf.Lerp(-halfSVSize.y, halfSVSize.y, V);

        Vector2 pos = new(x, y);

        pickerTransform.transform.localPosition = pos;

        Vector2 normalizedPos = GetNormalizedPos(pos);

        OnPickerPosChanged?.Invoke(normalizedPos.x, normalizedPos.y);
    }

    private Vector2 GetNormalizedPos(Vector2 position)
    {
        Vector2 normalizedPos;

        normalizedPos.x = (position.x + halfSVSize.x) / svTransform.sizeDelta.x;
        normalizedPos.y = (position.y + halfSVSize.y) / svTransform.sizeDelta.y;

        return normalizedPos;
    }
}
