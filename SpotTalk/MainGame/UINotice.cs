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
        this.txtNotice.text = num + "�������� ���� �� �ֽ��ϴ�.";
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

        // ���İ��� 0�� �Ǹ� ��Ȱ��ȭ
        this.gameObject.SetActive(false);
    }

}
