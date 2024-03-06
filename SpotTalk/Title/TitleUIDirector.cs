using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIDirector : MonoBehaviour
{
    public Button btnStart;
    public Button btnExit;
    public Button btnRules;
    public AudioSource audioButton;

    public UIRules uiRules;
    void Start()
    {
        this.btnStart.onClick.AddListener(() =>
        {
            EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.StartGame);
            this.audioButton.Play();
        });

        this.btnExit.onClick.AddListener(() =>
        {
            Application.Quit();
            this.audioButton.Play();
        });

        this.btnRules.onClick.AddListener(() =>
        {
            this.uiRules.gameObject.SetActive(true);
            this.audioButton.Play();
        });
    }

}
