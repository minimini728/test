using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayUIDirector : MonoBehaviour
{
    public UIColorPicker uiColorPicker; //���� ���� �ȷ�Ʈ UI
    public UIBoard uiBoard; //������ UI
    public UINotice uiNotice; //�˸��� UI
    public UIWin uiWin; //�¸� UI

    public UIFirstPlayer uiFirstPlayer; //ù ��° �÷��̾� ������ UI
    public UISecondPlayer uiSecondPlayer; //�� ��° �÷��̾� ������ UI

    public Color colorFirstPlayer; //ù ��° �÷��̾� ����
    public Color colorSecondPlayer; //�� ��° �÷��̾� ����
    void Start()
    {
        //�˸��� �����ִ� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler<int>((int)EventEnum.eEventType.ShowNoticeUI, ShowNoticeUI);

        //���� �Ͽ� ���� �÷��̾� �����̴� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler<int>((int)EventEnum.eEventType.ChangePlayer, TurnSliderInit);

        //���� �¸� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler<Color>((int)EventEnum.eEventType.Win, ShowWinUI);

        //���� ����� �����̴� ���� �̺�Ʈ ���
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.StopSlider, StopSlider);
    }

    //����, ���� �ؽ�Ʈ �ʱ�ȭ �޼���
    public void TextInit(Color fistPlayerColor, Color secondPlayerColor)
    {
        this.uiFirstPlayer.gameObject.SetActive(true);
        this.colorFirstPlayer = fistPlayerColor;

        //�����̴�
        this.uiFirstPlayer.isTurn = true;

        this.uiSecondPlayer.gameObject.SetActive(true);
        this.colorSecondPlayer = secondPlayerColor;

        //�׸���Ŵ��� �ʱ�ȭ
        this.uiBoard.Init(this.colorFirstPlayer, this.colorSecondPlayer);

        //������ �ʱ�ȭ
        this.PlayerProfileInit();
    }

    private void PlayerProfileInit()
    {
        int firstColorNum = GetChildIndexByColor(this.colorFirstPlayer); //ù ��° �÷��̾� ���� int�� ����
        this.uiFirstPlayer.transform.GetChild(2).GetChild(firstColorNum).gameObject.SetActive(true); //ù ��° �÷��̾� ���� �̹��� Ȱ��ȭ

        int secondColorNum = GetChildIndexByColor(this.colorSecondPlayer); //�� ��° �÷��̾� ���� int�� ����
        this.uiSecondPlayer.transform.GetChild(2).GetChild(secondColorNum).gameObject.SetActive(true); //�� ��° �÷��̾� ���� �̹��� Ȱ��ȭ
    }

    //���� �� �÷��̾� �����̴� �ʱ�ȭ
    private void TurnSliderInit(short type, int a) 
    {
        //a 0: firstPlayerTurn, 1: secondPlayerTurn
        if (a == 0)
        {
            this.uiFirstPlayer.isTurn = true;
            this.uiSecondPlayer.isTurn = false;
        }
        else
        {
            this.uiSecondPlayer.isTurn = true;
            this.uiFirstPlayer.isTurn = false;

        }
    }
    int GetChildIndexByColor(Color color)
    {
        if (color == Color.yellow) return 0;
        if (color == Color.red) return 1;
        if (color == Color.green) return 2;
        if (color == Color.blue) return 3;

        return 0;
    }

    private void ShowWinUI(short type, Color color)
    {
        this.uiWin.gameObject.SetActive(true);
        this.uiWin.Init(color);
    }

    private void StopSlider(short type)
    {
        this.uiFirstPlayer.isTurn = false;
        this.uiSecondPlayer.isTurn = false;
    }

    private void ShowNoticeUI(short type, int num)
    {
        this.uiNotice.gameObject.SetActive(true);
        this.uiNotice.Init(num);
    }
    private void OnDisable()
    {        
        //�˸��� �����ִ� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler<int>((int)EventEnum.eEventType.ShowNoticeUI, ShowNoticeUI);

        //���� �Ͽ� ���� �÷��̾� �����̴� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler<int>((int)EventEnum.eEventType.ChangePlayer, TurnSliderInit);

        //���� �¸� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler<Color>((int)EventEnum.eEventType.Win, ShowWinUI);

        //���� ����� �����̴� ���� �̺�Ʈ ����
        EventDispatcher.instance.RemoveEventHandler((int)EventEnum.eEventType.StopSlider, StopSlider);

    }
}
