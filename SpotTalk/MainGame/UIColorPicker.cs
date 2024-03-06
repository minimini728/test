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

    public AudioSource audioButton; //��ư ȿ����

    private bool isFirstPlayerTurn = true; // �÷��̾� ���� ����
    private Color selectedColorFirstPlayer; // ù ��° �÷��̾ ������ ������ ����

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

            // ù ��° �÷��̾ ������ ���� ��ư�� ��Ȱ��ȭ
            DisableColorButton(color);

            this.txtFirstPerson.gameObject.SetActive(false);
            this.txtSecondPerson.gameObject.SetActive(true);

            //ù ��° �÷��̾ ������ ���� �̺�Ʈ ����
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.FirstColorPick, color);
        }
        else
        {
            // �� ��° �÷��̾ ������ ���� ��ư�� ��Ȱ��ȭ
            DisableColorButton(color);

            //�� ��° �÷��̾ ������ ���� �̺�Ʈ ����
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.SecondColorPick, color);
            this.gameObject.SetActive(false);
        }
    }

    private void DisableColorButton(Color color)
    {
        // ������ ���� ��ư�� ��Ȱ��ȭ
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
