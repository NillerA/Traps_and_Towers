using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField]
    private GameObject ShopUI;

    public void PurchaseTower(TowerItem item)
    {
        BuildManager.Instance.item = item;
        BuildManager.Instance.StartDrag();
    }

    public void CanPlaceTurret()
    {
        ShopUI.SetActive(true);
    }

    public void OnPlaceSucces()
    {
        ShopUI.SetActive(false);
        GameManager.Instance.TowerPlaced();
    }
}
