using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWin : MonoBehaviour
{
    public Button btnHome; //새 게임 버튼
    public Button btnExit; //종료 버튼
    public AudioSource audioButton; //버튼 효과음

    public TMP_Text txtWinColor; //이긴 플레이어 색상

    void Start()
    {   
        //새 게임 버튼
        this.btnHome.onClick.AddListener(() =>
        {
            this.audioButton.Play(); //버튼 클릭 효과음
            EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.StartNewGame);
        });

        //종료 버튼
        this.btnExit.onClick.AddListener(() =>
        {
            this.audioButton.Play(); //버튼 클릭 효과음
            Application.Quit();
        });
    }
    public void Init(Color color)
    {
        this.txtWinColor.text = GetColorToString(color); //승리한 플레이어 색상 텍스트
        Invoke("StopSlider", 1f);
    }

    //승리한 플레이어 색상 string으로 변환
    string GetColorToString(Color color)
    {
        if (color == Color.yellow) return "Yellow";
        if (color == Color.red) return "Red";
        if (color == Color.green) return "Green";
        if (color == Color.blue) return "Blue";

        return "Color";
    }

    private void StopSlider()
    {
        EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.StopSlider); //슬라이더 멈춤
    }

}
