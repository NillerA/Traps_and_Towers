using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public GameObject ShopUI;
    public GameManager gameManager;

    public void PurchaseTower(GridTileItem item)
    {
        MousePosition3D.Instance.item = item;
        MousePosition3D.Instance.StartDrag();
    }

    public void CanPlaceTurret()
    {
        ShopUI.SetActive(true);
    }

    public void OnPlaceSucces()
    {
        ShopUI.SetActive(false);
        gameManager.TowerPlaced();
    }

    public void PurchaseRangedTurret()
    {
        MousePosition3D.Instance.StartDrag();

        Debug.Log("Ranged Turret Purchaed");
    }

    public void PurchaseBarrierTurret()
    {
        Debug.Log("Barrier Turret Purchaed");
    }
}
