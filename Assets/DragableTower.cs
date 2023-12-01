using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class DragableTower : MonoBehaviour
{

    [SerializeField]
    TowerItem tower;
    [SerializeField]
    UnityEvent DragStart;
    GameObject towerItemShowcase;
    Vector3 defaultPos;
    Quaternion defaultRot;

    private void Start()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    public void SetItem(TowerItem item)
    {
        tower = item;
        if(towerItemShowcase != null)
        Destroy(towerItemShowcase);
        towerItemShowcase = Instantiate(tower.PreviewPrefab, transform.position, transform.rotation, transform);
    }

    public void ResetTower()
    {
        transform.position = defaultPos;
        transform.rotation = defaultRot;
    }

    private void OnMouseOver()
    {
        BuildManager.Instance.ShowTowerStatsDisplay(tower.ItemPrefab.GetComponent<TowerScript>().towerData);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DragStart.Invoke();
            BuildManager.Instance.item = tower;
            BuildManager.Instance.StartDrag(gameObject);
        }
    }

    private void OnMouseExit()
    {
        BuildManager.Instance.HideStatsDisplay();
    }
}
