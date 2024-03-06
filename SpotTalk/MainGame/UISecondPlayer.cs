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
            Debug.Log("세컨드 턴 들어옴");
            Debug.LogFormat("<color=green>코루틴 시작</color>");
            timerCoroutine = StartCoroutine(StartTimer());
        }
        else if (!isTurn && timerCoroutine != null)
        {
            Debug.LogFormat("<color=yellow>코루틴 멈추기</color>");
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            slider.value = 1;
            this.txtTimer.text = maxTime.ToString();
        }

    }

    IEnumerator StartTimer()
    {
        Debug.Log("세컨드 코루틴 시작");
        float currentTime = maxTime;

        while (currentTime > 0)
        {
            //시간이 흐를 때마다 감소
            currentTime -= Time.deltaTime;

            //슬라이더 갱신
            UpdateSliderValue(currentTime);

            yield return null;
        }

        //시간이 다 되면 원하는 동작 수행
        this.isTurn = false;
        this.slider.value = 1;
        timerCoroutine = null; //코루틴이 완료되었으므로 null로 설정
        EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.TimeOver, 0);

    }
    void UpdateSliderValue(float currentTime)
    {
        // 슬라이더 값 갱신
        float normalizedTime = currentTime / maxTime;
        slider.value = Mathf.Clamp01(normalizedTime); //0에서 1 사이로 클램프
        this.txtTimer.text = ((int)(normalizedTime * maxTime)).ToString(); //타이머 텍스트 숫자 감소
    }
}
