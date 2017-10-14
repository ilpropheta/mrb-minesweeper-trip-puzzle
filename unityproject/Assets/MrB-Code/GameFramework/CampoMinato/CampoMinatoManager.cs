using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CampoMinatoManager : MonoBehaviour
{
    // 8x17 (with holes)
    public GameObject matrixRoot;
    Minesweeper.MinesweeperGraph graph = new Minesweeper.MinesweeperGraph();
    Minesweeper.MinesweeperNode[][] matrix = new Minesweeper.MinesweeperNode[8][];

    void AddConnection(int from_I, int from_J, int to_I, int to_J)
    {
        graph.AddLink(matrix[from_I][from_J], matrix[to_I][to_J]);
        //Debug.Log(string.Format("Connection: {0} {1} - {2} {3}", from_I, from_J, to_I, to_J));
    }

    void Start ()
    {
        for(var i=0; i<matrix.Length; ++i)
        {
            matrix[i] = new Minesweeper.MinesweeperNode[17];
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
            //Debug.Log(string.Format("{0} {1}", row, col));
            button.onClick.AddListener(() =>
            {
                OnButtonClick(graphNode);
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
	}
	
    void OnButtonClick(Minesweeper.MinesweeperNode graphNode)
    {
        Debug.Log(string.Format("clicked on {0}", graphNode.label));
        graph.UncoverSafeNodes(graphNode);
    }

	
}
