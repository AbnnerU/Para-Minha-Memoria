using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [SerializeField]
    private bool drawGizmos;

    [SerializeField]
    private bool preview = true;

    [SerializeField]
    private Transform startPoint;

    //[SerializeField]
    //private GameObject dropAreaPrefab;

    [Header("Grid Config | Left To Right | Down To Up")]
    [Space(10)]

    [SerializeField]
    private Vector2 gridProporcion;

    [SerializeField]
    private Vector2 cellsSize;

    [SerializeField]
    private Vector2 cellsSpacing;

    [Header("Content")]
    [Space(10)]

    [SerializeField]
    private List<GridContent> cellsContent = new List<GridContent>();

    [SerializeField]
    private List<CellsConfig> cellsConfig = new List<CellsConfig>();

    private List<Vector2> cellsMiddle = new List<Vector2>();

    private void Awake()
    {
        UpdateCells();
    }

    public void UpdateCells()
    {
        Vector2 startPointReference = startPoint.position;
        int totaMapContent = cellsContent.Count;
        int contentID = 0;
        int cellId = 0;

        cellsConfig.Clear();
        cellsConfig.Capacity = 0;


        cellsMiddle.Clear();
        cellsMiddle.Capacity = 0;

        for (int y = 0; y < gridProporcion.y; y++)
        {
            float spacingX = 0;
            for (int x = 0; x < gridProporcion.x; x++)
            {
                CellsConfig cell = new CellsConfig();

                Vector2 add = new Vector2(x, y);
                cellsMiddle.Add((Vector2)startPointReference + ((add * cellsSize) + (Vector2.right * spacingX)));

                spacingX += cellsSpacing.x;

                cell.id = cellId;
                cell.cellMiddle = cellsMiddle[cellId];

                if (contentID < totaMapContent)
                {
                    cell.cellState = CellState.FILLED;
                    cell.content = cellsContent[contentID];

                    contentID++;
                }
                else
                {
                    cell.cellState = CellState.EMPTY;

                    cell.content = null;
                }

                cellsConfig.Add(cell);

                cellId++;
            }
            startPointReference += Vector2.up * cellsSpacing.y;
        }
    }

    private void UpdateMapGridContent()
    {
        if (cellsContent.Count == 0)
            return;

        Vector2 startPointReference = startPoint.position;
        int totaMapContent = cellsContent.Count;
        int amount = 0;

        for (int y = 0; y < gridProporcion.y; y++)
        {
            float spacingX = 0;
            for (int x = 0; x < gridProporcion.x; x++)
            {
                if (amount < totaMapContent)
                {
                    Vector2 add = new Vector2(x, y);

                    cellsContent[amount].gameObject.transform.position = (Vector2)startPointReference + ((add * cellsSize) + (Vector2.right * spacingX));

                    spacingX += cellsSpacing.x;

                    amount++;
                }
                else
                {
                    break;
                }
            }
            startPointReference += Vector2.up * cellsSpacing.y;
        }
    }

    public void ResetGrid()
    {
        UpdateMapGridContent();

        UpdateCells();
    }

    public bool IsInsideOfAnGridCell(Vector3 positionReference, out Vector2 cellMiddle, out int cellID)
    {
        cellMiddle = Vector2.zero;

        cellID = 0;

        if (IsInsideOfMapArea(positionReference) == false)
            return false;

        for (int i = 0; i < cellsConfig.Count; i++)
        {
            Vector2 vector = cellsConfig[i].cellMiddle;

            float cellLeftXBorder = vector.x - (cellsSize.x / 2);
            float cellRigthXBorder = vector.x + (cellsSize.x / 2);

            float cellDownYBorder = vector.y - (cellsSize.y / 2);
            float cellUpYBorder = vector.y + (cellsSize.y / 2);

            if (positionReference.x > cellLeftXBorder && positionReference.x < cellRigthXBorder)
            {
                if (positionReference.y > cellDownYBorder && positionReference.y < cellUpYBorder)
                {
                    cellMiddle = vector;

                    cellID = cellsConfig[i].id;

                    print("Is on cell" + vector);
                    print("Cell id: " + cellID);
                    return true;
                }
            }

        }

        print("In out of all cells");
        return false;
    }

    public bool IsInsideOfMapArea(Vector3 positionReference)
    {
        float leftXBorder = startPoint.position.x - (cellsSize.x / 2);
        float rigthXBorder = leftXBorder + (cellsSize.x * gridProporcion.x) + (cellsSpacing.x * gridProporcion.x);

        float downYBorder = startPoint.position.y - (cellsSize.y / 2);
        float upYBorder = downYBorder + (cellsSize.y * gridProporcion.y) + (cellsSpacing.y * gridProporcion.y); ;

        if (positionReference.x > leftXBorder && positionReference.x < rigthXBorder)
        {
            if (positionReference.y > downYBorder && positionReference.y < upYBorder)
            {
                print("In on map");
                return true;
            }
            else
            {
                print("In out of map");
                return false;
            }
        }
        else
        {
            print("In out of map");
            return false;
        }
    }

    #region CellsInfo/Changes
    public void TrySetNewContentAtCell(MapGridContent newContent, int id)
    {
        int index = cellsConfig.FindIndex(x => x.id == id);

        if (index < 0)
        {
            Debug.LogError(id + " cell dont exist");
            return;
        }
        else
        {
            cellsConfig[index].content = newContent;
            cellsConfig[index].cellState = CellState.FILLED;
        }

    }

    public void ChangeStateOFCell(CellState state, int id)
    {
        int index = cellsConfig.FindIndex(x => x.id == id);

        if (index < 0)
        {
            Debug.LogError(id + " cell dont exist");
            return;
        }
        else
        {
            cellsConfig[index].cellState = state;
        }
    }

    public GridContent TryGetContent(GridContent reference)
    {
        int index = cellsConfig.FindIndex(x => x.content == reference);

        if (index < 0)
        {
            Debug.LogError(reference + " dont exist");
            return null;
        }
        else
        {
            return cellsConfig[index].content;
        }
    }

    public GridContent TryGetContent(int contentId)
    {
        int index = cellsConfig.FindIndex(x => x.id == contentId);

        if (index < 0)
        {
            Debug.LogWarning(contentId + " dont exist");
            return null;
        }
        else
        {
            return cellsConfig[index].content;
        }
    }

    public int TryGetContentCellId(GridContent reference)
    {
        int index = cellsConfig.FindIndex(x => x.content == reference);

        if (index < 0)
        {
            Debug.LogError(reference + " dont exist");
            return -1;
        }
        else
        {
            //print("ID: " + index);

            return cellsConfig[index].id;
        }
    }

    public void TrySetCellContentNull(int id, out bool complete)
    {
        int index = cellsConfig.FindIndex(x => x.id == id);

        if (index < 0)
        {
            Debug.LogError(id + " not exist");
            complete = false;
            return;
        }
        else
        {
            cellsConfig[index].cellState = CellState.EMPTY;

            cellsConfig[index].content = null;

            complete = true;
        }

    }

    public void SwitchCellsContent(int cellOneID, int cellTwoID, bool updatePositions)
    {
        if (ValidyCellID(cellOneID) && ValidyCellID(cellTwoID))
        {
            GridContent cellOneContent = null;
            GridContent cellTwoContent = null;

            cellOneContent = cellsConfig[cellOneID].content;
            cellTwoContent = cellsConfig[cellTwoID].content;

            //print(cellOneID + "|" + cellOneContent);

            //print(cellTwoID + "|" + cellTwoContent);

            if (cellOneContent != null)
            {
                cellsConfig[cellTwoID].content = cellOneContent;

                cellsConfig[cellTwoID].cellState = CellState.FILLED;

                if (updatePositions)
                    cellOneContent.transform.position = cellsConfig[cellTwoID].cellMiddle;
            }
            else
            {
                cellsConfig[cellTwoID].content = null;

                cellsConfig[cellTwoID].cellState = CellState.EMPTY;
            }


            if (cellTwoContent != null)
            {
                cellsConfig[cellOneID].content = cellTwoContent;

                cellsConfig[cellOneID].cellState = CellState.FILLED;

                if (updatePositions)
                    cellTwoContent.transform.position = cellsConfig[cellOneID].cellMiddle;
            }
            else
            {
                cellsConfig[cellOneID].content = null;

                cellsConfig[cellOneID].cellState = CellState.EMPTY;
            }
        }
    }

    public bool ValidyCellID(int id)
    {
        int index = cellsConfig.FindIndex(x => x.id == id);

        if (index < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    public List<CellsConfig> GetCellsConfig()
    {
        return cellsConfig;
    }

    public Vector2 GetGridProporcion()
    {
        return gridProporcion;
    }

    public List<GridContent> GetCellsContent()
    {
        return cellsContent;
    }

    public Transform GetStartPoint()
    {
        return startPoint;
    }

    private void OnValidate()
    {
        if (preview)
        {
            UpdateMapGridContent();          
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Vector2 startPointReference = startPoint.position;

            for (int y = 0; y < gridProporcion.y; y++)
            {
                float spacingX = 0;
                for (int x = 0; x < gridProporcion.x; x++)
                {
                    Vector2 add = new Vector2(x, y);
                    Color color = Color.red;
                    color.a = 0.5f;

                    Gizmos.color = color;
                    Gizmos.DrawCube((Vector2)startPointReference + ((add * cellsSize) + (Vector2.right * spacingX)), cellsSize);
                    spacingX += cellsSpacing.x;
                }
                startPointReference += Vector2.up * cellsSpacing.y;
            }
        }
    }

    public Vector2 GetCellsSize()
    {
        return cellsSize;
    }

    public Vector2 GetCellsSpacing()
    {
        return cellsSpacing;
    }
}

[Serializable]
public class CellsConfig
{
    public int id;

    public Vector2 cellMiddle;

    public CellState cellState;

    public GridContent content;


}

public enum CellState
{
    EMPTY,
    FILLED
}



