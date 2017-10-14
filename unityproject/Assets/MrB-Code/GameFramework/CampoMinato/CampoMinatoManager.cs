using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CampoMinatoManager : MonoBehaviour
{
    static readonly int ROWS = 8;
    static readonly int COLS = 17;

    // 8x17 (with holes)
    public GameObject matrixRoot;
    Minesweeper.MinesweeperGraph graph = new Minesweeper.MinesweeperGraph();
    Minesweeper.MinesweeperNode[][] matrix = new Minesweeper.MinesweeperNode[ROWS][];
    Button[][] buttonsMatrix = new Button[ROWS][];

    public Image btn_bomb;
    public Image btn_empty;
    public Image btn_flag;
    public Image btn_one;
    public Image btn_two;
    public Image btn_three;
    public Image btn_four;

    void AddConnection(int from_I, int from_J, int to_I, int to_J)
    {
        graph.AddLink(matrix[from_I][from_J], matrix[to_I][to_J]);
        //Debug.Log(string.Format("Connection: {0} {1} - {2} {3}", from_I, from_J, to_I, to_J));
    }

    Sprite NumberOfBombsToSprite(Minesweeper.MinesweeperNode node)
    {
        if (node.nearMines == 1)
        {
            return btn_one.sprite;
        }
        if (node.nearMines == 2)
        {
            return btn_two.sprite;
        }
        if (node.nearMines == 3)
        {
            return btn_three.sprite;
        }
        if (node.nearMines == 4)
        {
            return btn_four.sprite;
        }
        return null;
    }

    void Start ()
    {
        for(var i=0; i<matrix.Length; ++i)
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

            var bomb = button.gameObject.GetComponent<BottoneCampoMinato>();
            if (bomb != null && bomb.mina)
            {
                graphNode.mined = true;
                Debug.Log(string.Format("{0} {1} is mined", row, col));
            }

            matrix[row][col] = graphNode;
            buttonsMatrix[row][col] = button;

            //Debug.Log(string.Format("{0} {1}", row, col));
            button.onClick.AddListener(() =>
            {
                OnButtonClick(button, graphNode);
            });
        }

        for (var i=0; i<matrix.Length; ++i)
        {
            for(var j=0; j<matrix[i].Length; ++j)
            {
                if (matrix[i][j] != null)
                {
                    if (i!=0 && matrix[i-1][j] != null) // prev row exists
                    {
                        AddConnection(i, j, i - 1, j); // top, center

                        if (j!=0 && matrix[i-1][j - 1] != null)
                        {
                            AddConnection(i, j, i - 1, j-1); // top, left
                        }
                        if (j < matrix[i].Length-1 && matrix[i - 1][j + 1] != null)
                        {
                            AddConnection(i, j, i - 1, j + 1); // top, right
                        }
                    }
                    if (i < matrix.Length - 1 && matrix[i+1][j] != null) // next row exists
                    {
                        AddConnection(i, j, i + 1, j); // bottom, center

                        if (j != 0 && matrix[i + 1][j - 1] != null)
                        {
                            AddConnection(i, j, i + 1, j-1); // bottom, left
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
	
    void OnButtonClick(Button uiButton, Minesweeper.MinesweeperNode graphNode)
    {
        Debug.Log(string.Format("clicked on {0}", graphNode.label));
        graph.UncoverSafeNodes(graphNode);
        uiButton.gameObject.GetComponent<Image>().sprite = btn_empty.sprite;

        if (graphNode.mined)
        {
            uiButton.gameObject.GetComponent<Image>().sprite = btn_bomb.sprite;
            // sconfitta
            return;
        }

        for (var i=0; i<ROWS; ++i)
        {
            for (var j=0; j<COLS; ++j)
            {
                if (matrix[i][j] != null)
                {
                    // è stato scoperto?
                    if (!matrix[i][j].covered)
                    {
                        Debug.Log(string.Format("{0} {1} scoperto", i, j));
                        buttonsMatrix[i][j].gameObject.GetComponent<Image>().sprite = btn_empty.sprite;
                    }
                    if (matrix[i][j].nearMines > 0)
                    {
                        Debug.Log(string.Format("{0} {1} ha {2}", i, j, matrix[i][j].nearMines));
                        buttonsMatrix[i][j].gameObject.GetComponent<Image>().sprite = NumberOfBombsToSprite(matrix[i][j]);
                    }
                }
            }
        }
    }

	
}
