using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFirstPlayer : MonoBehaviour
{
    public Slider slider;
    public TMP_Text txtTimer;
    public float maxTime = 30f;
    public bool isTurn = false;
    private Coroutine timerCoroutine;

    private void Update()
    {
        if (isTurn && timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(StartTimer());
        }
        else if (!isTurn && timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            slider.value = 1;
            this.txtTimer.text = maxTime.ToString();
        }
    }

    IEnumerator StartTimer()
    {
        float currentTime = maxTime;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateSliderValue(currentTime);
            yield return null;
        }

        isTurn = false;
        slider.value = 1;
        timerCoroutine = null; //코루틴이 완료되었으므로 null로 설정
        EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.TimeOver, 0);
    }

    void UpdateSliderValue(float currentTime)
    {
        float normalizedTime = currentTime / maxTime;
        slider.value = Mathf.Clamp01(normalizedTime);
        this.txtTimer.text = ((int)(normalizedTime * maxTime)).ToString(); //타이머 텍스트 숫자 감소
    }

}
