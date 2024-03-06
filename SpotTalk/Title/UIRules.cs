using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRules : MonoBehaviour
{
    public Button btnClose;
    public AudioSource audioClose;
    void Start()
    {
        this.btnClose.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            this.audioClose.Play();
        });
    }

}
