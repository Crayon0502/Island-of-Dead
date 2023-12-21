using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScaleButtonOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Vector3 originalScale;

    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale; // 버튼의 원래 스케일 저장
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1.2f); // 강조 스케일 (예: 1.2배)
        transform.localScale = highlightScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // 원래 스케일로 복원
    }
}
