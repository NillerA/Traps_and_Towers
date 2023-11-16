using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public MousePosition3D mousePos;
    // Start is called before the first frame update
    private MousePosition3D mousePosition;
    public GameObject ShopUI;
    public GameManager gameManager;
    public void PurchaseTower(GridTileItem item)
    {
        mousePos.item = item;
        mousePos.StartDrag();
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
        mousePos.StartDrag();

        Debug.Log("Ranged Turret Purchaed");
    }
    public void PurchaseBarrierTurret()
    {
        Debug.Log("Barrier Turret Purchaed");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
