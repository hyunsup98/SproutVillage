using System.Collections;
using UnityEngine;

public class HouseRoof : MonoBehaviour
{
    [SerializeField] private SpriteRenderer roofRenderer;   //이미지 알파값을 조정할 렌더러
    [SerializeField] private float fadeTime = 0.5f;            //페이드 전환이 시작되고 끝날때 까지의 시간
    private float fadeTimer = 0f;
    private Color color;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        TryGetComponent(out roofRenderer);
    }

    private void ImageAlphaFade(float prevAlpha, float nextAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(CoImageAlphaFade(prevAlpha, nextAlpha));
    }

    /// <summary>
    /// 렌더러의 이미지 알파값을 조정하는 메서드
    /// </summary>
    /// <param name="prevAlpha"> 기존의 알파값 </param>
    /// <param name="nextAlpha"> 변경할 알파값 </param>
    /// <returns></returns>
    private IEnumerator CoImageAlphaFade(float prevAlpha, float nextAlpha)
    {
        //타이머 초기화
        fadeTimer = 0f;

        //color에 현재 이미지의 컬러값을 넣어줌
        color = roofRenderer.color;

        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;

            color.a = Mathf.Lerp(prevAlpha, nextAlpha, fadeTimer / fadeTime);
            roofRenderer.color = color;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //현재 알파값을 0으로 전환
            ImageAlphaFade(roofRenderer.color.a, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //현재 알파값을 1로 전환
            ImageAlphaFade(roofRenderer.color.a, 1);
        }
    }
}
