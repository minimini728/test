using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISecondPlayer : MonoBehaviour
{
    public Slider slider;
    public TMP_Text txtTimer;
    public float maxTime = 30f;
    public bool isTurn = false;
    private Coroutine timerCoroutine;

    void Start()
    {

    }

    private void Update()
    {
        if (isTurn && timerCoroutine == null)
        {
            Debug.Log("������ �� ����");
            Debug.LogFormat("<color=green>�ڷ�ƾ ����</color>");
            timerCoroutine = StartCoroutine(StartTimer());
        }
        else if (!isTurn && timerCoroutine != null)
        {
            Debug.LogFormat("<color=yellow>�ڷ�ƾ ���߱�</color>");
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            slider.value = 1;
            this.txtTimer.text = maxTime.ToString();
        }

    }

    IEnumerator StartTimer()
    {
        Debug.Log("������ �ڷ�ƾ ����");
        float currentTime = maxTime;

        while (currentTime > 0)
        {
            //�ð��� �带 ������ ����
            currentTime -= Time.deltaTime;

            //�����̴� ����
            UpdateSliderValue(currentTime);

            yield return null;
        }

        //�ð��� �� �Ǹ� ���ϴ� ���� ����
        this.isTurn = false;
        this.slider.value = 1;
        timerCoroutine = null; //�ڷ�ƾ�� �Ϸ�Ǿ����Ƿ� null�� ����
        EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.TimeOver, 0);

    }
    void UpdateSliderValue(float currentTime)
    {
        // �����̴� �� ����
        float normalizedTime = currentTime / maxTime;
        slider.value = Mathf.Clamp01(normalizedTime); //0���� 1 ���̷� Ŭ����
        this.txtTimer.text = ((int)(normalizedTime * maxTime)).ToString(); //Ÿ�̸� �ؽ�Ʈ ���� ����
    }
}
