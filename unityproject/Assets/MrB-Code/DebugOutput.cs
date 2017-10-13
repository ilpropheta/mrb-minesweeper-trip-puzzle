using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add to any game object
/// Requires a Text control named DebugOutputText
/// </summary>
public class DebugOutput : MonoBehaviour
{

    private static readonly string[] m_items = new string[20];

    public static void Row(int row, string text)
    {
        m_items[row] = text;
    }

    public static void Append(string text)
    {
        for (int i = 1; i <20; i++)
        {
            m_items[i - 1] = m_items[i];
        }
        m_items[19] = text;
    }

    private Text _textControl;

	void Awake () {
        _textControl = FindObjectsOfType<Text>().SingleOrDefault(x => x.name == "DebugOutputText");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var sb = new StringBuilder();
	    foreach (var item in m_items)
	    {
	        sb.AppendLine(item);
	    }
	    _textControl.text = sb.ToString();
	}
}
