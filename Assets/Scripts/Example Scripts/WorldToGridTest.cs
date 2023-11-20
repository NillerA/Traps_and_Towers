using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldToGridTest : MonoBehaviour
{

    public GridManager grid;
    GameObject current;
    public GridTileItem item;

    public InputAction MouseDown;
    public bool IsDown;

    public void Start()
    {
        MouseDown.Enable();
        //MouseDown.started += StartDrag;
        MouseDown.canceled += StopDrag;
    }

    public void StartDrag(InputAction.CallbackContext context)
    {
        IsDown = true;
        StartCoroutine(Drag());
    }

    public void StopDrag(InputAction.CallbackContext context)
    {
        Debug.Log("Stop");
        IsDown = false;
    }

    private IEnumerator Drag()
    {
        (int gridX, int gridY) = (-1,-1);
        while (IsDown)
        {
            //these 2 lines would work if camera wasent at an angle
            //Vector2 mousePos = Input.mousePosition;//gets the mouse position
            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,10));//uses simple math to get a position in world space (z coordinate) amount away from camera in the direction under the mouse
        
            //because of the angle we use these lines instead
            Vector2 mousePos = Input.mousePosition;//gets the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePos);//turns the mouse position into a raycast direction
            Vector3 worldPos = -Vector3.one;//creates the worldPos variable and sets it to negative one on all axis so that if we dont hit anything it will not be on tile 0,0
            if(Physics.Raycast(ray, out RaycastHit hitInfo))//checs if theres anything under the mouse in 3d space
                worldPos = hitInfo.point;//sets worldPos to the point where theres somthing under the mouse
            //Debug.Log(worldPos);//logs the worldPos

            (gridX, gridY) = grid.WorldToGrid(worldPos);//turns vector3 worldPos into an x and y codinate on the grid
            if (gridX >= 0 && gridY >= 0 && gridX < grid.GetXGridSize() && gridY < grid.GetYGridSize())//checks if grid coordinate is out of bounds of the grid size
                if (current != grid.GetVisualTile(gridX, gridY))//checks if last frames tile is not the same as the tile we are hovering over now
                {
                    if (current != null)//checks if last tile was not null
                        current.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;//sets last tiles color back to green
                    current = grid.GetVisualTile(gridX, gridY);//sets current tile to the tile on the coordinates we are hovering at
                    current.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;//sets the tiles color that we cuurntly are hovering over to green
                }
            yield return null;
        }
        if (grid.PlaceTileItem(gridX, gridY, item))
            Debug.Log("Succes");
        else
            Debug.Log("not valid");
    }
}
