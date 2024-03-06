using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoard : MonoBehaviour
{
    public Transform contentTrans; // ���� �� ��ġ
    public int rows = 9;  // �� ��
    public int columns = 9;  // �� ��
    public GameObject cellPrefab;  // �� ���� �����ϱ� ���� ������
    public Button btnSelect; //���� ��ư
    public AudioSource audioButton; //��ư ȿ����

    private Button selectedCell; // ���� ������ ��
    private Button preCell; //���� ������ ��

    private Color firstPlayerColor;
    private Color secondPlayerColor;
    private bool isFirstPlayerTurn = true; //�÷��̾� �� ����
    private bool isFirstPlayerFirstTurn = true; //ù ��° �÷��̾��� ù ��° ��
    // 3�� Ȯ���� ���� �迭
    private GameObject[,,] cells; // [x, y, player]

    private void Start()
    {
        //���� ��ư �ʱ�ȭ
        this.btnSelect.onClick.AddListener(() => PlaceStone());
        //�׸��� �ʱ�ȭ
        InitializeGrid();
    }

    //ù ��°, �� ��° �÷��̾� ���� �Ҵ�
    public void Init(Color firstPlayerColor, Color secondPlayerColor)
    {
        this.firstPlayerColor = firstPlayerColor;
        this.secondPlayerColor = secondPlayerColor;
    }

    //�׸��� �ʱ�ȭ �޼���
    void InitializeGrid()
    {
        cells = new GameObject[rows, columns, 5]; // 5�� �� �ڽ� �̹����� ���� (�÷��̾� ����)

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //cellPrefab�� �����Ͽ� �� ���� ����
                var go = Instantiate(this.cellPrefab, this.contentTrans);

                //�� �ؽ�Ʈ ������Ʈ�� ���� �Ҵ�
                Text cellText = go.GetComponentInChildren<Text>();
                cellText.text = ((i % 3) * 3 + (j % 3) + 1).ToString();

                //���� �±� �Ҵ�
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

                //�� ���� Ŭ�� �̺�Ʈ �߰�
                Button cellButton = go.GetComponent<Button>();
                cellButton.onClick.AddListener(() => OnCellClick(cellButton));

                //cells �迭�� �� �߰�
                cells[i, j, 0] = go; // 0�� �⺻ ��
                cells[i, j, 1] = go.transform.GetChild(1).gameObject; // 1�� Color.yellow
                cells[i, j, 2] = go.transform.GetChild(2).gameObject; // 2�� Color.red
                cells[i, j, 3] = go.transform.GetChild(3).gameObject; // 3�� Color.green
                cells[i, j, 4] = go.transform.GetChild(4).gameObject; // 4�� Color.blue;

                // ��� ���� ��Ȱ��ȭ
                for (int k = 1; k <= 4; k++)
                {
                    cells[i, j, k].SetActive(false);
                }
            }
        }
    }

    //�� Ŭ�� �� ȣ��Ǵ� �Լ�
    void OnCellClick(Button clickedCell)
    {
        //������ ������ ���� �ƿ����� ��Ȱ��ȭ
        if (selectedCell != null)
        {
            var outline = selectedCell.GetComponent<Outline>();
            outline.enabled = false;
        }

        //���� Ŭ���� �� ���� �� �ƿ����� Ȱ��ȭ
        selectedCell = clickedCell;
        var currentOutline = selectedCell.GetComponent<Outline>();
        currentOutline.enabled = true;
    }

    //�� Ȯ�� ��ư �޼���
    void PlaceStone()
    {
        if (selectedCell == null)
        {
            Debug.Log("���� ���� �����ϼ���.");
            return;
        }

        if (IsCellEmpty(selectedCell))
        {
            if (isFirstPlayerFirstTurn && isFirstPlayerTurn)
            {
                // ó������ �ƹ����� ����
                SetCellImageActive(selectedCell, firstPlayerColor);
                int row = selectedCell.transform.GetSiblingIndex() / columns;
                int col = selectedCell.transform.GetSiblingIndex() % columns;
                cells[row, col, GetChildIndexByColor(firstPlayerColor)].SetActive(true); // cells �迭�� �� ����

                CheckForWin(selectedCell, firstPlayerColor);
                isFirstPlayerTurn = false;
                isFirstPlayerFirstTurn = false;
                this.preCell = selectedCell;

                this.audioButton.Play(); //��ư Ŭ�� ȿ����

                //�� �ٲٴ� �̺�Ʈ �ߵ�
                EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ChangePlayer, 1);

            }
            else if (!isFirstPlayerFirstTurn && isFirstPlayerTurn)
            {
                // ù ��° �÷��̾�: �� ��° �÷��̾ ���� ���� cellText �� Ȯ��
                int cellTextValue = GetCellTextValue(this.preCell);
                string targetTag = cellTextValue + "Spot";

                // ���� ���� �� �ִ��� Ȯ���ϰ� ���� �ڵ�
                bool canPlace = CanPlaceStone(targetTag, selectedCell);

                if (canPlace)
                {
                    SetCellImageActive(selectedCell, firstPlayerColor);
                    int row = selectedCell.transform.GetSiblingIndex() / columns;
                    int col = selectedCell.transform.GetSiblingIndex() % columns;
                    cells[row, col, GetChildIndexByColor(firstPlayerColor)].SetActive(true); // cells �迭�� �� ����

                    CheckForWin(selectedCell, firstPlayerColor);
                    isFirstPlayerTurn = false;
                    this.preCell = selectedCell;

                    this.audioButton.Play(); //��ư Ŭ�� ȿ����

                    //�� �ٲٴ� �̺�Ʈ �ߵ�
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ChangePlayer, 1);

                }
                else
                {                      
                    //�˸��� UI �����ִ� �̺�Ʈ �ߵ�
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ShowNoticeUI, cellTextValue);

                    Debug.Log(targetTag + "���� ���� ���� �� �ֽ��ϴ�.");
                }

            }
            else
            {
                //�� ��° �÷��̾�: ù ��° �÷��̾ ���� ���� cellText �� Ȯ��
                int cellTextValue = GetCellTextValue(this.preCell);
                string targetTag = cellTextValue + "Spot";

                //���� ���� �� �ִ��� Ȯ���ϰ� ���� �ڵ�
                bool canPlace = CanPlaceStone(targetTag, selectedCell);

                if (canPlace)
                {
                    SetCellImageActive(selectedCell, secondPlayerColor);
                    int row = selectedCell.transform.GetSiblingIndex() / columns;
                    int col = selectedCell.transform.GetSiblingIndex() % columns;
                    cells[row, col, GetChildIndexByColor(secondPlayerColor)].SetActive(true); //cells �迭�� �� ����

                    CheckForWin(selectedCell, secondPlayerColor);
                    isFirstPlayerTurn = true;
                    this.preCell = selectedCell;

                    this.audioButton.Play(); //��ư Ŭ�� ȿ����

                    //�� �ٲٴ� �̺�Ʈ �ߵ�
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ChangePlayer, 0);

                }
                else
                {
                    //�˸��� UI �����ִ� �̺�Ʈ �ߵ�
                    EventDispatcher.instance.SendEvent<int>((int)EventEnum.eEventType.ShowNoticeUI, cellTextValue);

                    Debug.Log(targetTag + "���� ���� ���� �� �ֽ��ϴ�.");
                }
            }
        }
        else
        {
            Debug.Log("�̹� ���� ������ �ֽ��ϴ�.");
        }
    }

    // ���� ����ִ��� Ȯ��
    bool IsCellEmpty(Button cell)
    {
        int childIndex = GetChildIndexByColor(firstPlayerColor);
        int childIndex1 = GetChildIndexByColor(secondPlayerColor);
        return !cell.transform.GetChild(childIndex).gameObject.activeSelf && !cell.transform.GetChild(childIndex1).gameObject.activeSelf;
    }

    //�� ��ȣ �Ľ� �޼���
    int GetCellTextValue(Button cell)
    {
        Text cellText = cell.GetComponentInChildren<Text>();
        int value;

        if (int.TryParse(cellText.text, out value))
        {
            return value;
        }
        return -1; // �Ľ� ���� �� -1 ��ȯ
    }

    //���� ���� ���� �� �±� �� �޼���
    bool CanPlaceStone(string targetTag, Button cell)
    {
        //���� �÷��̾ ���� ���� cellText ���� ��ġ�ϴ� XSpot���� ���� ���� �� ����
        if (cell.CompareTag(targetTag))
        {
            //Debug.Log("�±� ��ġ cell tag: " + cell.tag + "\ttargetTage: " + targetTag);
            return true;
        }

        return false;
    }

    //�� �ڽ����� �ִ� �̹��� Ȱ��ȭ �޼���
    void SetCellImageActive(Button cell, Color playerColor)
    {
        int childIndex = GetChildIndexByColor(playerColor); //�÷��̾� ������ �ڽ� ��ȣ�� ��ȯ
        cell.transform.GetChild(childIndex).gameObject.SetActive(true);
    }

    //�÷��̾� ������ �ڽ� ��ȣ�� ��ȯ �޼���
    int GetChildIndexByColor(Color color)
    {
        if (color == Color.yellow) return 1;
        if (color == Color.red) return 2;
        if (color == Color.green) return 3;
        if (color == Color.blue) return 4;

        return 0;
    }

    // 3�� Ȯ�� �Լ�
    void CheckForWin(Button cell, Color playerColor)
    {
        int row = cell.transform.GetSiblingIndex() / columns;
        int col = cell.transform.GetSiblingIndex() % columns;

        //Debug.LogFormat("row: {0}, col: {1}", row, col);

        if (CheckHorizontalWin(row, col, playerColor) ||
            CheckVerticalWin(row, col, playerColor) ||
            CheckDiagonalWin(row, col, playerColor))
        {
            Debug.Log("3�� �ϼ� �¸�!");
            EventDispatcher.instance.SendEvent<Color>((int)EventEnum.eEventType.Win, playerColor); //�¸� UI�˾� �����ֱ�
        }
    }

    // �������� 3�� Ȯ��
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
                count = 0; // ���ӵ� ���� �ƴ϶�� ī��Ʈ �ʱ�ȭ
            }
        }

        return false;
    }

    //�������� 3�� Ȯ��
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
                count = 0; // ���ӵ� ���� �ƴ϶�� ī��Ʈ �ʱ�ȭ
            }
        }

        return false;
    }

    //�밢������ 3�� Ȯ��
    bool CheckDiagonalWin(int row, int col, Color playerColor)
    {
        int count1 = 0;
        int count2 = 0;

        // �밢�� ���� 1
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

        // �밢�� ���� 2
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
