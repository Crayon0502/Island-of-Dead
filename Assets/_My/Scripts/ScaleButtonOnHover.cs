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
        originalScale = transform.localScale; // ��ư�� ���� ������ ����
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1.2f); // ���� ������ (��: 1.2��)
        transform.localScale = highlightScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // ���� �����Ϸ� ����
    }
}
