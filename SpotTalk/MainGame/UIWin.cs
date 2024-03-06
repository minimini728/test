using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWin : MonoBehaviour
{
    public Button btnHome; //�� ���� ��ư
    public Button btnExit; //���� ��ư
    public AudioSource audioButton; //��ư ȿ����

    public TMP_Text txtWinColor; //�̱� �÷��̾� ����

    void Start()
    {   
        //�� ���� ��ư
        this.btnHome.onClick.AddListener(() =>
        {
            this.audioButton.Play(); //��ư Ŭ�� ȿ����
            EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.StartNewGame);
        });

        //���� ��ư
        this.btnExit.onClick.AddListener(() =>
        {
            this.audioButton.Play(); //��ư Ŭ�� ȿ����
            Application.Quit();
        });
    }
    public void Init(Color color)
    {
        this.txtWinColor.text = GetColorToString(color); //�¸��� �÷��̾� ���� �ؽ�Ʈ
        Invoke("StopSlider", 1f);
    }

    //�¸��� �÷��̾� ���� string���� ��ȯ
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
        EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.StopSlider); //�����̴� ����
    }

}
