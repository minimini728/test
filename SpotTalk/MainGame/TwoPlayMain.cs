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
        //ù ��° �÷��̾� ���� ���� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler<Color>((int)EventEnum.eEventType.FirstColorPick, FirstPlayerColorPick);
        //�� ��° �÷��̾� ���� ���� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler<Color>((int)EventEnum.eEventType.SecondColorPick, SecondPlayerColorPick);

        //Ÿ�ӿ��� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler<int>((int)EventEnum.eEventType.TimeOver, TimeOver);

    }
    private void FirstPlayerColorPick(short type, Color color)
    {
        this.firstPlayerColor = color; //���� �� ����
    }

    private void SecondPlayerColorPick(short type, Color color)
    {
        this.secondPlayerColor = color; //���� �� ����

        //UIĵ���� �ؽ�Ʈ �ʱ�ȭ
        this.director.TextInit(this.firstPlayerColor, this.secondPlayerColor);
    }
    private void TimeOver(short type, int a)
    {
        if (a == 0)
        {
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.Win, secondPlayerColor); //�¸� UI�˾� �����ֱ�

        }
        else
        {
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.Win, firstPlayerColor); //�¸� UI�˾� �����ֱ�

        }
    }

    private void OnDisable()
    {
        //ù ��° �÷��̾� ���� ���� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler<Color>((int)EventEnum.eEventType.FirstColorPick, FirstPlayerColorPick);
        //�� ��° �÷��̾� ���� ���� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler<Color>((int)EventEnum.eEventType.SecondColorPick, SecondPlayerColorPick);

        //Ÿ�ӿ��� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler<int>((int)EventEnum.eEventType.TimeOver, TimeOver);

    }
}
