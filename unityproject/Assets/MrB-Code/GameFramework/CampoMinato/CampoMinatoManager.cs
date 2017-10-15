using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CampoMinatoManager : MonoBehaviour
{
    static readonly int ROWS = 8;
    static readonly int COLS = 17;

    // 8x17 (with holes)
    public GameObject matrixRoot;
    public Image img_covered;
    public Image img_bomb;
    public Image img_empty;
    public Image img_flag;
    public Image img_one;
    public Image img_two;
    public Image img_three;
    public Image img_four;
    public Button btn_submit;

    Minesweeper.MinesweeperGraph graph = new Minesweeper.MinesweeperGraph();
    Minesweeper.MinesweeperNode[][] matrix = new Minesweeper.MinesweeperNode[ROWS][];
    Button[][] buttonsMatrix = new Button[ROWS][];
    bool isLosing = false; // ekt, forgive me and do this better

    void AddConnection(int from_I, int from_J, int to_I, int to_J)
    {
        graph.AddLink(matrix[from_I][from_J], matrix[to_I][to_J]);
        //Debug.Log(string.Format("Connection: {0} {1} - {2} {3}", from_I, from_J, to_I, to_J));
    }

    void Start()
    {
        for (var i = 0; i < matrix.Length; ++i)
        {
            matrix[i] = new Minesweeper.MinesweeperNode[COLS];
            buttonsMatrix[i] = new Button[COLS];
        }

        string pattern = @"^Button_(\d+)_(\d+)$";
        var buttons = matrixRoot.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            var name = button.gameObject.name;

            var res = Regex.Split(name, pattern);
            int row = int.Parse(res[1]);
            int col = int.Parse(res[2]);
            var graphNode = new Minesweeper.MinesweeperNode(string.Format("{0}_{1}", row, col));

            var buttonExtraState = button.gameObject.GetComponent<BottoneCampoMinato>();
            if (buttonExtraState != null)
            {
                switch (buttonExtraState.state)
                {
                    case CellState.Mined:
                        graphNode.mined = true;
                        break;
                    default:
                        break;
                }
            }

            matrix[row][col] = graphNode;
            buttonsMatrix[row][col] = button;

            button.onClick.AddListener(() =>
            {
                OnButtonClick(button, graphNode);
            });
        }

        for (var i = 0; i < matrix.Length; ++i)
        {
            for (var j = 0; j < matrix[i].Length; ++j)
            {
                if (matrix[i][j] != null)
                {
                    if (i != 0 && matrix[i - 1][j] != null) // prev row exists
                    {
                        AddConnection(i, j, i - 1, j); // top, center

                        if (j != 0 && matrix[i - 1][j - 1] != null)
                        {
                            AddConnection(i, j, i - 1, j - 1); // top, left
                        }
                        if (j < matrix[i].Length - 1 && matrix[i - 1][j + 1] != null)
                        {
                            AddConnection(i, j, i - 1, j + 1); // top, right
                        }
                    }
                    if (i < matrix.Length - 1 && matrix[i + 1][j] != null) // next row exists
                    {
                        AddConnection(i, j, i + 1, j); // bottom, center

                        if (j != 0 && matrix[i + 1][j - 1] != null)
                        {
                            AddConnection(i, j, i + 1, j - 1); // bottom, left
                        }
                        if (j < matrix[i].Length - 1 && matrix[i + 1][j + 1] != null)
                        {
                            AddConnection(i, j, i + 1, j + 1); // bottom, right
                        }
                    }
                    if (j < matrix[i].Length - 1 && matrix[i][j + 1] != null)
                    {
                        AddConnection(i, j, i, j + 1); // right, center
                    }
                    if (j != 0 && matrix[i][j - 1] != null)
                    {
                        AddConnection(i, j, i, j - 1); // left, center
                    }
                }
            }
        }

        graph.ComputeNearMines();
    }

    void CheckSolution()
    {
        bool won = true;
        ChangeElements((node, button) =>
        {
            if (node.covered && !node.mined)
                won = false;
        });

        if (won)
        {
            // handle logic
            Debug.Log("hai vinto");
        }
        else
        {
            Debug.Log("non hai vinto");
        }
    }

    void SetSpriteTo(Button button, Image image)
    {
        button.GetComponent<Image>().sprite = image.sprite;
    }

    void UpdateSpriteAccordingToNodeProperties(int i, int j)
    {
        var node = matrix[i][j];
        var uiButton = buttonsMatrix[i][j];
        if (node == null || uiButton == null)
        {
            Debug.Log(string.Format("Strange...requested to change sprite of {0} {1}. It's null", i, j));
            return;
        }

        if (node.flag)
        {
            SetSpriteTo(uiButton, img_flag);
        }
        else if (node.covered)
        {
            SetSpriteTo(uiButton, img_covered);
        }
        else
        {
            if (node.mined)
            {
                SetSpriteTo(uiButton, img_bomb);
            }
            else if (node.nearMines == 1)
            {
                SetSpriteTo(uiButton, img_one);
            }
            else if (node.nearMines == 2)
            {
                SetSpriteTo(uiButton, img_two);
            }
            else if (node.nearMines == 3)
            {
                SetSpriteTo(uiButton, img_three);
            }
            else if (node.nearMines == 4)
            {
                SetSpriteTo(uiButton, img_four);
            }
            else
            {
                SetSpriteTo(uiButton, img_empty);
            }
        }
    }

    void ChangeElements(System.Action<Minesweeper.MinesweeperNode, Button> action)
    {
        for (var i = 0; i < ROWS; ++i)
        {
            for (var j = 0; j < COLS; ++j)
            {
                if (matrix[i][j] != null)
                {
                    action(matrix[i][j], buttonsMatrix[i][j]);
                    UpdateSpriteAccordingToNodeProperties(i, j);
                }
            }
        }
    }

    void ResetMatch()
    {
        isLosing = false;
        ChangeElements((node, button) =>
        {
            node.covered = true;
        });
    }

    void ShowAllBombs()
    {
        ChangeElements((node, button) =>
        {
            if (node.mined)
                node.covered = false;
        });
    }

    void UpdateMatrixRendering()
    {
        ChangeElements((node, button) => { });
    }

    IEnumerator FadeResetMatrix()
    {
        yield return new WaitForSeconds(3);
        ResetMatch();
    }

    void OnButtonClick(Button uiButton, Minesweeper.MinesweeperNode graphNode)
    {
        if (!isLosing) // again, please ekt do this better
        {
            graph.UncoverSafeNodes(graphNode);
            UpdateMatrixRendering();

            // sconfitta
            if (graphNode.mined)
            {
                isLosing = true;
                ShowAllBombs();
                StartCoroutine(FadeResetMatrix());
                return;
            }

            CheckSolution();
        }
    }


}
