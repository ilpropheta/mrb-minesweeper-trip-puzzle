using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Mined
}

public class BottoneCampoMinato : MonoBehaviour
{
    public CellState state = CellState.Mined;
}
