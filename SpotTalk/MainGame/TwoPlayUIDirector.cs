using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayUIDirector : MonoBehaviour
{
    public UIColorPicker uiColorPicker; //색상 선택 팔레트 UI
    public UIBoard uiBoard; //보드판 UI
    public UINotice uiNotice; //알림판 UI
    public UIWin uiWin; //승리 UI

    public UIFirstPlayer uiFirstPlayer; //첫 번째 플레이어 프로필 UI
    public UISecondPlayer uiSecondPlayer; //두 번째 플레이어 프로필 UI

    public Color colorFirstPlayer; //첫 번째 플레이어 색상
    public Color colorSecondPlayer; //두 번째 플레이어 색상
    void Start()
    {
        //알림판 보여주는 이벤트 등록
        EventDispatcher.instance.AddEventHandler<int>((int)EventEnum.eEventType.ShowNoticeUI, ShowNoticeUI);

        //게임 턴에 따른 플레이어 슬라이더 이벤트 등록
        EventDispatcher.instance.AddEventHandler<int>((int)EventEnum.eEventType.ChangePlayer, TurnSliderInit);

        //게임 승리 이벤트 등록
        EventDispatcher.instance.AddEventHandler<Color>((int)EventEnum.eEventType.Win, ShowWinUI);

        //게임 종료시 슬라이더 멈춤 이벤트 등록
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.StopSlider, StopSlider);
    }

    //색상, 색상 텍스트 초기화 메서드
    public void TextInit(Color fistPlayerColor, Color secondPlayerColor)
    {
        this.uiFirstPlayer.gameObject.SetActive(true);
        this.colorFirstPlayer = fistPlayerColor;

        //슬라이더
        this.uiFirstPlayer.isTurn = true;

        this.uiSecondPlayer.gameObject.SetActive(true);
        this.colorSecondPlayer = secondPlayerColor;

        //그리드매니저 초기화
        this.uiBoard.Init(this.colorFirstPlayer, this.colorSecondPlayer);

        //프로필 초기화
        this.PlayerProfileInit();
    }

    private void PlayerProfileInit()
    {
        int firstColorNum = GetChildIndexByColor(this.colorFirstPlayer); //첫 번째 플레이어 색상 int로 변경
        this.uiFirstPlayer.transform.GetChild(2).GetChild(firstColorNum).gameObject.SetActive(true); //첫 번째 플레이어 색상 이미지 활성화

        int secondColorNum = GetChildIndexByColor(this.colorSecondPlayer); //두 번째 플레이어 색상 int로 변경
        this.uiSecondPlayer.transform.GetChild(2).GetChild(secondColorNum).gameObject.SetActive(true); //두 번째 플레이어 색상 이미지 활성화
    }

    //게임 턴 플레이어 슬라이더 초기화
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
        //알림판 보여주는 이벤트 삭제
        EventDispatcher.instance.RemoveEventHandler<int>((int)EventEnum.eEventType.ShowNoticeUI, ShowNoticeUI);

        //게임 턴에 따른 플레이어 슬라이더 이벤트 삭제
        EventDispatcher.instance.RemoveEventHandler<int>((int)EventEnum.eEventType.ChangePlayer, TurnSliderInit);

        //게임 승리 이벤트 삭제
        EventDispatcher.instance.RemoveEventHandler<Color>((int)EventEnum.eEventType.Win, ShowWinUI);

        //게임 종료시 슬라이더 멈춤 이벤트 삭제
        EventDispatcher.instance.RemoveEventHandler((int)EventEnum.eEventType.StopSlider, StopSlider);

    }
}
