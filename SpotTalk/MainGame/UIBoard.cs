using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoard : MonoBehaviour
{
    public Transform contentTrans; // 복사 될 위치
    public int rows = 9;  // 행 수
    public int columns = 9;  // 열 수
    public GameObject cellPrefab;  // 각 셀을 생성하기 위한 프리팹
    public Button btnSelect; //선택 버튼
    public AudioSource audioButton; //버튼 효과음

    private Button selectedCell; // 현재 선택한 셀
    private Button preCell; //이전 선택한 셀

    private Color firstPlayerColor;
    private Color secondPlayerColor;
    private bool isFirstPlayerTurn = true; //플레이어 턴 추적
    private bool isFirstPlayerFirstTurn = true; //첫 번째 플레이어의 첫 번째 턴
    // 3목 확인을 위한 배열
    private GameObject[,,] cells; // [x, y, player]

    private void Start()
    {
        //선택 버튼 초기화
        this.btnSelect.onClick.AddListener(() => PlaceStone());
        //그리드 초기화
        InitializeGrid();
    }

    //첫 번째, 두 번째 플레이어 색상 할당
    public void Init(Color firstPlayerColor, Color secondPlayerColor)
    {
        this.firstPlayerColor = firstPlayerColor;
        this.secondPlayerColor = secondPlayerColor;
    }

    //그리드 초기화 메서드
    void InitializeGrid()
    {
        cells = new GameObject[rows, columns, 5]; // 5는 셀 자식 이미지의 개수 (플레이어 색상별)

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //cellPrefab을 복제하여 각 셀을 생성
                var go = Instantiate(this.cellPrefab, this.contentTrans);

                //셀 텍스트 컴포넌트에 값을 할당
                Text cellText = go.GetComponentInChildren<Text>();
                cellText.text = ((i % 3) * 3 + (j % 3) + 1).ToString();

                //셀에 태그 할당
                if (i < 3)
                {
                    if (j < 3)
                        go.tag = "1Spot";
                    else if (j < 6)
                        go.tag = "2Spot";
                    else
                        go.tag = "3Spot";
                }
                else if (i < 6)
                {
                    if (j < 3)
                        go.tag = "4Spot";
                    else if (j < 6)
                        go.tag = "5Spot";
                    else
                        go.tag = "6Spot";
                }
                else
                {
                    if (j < 3)
                        go.tag = "7Spot";
                    else if (j < 6)
                        go.tag = "8Spot";
                    else
                        go.tag = "9Spot";
                }

                //각 셀에 클릭 이벤트 추가
                Button cellButton = go.GetComponent<Button>();
                cellButton.onClick.AddListener(() => OnCellClick(cellButton));

                //cells 배열에 셀 추가
                cells[i, j, 0] = go; // 0은 기본 셀
                cells[i, j, 1] = go.transform.GetChild(1).gameObject; // 1은 Color.yellow
                cells[i, j, 2] = go.transform.GetChild(2).gameObject; // 2은 Color.red
                cells[i, j, 3] = go.transform.GetChild(3).gameObject; // 3은 Color.green
                cells[i, j, 4] = go.transform.GetChild(4).gameObject; // 4은 Color.blue;

                // 모든 돌을 비활성화
                for (int k = 1; k <= 4; k++)
                {
                    cells[i, j, k].SetActive(false);
                }
            }
        }
    }

    //셀 클릭 시 호출되는 함수
    void OnCellClick(Button clickedCell)
    {
        //이전에 선택한 셀의 아웃라인 비활성화
        if (selectedCell != null)
        {
            var outline = selectedCell.GetComponent<Outline>();
            outline.enabled = false;
        }

        //현재 클릭한 셀 선택 및 아웃라인 활성화
        selectedCell = clickedCell;
        var currentOutline = selectedCell.GetComponent<Outline>();
        currentOutline.enabled = true;
    }

    //셀 확정 버튼 메서드
    void PlaceStone()
    {
        if (selectedCell == null)
        {
            Debug.Log("먼저 셀을 선택하세요.");
            return;
        }

        if (IsCellEmpty(selectedCell))
        {
            if (isFirstPlayerFirstTurn && isFirstPlayerTurn)
            {
                // 처음에는 아무데나 놓기
                SetCellImageActive(selectedCell, firstPlayerColor);
                int row = selectedCell.transform.GetSiblingIndex() / columns;
                int col = selectedCell.transform.GetSiblingIndex() % columns;
                cells[row, col, GetChildIndexByColor(firstPlayerColor)].SetActive(true); // cells 배열에 돌 놓기

                CheckForWin(selectedCell, firstPlayerColor);
                isFirstPlayerTurn = false;
                isFirstPlayerFirstTurn = false;
                this.preCell = selectedCell;

                this.audioButton.Play(); //버튼 클릭 효과음

                //턴 바꾸는 이벤트 발동
                EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ChangePlayer, 1);

            }
            else if (!isFirstPlayerFirstTurn && isFirstPlayerTurn)
            {
                // 첫 번째 플레이어: 두 번째 플레이어가 놓은 돌의 cellText 값 확인
                int cellTextValue = GetCellTextValue(this.preCell);
                string targetTag = cellTextValue + "Spot";

                // 돌을 놓을 수 있는지 확인하고 놓는 코드
                bool canPlace = CanPlaceStone(targetTag, selectedCell);

                if (canPlace)
                {
                    SetCellImageActive(selectedCell, firstPlayerColor);
                    int row = selectedCell.transform.GetSiblingIndex() / columns;
                    int col = selectedCell.transform.GetSiblingIndex() % columns;
                    cells[row, col, GetChildIndexByColor(firstPlayerColor)].SetActive(true); // cells 배열에 돌 놓기

                    CheckForWin(selectedCell, firstPlayerColor);
                    isFirstPlayerTurn = false;
                    this.preCell = selectedCell;

                    this.audioButton.Play(); //버튼 클릭 효과음

                    //턴 바꾸는 이벤트 발동
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ChangePlayer, 1);

                }
                else
                {                      
                    //알림판 UI 보여주는 이벤트 발동
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ShowNoticeUI, cellTextValue);

                    Debug.Log(targetTag + "에만 돌을 놓을 수 있습니다.");
                }

            }
            else
            {
                //두 번째 플레이어: 첫 번째 플레이어가 놓은 돌의 cellText 값 확인
                int cellTextValue = GetCellTextValue(this.preCell);
                string targetTag = cellTextValue + "Spot";

                //돌을 놓을 수 있는지 확인하고 놓는 코드
                bool canPlace = CanPlaceStone(targetTag, selectedCell);

                if (canPlace)
                {
                    SetCellImageActive(selectedCell, secondPlayerColor);
                    int row = selectedCell.transform.GetSiblingIndex() / columns;
                    int col = selectedCell.transform.GetSiblingIndex() % columns;
                    cells[row, col, GetChildIndexByColor(secondPlayerColor)].SetActive(true); //cells 배열에 돌 놓기

                    CheckForWin(selectedCell, secondPlayerColor);
                    isFirstPlayerTurn = true;
                    this.preCell = selectedCell;

                    this.audioButton.Play(); //버튼 클릭 효과음

                    //턴 바꾸는 이벤트 발동
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ChangePlayer, 0);

                }
                else
                {
                    //알림판 UI 보여주는 이벤트 발동
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ShowNoticeUI, cellTextValue);

                    Debug.Log(targetTag + "에만 돌을 놓을 수 있습니다.");
                }
            }
        }
        else
        {
            Debug.Log("이미 돌이 놓여져 있습니다.");
        }
    }

    // 셀이 비어있는지 확인
    bool IsCellEmpty(Button cell)
    {
        int childIndex = GetChildIndexByColor(firstPlayerColor);
        int childIndex1 = GetChildIndexByColor(secondPlayerColor);
        return !cell.transform.GetChild(childIndex).gameObject.activeSelf && !cell.transform.GetChild(childIndex1).gameObject.activeSelf;
    }

    //셀 번호 파싱 메서드
    int GetCellTextValue(Button cell)
    {
        Text cellText = cell.GetComponentInChildren<Text>();
        int value;

        if (int.TryParse(cellText.text, out value))
        {
            return value;
        }
        return -1; // 파싱 오류 시 -1 반환
    }

    //이전 셀과 현재 셀 태그 비교 메서드
    bool CanPlaceStone(string targetTag, Button cell)
    {
        //이전 플레이어가 놓은 돌의 cellText 값과 일치하는 XSpot에만 돌을 놓을 수 있음
        if (cell.CompareTag(targetTag))
        {
            //Debug.Log("태그 일치 cell tag: " + cell.tag + "\ttargetTage: " + targetTag);
            return true;
        }

        return false;
    }

    //셀 자식으로 있는 이미지 활성화 메서드
    void SetCellImageActive(Button cell, Color playerColor)
    {
        int childIndex = GetChildIndexByColor(playerColor); //플레이어 색상을 자식 번호로 반환
        cell.transform.GetChild(childIndex).gameObject.SetActive(true);
    }

    //플레이어 색상을 자식 번호로 반환 메서드
    int GetChildIndexByColor(Color color)
    {
        if (color == Color.yellow) return 1;
        if (color == Color.red) return 2;
        if (color == Color.green) return 3;
        if (color == Color.blue) return 4;

        return 0;
    }

    // 3목 확인 함수
    void CheckForWin(Button cell, Color playerColor)
    {
        int row = cell.transform.GetSiblingIndex() / columns;
        int col = cell.transform.GetSiblingIndex() % columns;

        //Debug.LogFormat("row: {0}, col: {1}", row, col);

        if (CheckHorizontalWin(row, col, playerColor) ||
            CheckVerticalWin(row, col, playerColor) ||
            CheckDiagonalWin(row, col, playerColor))
        {
            Debug.Log("3목 완성 승리!");
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.Win, playerColor); //승리 UI팝업 보여주기
        }
    }

    // 수평으로 3목 확인
    bool CheckHorizontalWin(int row, int col, Color playerColor)
    {
        int count = 0;
        for (int i = col - 2; i <= col + 2; i++)
        {
            if (i < 0 || i >= columns) continue;

            if (cells[row, i, GetChildIndexByColor(playerColor)].activeSelf)
            {
                count++;

                if (count >= 3)
                {
                    return true;
                }
            }
            else
            {
                count = 0; // 연속된 돌이 아니라면 카운트 초기화
            }
        }

        return false;
    }

    //수직으로 3목 확인
    bool CheckVerticalWin(int row, int col, Color playerColor)
    {
        int count = 0;
        for (int i = row - 2; i <= row + 2; i++)
        {
            if (i < 0 || i >= rows) continue;

            if (cells[i, col, GetChildIndexByColor(playerColor)].activeSelf)
            {
                count++;
                if (count >= 3)
                {
                    return true;
                }
            }
            else
            {
                count = 0; // 연속된 돌이 아니라면 카운트 초기화
            }
        }

        return false;
    }

    //대각선으로 3목 확인
    bool CheckDiagonalWin(int row, int col, Color playerColor)
    {
        int count1 = 0;
        int count2 = 0;

        // 대각선 방향 1
        for (int i = -2; i <= 2; i++)
        {
            int r = row + i;
            int c = col + i;
            if (r >= 0 && r < rows && c >= 0 && c < columns)
            {
                if (cells[r, c, GetChildIndexByColor(playerColor)].activeSelf)
                {
                    count1++;
                    if (count1 >= 3)
                    {
                        return true;
                    }
                }
                else
                {
                    count1 = 0;
                }
            }
        }

        // 대각선 방향 2
        for (int i = -2; i <= 2; i++)
        {
            int r = row + i;
            int c = col - i;
            if (r >= 0 && r < rows && c >= 0 && c < columns)
            {
                if (cells[r, c, GetChildIndexByColor(playerColor)].activeSelf)
                {
                    count2++;
                    if (count2 >= 3)
                    {
                        return true;
                    }
                }
                else
                {
                    count2 = 0;
                }
            }
        }

        return false;
    }

}
