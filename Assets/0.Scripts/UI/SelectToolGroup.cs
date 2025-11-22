using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectToolGroup : MonoBehaviour
{
    [Header("도구 선택 슬롯 리스트")]
    [SerializeField] private List<RectTransform> slots = new List<RectTransform>();

    [SerializeField] private float radius = 150f;       //원형의 반지름
    [SerializeField] private float degree = 360f;       //원형의 둘레, 360 → 원 전체를 두르기

    [SerializeField] private float showTime = 0.3f;     //UI를 키는 시간
    [SerializeField] private float hideTime = 0.3f;     //UI를 끄는 시간

    private bool isActive = false;      //도구 선택 슬롯 UI가 활성화 되었는지

    private void Awake()
    {
        InputSystem.actions["ToolSelect"].started += ctx =>
        {
            if (!isActive)
                ShowSelectToolGroup();
            else
                HideSelectToolGroup();
        };
    }

    //도구 선택 슬롯을 키는 트위닝 메서드
    public void ShowSelectToolGroup()
    {
        Sequence sequence = DOTween.Sequence();

        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(true);

            float angle = -i * (degree / slots.Count) + 90f;
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            var rect = slots[i].GetComponent<RectTransform>();
            sequence.Join(rect.DOAnchorPos(new Vector2(x, y), showTime).SetEase(Ease.OutBack));
        }

        sequence.Play();

        isActive = true;
    }

    //도구 선택 슬롯을 끄는 트위닝 메서드
    public void HideSelectToolGroup()
    {
        Sequence sequence = DOTween.Sequence();

        for(int i = 0; i < slots.Count; i++)
        {
            var rect = slots[i].GetComponent<RectTransform>();
            sequence.Join(rect.DOAnchorPos(new Vector2(0, 0), hideTime).SetEase(Ease.InBack));
        }

        sequence.Play().OnComplete(() =>
        {
            foreach(var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        });

        isActive = false;
    }
}
