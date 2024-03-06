using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorPicker : MonoBehaviour
{
    public Text txtFirstPerson;
    public Text txtSecondPerson;

    public Button btnYellow;
    public Button btnRed;
    public Button btnGreen;
    public Button btnBlue;

    public AudioSource audioButton; //버튼 효과음

    private bool isFirstPlayerTurn = true; // 플레이어 턴을 추적
    private Color selectedColorFirstPlayer; // 첫 번째 플레이어가 선택한 색상을 저장

    void Start()
    {
        this.btnYellow.onClick.AddListener(() =>
        {
            OnColorButtonClick(Color.yellow);
            btnYellow.gameObject.GetComponent<Outline>().enabled = true;
            this.audioButton.Play();
        });
        this.btnRed.onClick.AddListener(() =>
        {
            OnColorButtonClick(Color.red);
            btnRed.gameObject.GetComponent<Outline>().enabled = true;
            this.audioButton.Play();
        });
        this.btnGreen.onClick.AddListener(() =>
        {
            OnColorButtonClick(Color.green);
            btnGreen.gameObject.GetComponent<Outline>().enabled = true;
            this.audioButton.Play();
        });
        this.btnBlue.onClick.AddListener(() =>
        {
            OnColorButtonClick(Color.blue);
            btnBlue.gameObject.GetComponent<Outline>().enabled = true;
            this.audioButton.Play();
        });
    }

    public void OnColorButtonClick(Color color)
    {
        if (isFirstPlayerTurn)
        {
            selectedColorFirstPlayer = color;
            isFirstPlayerTurn = false;

            // 첫 번째 플레이어가 선택한 색상 버튼을 비활성화
            DisableColorButton(color);

            this.txtFirstPerson.gameObject.SetActive(false);
            this.txtSecondPerson.gameObject.SetActive(true);

            //첫 번째 플레이어가 선택한 색상 이벤트 전달
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.FirstColorPick, color);
        }
        else
        {
            // 두 번째 플레이어가 선택한 색상 버튼을 비활성화
            DisableColorButton(color);

            //두 번째 플레이어가 선택한 색상 이벤트 전달
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.SecondColorPick, color);
            this.gameObject.SetActive(false);
        }
    }

    private void DisableColorButton(Color color)
    {
        // 선택한 색상 버튼을 비활성화
        if (color == Color.yellow)
        {
            this.btnYellow.interactable = false;
        }
        else if (color == Color.red)
        {
            this.btnRed.interactable = false;
        }
        else if (color == Color.green)
        {
            this.btnGreen.interactable = false;
        }
        else if (color == Color.blue)
        {
            this.btnBlue.interactable = false;
        }
    }
}
