using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlidePuzzleManager : MonoBehaviour
{
    [SerializeField] private bool startDisabled = false;

    [SerializeField] private bool active = true;

    [SerializeField] private MapGrid mapGrid;
   
    [SerializeField] private GridContent emptySlot;

    [Header("Ray config")]
    [Space(5)]

    [SerializeField] private Camera cameraReference;

    [SerializeField] private LayerMask layerMask;

    [Header("Sequence")]
    [Space(5)]

    [SerializeField] private bool validateSlidePuzzle;

    [SerializeField] private bool preview;

    [SerializeField]
    private List<PuzzleSequence> correctSequence = new List<PuzzleSequence>();

    private InputController inputController;

    private RaycastHit2D[] results = new RaycastHit2D[1];

    private float distance;  

    public Action OnCompleteAction;

    public Action OnMoveBlock;

    private bool started = false;

    private void Awake()
    {
        if (cameraReference == null)
            cameraReference = Camera.main;

        if (mapGrid == null)
            mapGrid = GetComponent<MapGrid>();

        inputController = FindObjectOfType<InputController>();

        inputController.OnLeftClickEvent += InputController_OnLeftClickEvent;
        inputController.OnMoveEvent += InputController_OnMoveEvent;

        CalculateDistance();
    }
  
    private void OnEnable()
    {
        if (startDisabled && started == false)
        {
            active = false;
            started = true;
            return;
        }
        else
        {
            active = true;
        }
    }

    private void OnDisable()
    {
        active = false;
    }

    private void CalculateDistance()
    {
        distance = mapGrid.GetCellsSize().x + mapGrid.GetCellsSpacing().x; 
    }

    private void InputController_OnLeftClickEvent()
    {
        if (active)
        {
            Ray ray = cameraReference.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics2D.RaycastNonAlloc(ray.origin, ray.direction, results, Mathf.Infinity,layerMask,0) > 0)
            {
                BlockDetection block = results[0].collider.GetComponent<BlockDetection>();

                //print(results[0].collider.name);

                if (block && emptySlot)
                {
                    if ((emptySlot.GetTransform().position - block.GetBlockParent().position).magnitude == distance)
                    {
                        Vector3 prevuiousPos = block.GetBlockParent().position;

                        block.GetBlockParent().position = emptySlot.GetTransform().position;

                        emptySlot.GetTransform().position = prevuiousPos;

                        SwitchCells(block);

                        if (validateSlidePuzzle)
                            VerifySequence();
                    }
                }
            }
        }
           
    }

    private void InputController_OnMoveEvent(Vector2 input)
    {
        if (active)
        {
            int emptySlotId = mapGrid.TryGetContentCellId(emptySlot);

            if (emptySlotId < 0) {
                Debug.LogError("EmptySlot id not finded");
                return;
            }

            int blockId = -1;
           

            if (input.x < 0)
            {
                int validationId = emptySlotId + 1;

                Vector2 intervalValue = StartAndEndId(emptySlotId);

                if (mapGrid.TryGetContent(validationId) != null && validationId >= intervalValue.x && validationId < intervalValue.y) 
                    blockId = emptySlotId + 1;
            }
            else if (input.x>0)
            {
                int validationId = emptySlotId - 1;

                Vector2 intervalValue = StartAndEndId(emptySlotId);

                if (mapGrid.TryGetContent(validationId)!=null && validationId >= intervalValue.x && validationId < intervalValue.y)
                    blockId = emptySlotId-1;
            }
            else if (input.y > 0)
            {
                int proporcionX = (int)mapGrid.GetGridProporcion().x;
                int diff = ((proporcionX - 1) - emptySlotId);
                int blockIdBelow = emptySlotId - diff - ((proporcionX) - diff);

                if (mapGrid.TryGetContent(blockIdBelow) != null)
                    blockId = blockIdBelow;
            }
            else if (input.y < 0)
            {
                int proporcionX = (int)mapGrid.GetGridProporcion().x;
                int diff = ((proporcionX - 1) - emptySlotId);
                int blockIdAbove = emptySlotId + diff + ((proporcionX) - diff);

                if (mapGrid.TryGetContent(blockIdAbove) != null)
                    blockId = blockIdAbove;            
            }


            if (blockId >= 0)
            {
                GridContent block = mapGrid.TryGetContent(blockId);

                Vector3 prevuiousPos = block.GetTransform().position;

                block.GetTransform().position = emptySlot.GetTransform().position;

                emptySlot.GetTransform().position = prevuiousPos;

                SwitchCells(block);

                if (validateSlidePuzzle)
                    VerifySequence();
            }
            
        }
    }

    private void SwitchCells(BlockDetection blockReference)
    {
        int emptySlotId = mapGrid.TryGetContentCellId(emptySlot);
        int blockSlotId = mapGrid.TryGetContentCellId(blockReference.GetGridContent());

        if(emptySlotId>=0 && blockSlotId >= 0)
        {
            mapGrid.SwitchCellsContent(emptySlotId, blockSlotId, true);

            OnMoveBlock?.Invoke();
        }
        else
        {
            Debug.LogError("Id error: EmptySlotId: " + emptySlotId + " | BlockID: " + blockSlotId);
        }
    }

    private void SwitchCells(GridContent blockReference)
    {
        int emptySlotId = mapGrid.TryGetContentCellId(emptySlot);
        int blockSlotId = mapGrid.TryGetContentCellId(blockReference);

        if (emptySlotId >= 0 && blockSlotId >= 0)
        {
            mapGrid.SwitchCellsContent(emptySlotId, blockSlotId, true);

            OnMoveBlock?.Invoke();
        }
        else
        {
            Debug.LogError("Id error: EmptySlotId: " + emptySlotId + " | BlockID: " + blockSlotId);
        }
    }

    private void VerifySequence()
    {
        for (int i=0; i < correctSequence.Count; i++)
        {
            PuzzleSequence currentSequence = correctSequence[i];

            if (currentSequence.content != mapGrid.TryGetContent(currentSequence.cellId))
            {
                print("Wrong sequence :" +currentSequence.cellId);
                return;
            }
        }

        OnCompleteAction?.Invoke();
        print("COMPLETED");

    }

    private Vector2 StartAndEndId(int emptySlotId)
    {
        Vector2 proporcion = mapGrid.GetGridProporcion();
        float start = 0;
        float end = 0;

        for(int i =0; i < proporcion.y; i++)
        {
            if(emptySlotId >= (proporcion.x*i) && emptySlotId < (proporcion.x* (i + 1)))
            {
                start = (proporcion.x * i);
                end = (proporcion.x * (i + 1));
                break;
            }
        }

        return new Vector2(start, end);              
    }

    private Vector2 Position(Vector2 startPointReference, Vector2 gridProporcion, Vector2 cellsSize, Vector2 cellsSpacing, int id)
    {
        int amount = 0;

        for (int y = 0; y < gridProporcion.y; y++)
        {
            float spacingX = 0;
            for (int x = 0; x < gridProporcion.x; x++)
            {
                if (amount == id)
                {
                    Vector2 add = new Vector2(x, y);

                    return ((Vector2)startPointReference + ((add * cellsSize) + (Vector2.right * spacingX))) + (new Vector2(0, 0.1f));

                }
                spacingX += cellsSpacing.x;

                amount++;
            }
            startPointReference += Vector2.up * cellsSpacing.y;
        }

        return Vector2.zero;
    }

    public void CompletePuzzle()
    {
        OnCompleteAction?.Invoke();
        print("COMPLETED");
    }

    private void OnDrawGizmos()
    {
        if (preview)
        {
            if (correctSequence.Count > 0)
            {
                Vector2 gridProporcion = mapGrid.GetGridProporcion();
                float cellSize = mapGrid.GetCellsSize().x;
                Vector2 spacing = mapGrid.GetCellsSpacing();


                foreach(PuzzleSequence ps in correctSequence)
                {
                    if (ps.content != null)
                    {
                        Gizmos.color = ps.gizmosColor;

                        Gizmos.DrawCube(ps.content.transform.position,new Vector3(1,1,0.1f));

                    }

                }

                foreach (PuzzleSequence ps in correctSequence)
                {
                    if (ps.content != null)
                    {
                        Gizmos.color = ps.gizmosColor;
                  
                        Gizmos.DrawLine(ps.content.transform.position, Position(mapGrid.GetStartPoint().position, gridProporcion, new Vector2(cellSize, cellSize), spacing, ps.cellId));
                      
                    }

                }

                foreach (PuzzleSequence ps in correctSequence)
                {
                    if (ps.content != null)
                    {
                        Gizmos.color = ps.gizmosColor;

                        Gizmos.DrawSphere(Position(mapGrid.GetStartPoint().position, gridProporcion, new Vector2(cellSize, cellSize), spacing, ps.cellId), 0.2f);
                    }

                }

            }
        }

        
    }

    public Camera GetCamera()
    {
        return cameraReference;
    }

    public LayerMask GetLayerMask()
    {
        return layerMask;
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetActive(bool isActive)
    {
        active = isActive;
    }

}

[System.Serializable]
public struct PuzzleSequence
{
    public int cellId;

    public GridContent content;

    public Color gizmosColor;
}
