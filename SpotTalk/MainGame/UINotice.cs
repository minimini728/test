using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotice : MonoBehaviour
{
    public Text txtNotice;
    public float fadeDuration = 2f;

    public void Init(int num)
    {
        this.txtNotice.text = num + "구역에만 놓을 수 있습니다.";
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;
        Color initialColor = this.gameObject.GetComponent<Image>().color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            this.gameObject.GetComponent<Image>().color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 알파값이 0이 되면 비활성화
        this.gameObject.SetActive(false);
    }

}
