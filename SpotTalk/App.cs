using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    public enum eSceneType
    {
        App, Title, TwoPlay
    }

    private eSceneType state;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        this.state = eSceneType.Title;
        this.ChangeScene(this.state);

        //게임 시작 이벤트 등록
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.StartGame, StartGame);
        //새게임 시작 이벤트 등록
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.StartNewGame, StartNewGame);
    }

    public void ChangeScene(eSceneType sceneType)
    {
        switch (sceneType)
        {
            case eSceneType.Title:
                var titleOper = SceneManager.LoadSceneAsync("Title");
                break;

            case eSceneType.TwoPlay:
                var twoPlayOper = SceneManager.LoadSceneAsync("TwoPlay");
                break;
        }
    }

    private void StartGame(short type)
    {
        this.state = eSceneType.TwoPlay;
        this.ChangeScene(this.state);
    }

    private void StartNewGame(short type)
    {
        this.state = eSceneType.Title;
        this.ChangeScene(this.state);
    }
}
