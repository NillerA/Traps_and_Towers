using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{

    public static BuildManager Instance;

    [SerializeField]
    private Shop Shop;
    [SerializeField]
    private CircleRend radiusShowcase;
    [SerializeField]
    private GameObject towerStatsDisplay;
    [SerializeField]
    private TextMeshProUGUI towerName, rangeText, damageText, attackSpeedText, attackTypeText, descriptionText;
    [SerializeField]
    GameObject tileStatsDisplay;
    [SerializeField]
    private TextMeshProUGUI tileNameText, tileWalkSpeedText, tileDescriptionText;
    private GameObject towerShowcase, towerShowcaseBad;
    public InputAction mouseDown;
    
    private GameObject current;
    [HideInInspector]
    public TowerItem item;
    private bool isDown;

    Coroutine drag;

    [SerializeField]
    GameObject tower1, tower2, tower3;

    [SerializeField]
    List<TowerItem> towerDeck;
    List<TowerItem> NotUsedFromDeck = new List<TowerItem>();

    BuildManager() 
    {
        Instance = this;
    }

    private void Start()
    {
        mouseDown.Enable();
        mouseDown.canceled += StopDrag;
    }

    public void CanPlaceTurrets()
    {
        NotUsedFromDeck.Clear();
        foreach (TowerItem toweritem in towerDeck)
        {
            NotUsedFromDeck.Add(toweritem);
        }
        TowerItem chosenItem = NotUsedFromDeck[Random.Range(0, NotUsedFromDeck.Count)];
        NotUsedFromDeck.Remove(chosenItem);
        tower1.GetComponent<DragableTower>().SetItem(chosenItem);
        tower1.SetActive(true);
        chosenItem = NotUsedFromDeck[Random.Range(0, NotUsedFromDeck.Count)];
        NotUsedFromDeck.Remove(chosenItem);
        tower2.GetComponent<DragableTower>().SetItem(chosenItem);
        tower2.SetActive(true);
        chosenItem = NotUsedFromDeck[Random.Range(0, NotUsedFromDeck.Count)];
        NotUsedFromDeck.Remove(chosenItem);
        tower3.GetComponent<DragableTower>().SetItem(chosenItem);
        tower3.SetActive(true);
    }

    public void ShowTowerStatsDisplay(TowerData towerData)
    {
        towerStatsDisplay.SetActive(true);
        towerName.text = towerData.name;
        rangeText.text = towerData.viewDistance.ToString();
        damageText.text = towerData.damage.ToString();
        attackSpeedText.text = towerData.attackSpeed.ToString();
        attackTypeText.text = towerData.attackType;
        descriptionText.text = towerData.description;
    }

    public void HideStatsDisplay()
    {
        towerStatsDisplay.SetActive(false);
    }

    public void ShowTileStatsDisplay(GridTileItem item)
    {
        tileStatsDisplay.gameObject.SetActive(true);
        tileNameText.text = item.name;
        tileWalkSpeedText.text = item.WalkSpeed.ToString();
        tileDescriptionText.text = item.description;
    }

    public void HideTileStatsDisplay()
    {
        tileStatsDisplay.SetActive(false);
    }

    public void StartDrag(GameObject objectToDrag)
    {
        if (drag != null)
            StopCoroutine(drag);
        isDown = true;
        drag = StartCoroutine(Drag(objectToDrag));
    }

    public void StopDrag(InputAction.CallbackContext context)
    {
        if (isDown)
        {
            isDown = false;
            radiusShowcase.Hide();
            if (towerShowcase != null)
                Destroy(towerShowcase);
            if (towerShowcaseBad != null)
                Destroy(towerShowcaseBad);
            WaveManager.Instance.UpdatePath();
        }
    }

    public IEnumerator Drag(GameObject objectToDrag)
    {
        towerShowcase = Instantiate(item.PreviewPrefab, radiusShowcase.transform.position, radiusShowcase.transform.rotation, radiusShowcase.transform);
        towerShowcase.SetActive(false);
        towerShowcaseBad = Instantiate(item.PreviewPrefabBad, radiusShowcase.transform.position, radiusShowcase.transform.rotation, radiusShowcase.transform);
        towerShowcaseBad.SetActive(false);
        (int gridX, int gridY) = (-1, -1);
        bool startAnim = true;
        float timer = 0;
        while (startAnim)
        {
            Vector2 mousePos = Input.mousePosition;//gets the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePos);//turns the mouse position into a raycast direction
            Vector3 worldPos = -Vector3.one;//creates the worldPos variable and sets it to negative one on all axis so that if we dont hit anything it will not be on tile 0,0
            if (Physics.Raycast(ray, out RaycastHit hitInfo2))//checs if theres anything under the mouse in 3d space
                worldPos = hitInfo2.point;//sets worldPos to the point where theres somthing under the mouse
            objectToDrag.GetComponent<BoxCollider>().enabled = false;
            timer += Time.deltaTime;
            if (timer > 0.5f)
                startAnim = false;
            objectToDrag.transform.position = Vector3.Lerp(objectToDrag.transform.position, worldPos, 10 * Time.deltaTime);
            objectToDrag.transform.rotation = Quaternion.Lerp(objectToDrag.transform.rotation, transform.rotation, 10 * Time.deltaTime);
            yield return null;
        }

        while (isDown)
        {
            Vector2 mousePos = Input.mousePosition;//gets the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePos);//turns the mouse position into a raycast direction
            Vector3 worldPos = -Vector3.one;//creates the worldPos variable and sets it to negative one on all axis so that if we dont hit anything it will not be on tile 0,0
            if (Physics.Raycast(ray, out RaycastHit hitInfo))//checs if theres anything under the mouse in 3d space
                worldPos = hitInfo.point;//sets worldPos to the point where theres somthing under the mouse
            objectToDrag.GetComponent<BoxCollider>().enabled = false;
            
            (gridX, gridY) = GridManager.Instance.WorldToGrid(worldPos);//turns vector3 worldPos into an x and y codinate on the grid
            if (gridX >= 0 && gridY >= 0 && gridX < GridManager.Instance.GetXGridSize() && gridY < GridManager.Instance.GetYGridSize())//checks if grid coordinate is out of bounds of the grid size
            {
                if (current != GridManager.Instance.GetVisualTile(gridX, gridY))//checks if last frames tile is not the same as the tile we are hovering over now
                {
                    //if (current != null)//checks if last tile was not null
                        //current.transform.GetChild(0).GetComponent<Renderer>().material.DisableKeyword("_EMISSION");//turns emmision off for the tile material
                    current = GridManager.Instance.GetVisualTile(gridX, gridY);//sets current tile to the tile on the coordinates we are hovering at
                    //current.transform.GetChild(0).GetComponent<Renderer>().material.EnableKeyword("_EMISSION");//turns emmision on for the tile material
                    radiusShowcase.transform.position = current.transform.position;
                    objectToDrag.transform.position = current.transform.position;
                    radiusShowcase.Draw(item.ItemPrefab.GetComponent<TowerScript>().towerData.viewDistance);
                    if (GridManager.Instance.CanPlaceItem(gridX, gridY) && WaveManager.Instance.UpdatePath(new Point(gridX,gridY)))
                    {
                        //current.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
                        objectToDrag.SetActive(true);
                        //towerShowcase.SetActive(true);
                        towerShowcaseBad.SetActive(false);
                        radiusShowcase.ChangeColor(Color.white);
                    }
                    else
                    {
                        objectToDrag.SetActive(false);
                        //towerShowcase.SetActive(false);
                        towerShowcaseBad.SetActive(true);
                        //current.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                        radiusShowcase.ChangeColor(Color.red);
                    }
                }
            }
            else
            {
                objectToDrag.SetActive(true);
                objectToDrag.transform.position = Vector3.Lerp(objectToDrag.transform.position, worldPos, 10 * Time.deltaTime);
                towerShowcase.SetActive(false);
                towerShowcaseBad.SetActive(false);
                radiusShowcase.Hide();
            }
            yield return null;
        }
        if (WaveManager.Instance.UpdatePath(new Point(gridX, gridY)))
        {
            if (GridManager.Instance.PlaceTileItem(gridX, gridY, item))
            {
                tower1.SetActive(false);
                tower2.SetActive(false);
                tower3.SetActive(false);
                GameManager.Instance.TowerPlaced();
            }
            else
            {
                objectToDrag.SetActive(true);
                Debug.LogWarning("Failed to place item because position is not valid (insert sound/screenshake or something to notify the player)");
            }
        }
        else
        {
            objectToDrag.SetActive(true);
            Debug.LogWarning("Failed to place item because theres is not a valid path for the enemys (insert sound/screenshake or something to notify the player)");
        }
        //current.transform.GetChild(0).GetComponent<Renderer>().material.DisableKeyword("_EMISSION");//turns emmision off for the tile material
        objectToDrag.GetComponent<BoxCollider>().enabled = true;
        objectToDrag.GetComponent<DragableTower>().ResetTower();
    }
}
