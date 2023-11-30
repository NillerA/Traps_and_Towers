using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefaultTileDisplay : MonoBehaviour
{

    [SerializeField]
    GridTileItem item;

    private void OnMouseEnter()
    {
        BuildManager.Instance.ShowTileStatsDisplay(item);
    }

    private void OnMouseExit()
    {
        BuildManager.Instance.HideTileStatsDisplay();
    }
}
