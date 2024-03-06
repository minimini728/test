using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayMain : MonoBehaviour
{
    public TwoPlayUIDirector director;

    private Color firstPlayerColor;
    private Color secondPlayerColor;
    void Start()
    {
        //첫 번째 플레이어 색상 저장 이벤트 등록
        EventDispatcher.instance.AddEventHandler<Color>((int)EventEnum.eEventType.FirstColorPick, FirstPlayerColorPick);
        //두 번째 플레이어 색상 저장 이벤트 등록
        EventDispatcher.instance.AddEventHandler<Color>((int)EventEnum.eEventType.SecondColorPick, SecondPlayerColorPick);

        //타임오버 이벤트 등록
        EventDispatcher.instance.AddEventHandler<int>((int)EventEnum.eEventType.TimeOver, TimeOver);

    }
    private void FirstPlayerColorPick(short type, Color color)
    {
        this.firstPlayerColor = color; //색상 값 저장
    }

    private void SecondPlayerColorPick(short type, Color color)
    {
        this.secondPlayerColor = color; //색상 값 저장

        //UI캔버스 텍스트 초기화
        this.director.TextInit(this.firstPlayerColor, this.secondPlayerColor);
    }
    private void TimeOver(short type, int a)
    {
        if (a == 0)
        {
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.Win, secondPlayerColor); //승리 UI팝업 보여주기

        }
        else
        {
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.Win, firstPlayerColor); //승리 UI팝업 보여주기

        }
    }

    private void OnDisable()
    {
        //첫 번째 플레이어 색상 저장 이벤트 제거
        EventDispatcher.instance.RemoveEventHandler<Color>((int)EventEnum.eEventType.FirstColorPick, FirstPlayerColorPick);
        //두 번째 플레이어 색상 저장 이벤트 제거
        EventDispatcher.instance.RemoveEventHandler<Color>((int)EventEnum.eEventType.SecondColorPick, SecondPlayerColorPick);

        //타임오버 이벤트 제거
        EventDispatcher.instance.RemoveEventHandler<int>((int)EventEnum.eEventType.TimeOver, TimeOver);

    }
}
