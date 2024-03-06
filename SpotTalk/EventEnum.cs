using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnum
{
    public enum eEventType
    {
        NONE = -1,
        FirstColorPick,
        SecondColorPick,
        ChangePlayer,
        Win,
        StopSlider,
        TimeOver,
        ShowNoticeUI,
        StartGame,
        StartNewGame
    }
}
