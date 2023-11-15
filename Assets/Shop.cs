using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update

    //Indsæt buildmanager

    void Start()
    {
     //buildmanager = Buildmanager.instance;   
    }
    public void PurchaseRangedTurret()
    {
        Debug.Log("Ranged Turret Purchaed");
        //buildmanager.SetTurretToBuild(buildmanager.RangedTurret)
    }
    public void PurchaseBarrierTurret()
    {
        Debug.Log("Barrier Turret Purchaed");
        //buildmanager.SetTurretToBuild(buildmanager.BarrierTurret)
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
