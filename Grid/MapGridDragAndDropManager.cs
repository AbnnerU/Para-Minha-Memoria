using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.GridMap
{
    public class MapGridDragAndDropManager : MonoBehaviour
    {
        [SerializeField] private MapGrid mapGrid;

        [SerializeField]
        private Dictionary<GridContent, GridMapObjectDrag> objectsDragArea = new Dictionary<GridContent, GridMapObjectDrag>();

        [SerializeField]
        private bool switchFilledCells;

        private void Start()
        {
            GetAllObjectsDrag();
        }

        private void GetAllObjectsDrag()
        {
            Vector2 gridProporcion = mapGrid.GetGridProporcion();

            int totaMapContent = mapGrid.GetCellsContent().Count;
            int amount = 0;

            for (int y = 0; y < gridProporcion.y; y++)
            {
                for (int x = 0; x < gridProporcion.x; x++)
                {
                    if (amount < totaMapContent)
                    {
                        GridMapObjectDrag drag = mapGrid.GetCellsContent()[amount].GetComponentInChildren<GridMapObjectDrag>();

                        if (drag != null)
                        {
                            objectsDragArea.Add(mapGrid.GetCellsContent()[amount], drag);
                        }
                        else
                        {
                            Debug.LogError("Cell content " + mapGrid.GetCellsContent()[amount] + " dont have component GridObjectDrag");
                            return;
                        }

                        amount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            foreach (GridMapObjectDrag g in objectsDragArea.Values)
            {
                g.OnStartDrag += ObjectDrag_OnStartDrag;
                g.OnEndDrag += ObjectDrag_OnEndDrag;
            }
        }

        private void ObjectDrag_OnStartDrag(Vector3 position, GridMapObjectDrag objectDrag)
        {
            Transform transformToAffect = objectDrag.GetContentTransform();

            transformToAffect.position = position;

            Vector3 local = transformToAffect.localPosition;

            transformToAffect.localPosition = new Vector3(local.x, local.y, 0);
        }

        private void ObjectDrag_OnEndDrag(Vector3 endPosition, GridMapObjectDrag objectDrag)
        {
            Vector2 middlePosition = Vector2.zero;
            int cellID = 0;
            print(endPosition);

            if (mapGrid.IsInsideOfAnGridCell((Vector2)endPosition, out middlePosition, out cellID)) //Drop inside of an cell
            {
                if (switchFilledCells)
                {
                    SwitchCellsContent(objectDrag, middlePosition, cellID);
                }
                else
                {
                    CellsConfig cell = mapGrid.GetCellsConfig().Find(x => x.id == cellID);

                    if (cell.cellState == CellState.EMPTY)
                    {
                        int oldCellID = mapGrid.TryGetContentCellId(objectDrag.GetMapGridContent());

                        if (oldCellID >= 0)
                        {
                            mapGrid.SwitchCellsContent(oldCellID, cellID, true);

                            objectDrag.SetNewDefaltPostion(middlePosition);
                        }
                        else
                        {
                            Debug.LogError(oldCellID + " not exist");
                        }
                    }
                    else
                    {
                        objectDrag.GetContentTransform().position = middlePosition;

                        objectDrag.SetNewDefaltPostion(middlePosition);
                    }

                }
            }
            else
            {
                objectDrag.SetToDefaltPosition();
            }

        }

        private void SwitchCellsContent(GridMapObjectDrag objectDrag, Vector2 middlePosition, int cellID)
        {
            CellsConfig cell = mapGrid.GetCellsConfig().Find(x => x.id == cellID);

            if (cell.cellState == CellState.EMPTY)
            {
                print("Droped on empty cell");
                int oldCellID = mapGrid.TryGetContentCellId(objectDrag.GetMapGridContent());

                if (oldCellID >= 0)
                {
                    mapGrid.SwitchCellsContent(oldCellID, cellID, true);

                    objectDrag.SetNewDefaltPostion(middlePosition);
                }
                else
                {
                    Debug.LogError(oldCellID + " not exist");
                }
            }
            else
            {
                print("Droped on filled cell");
                GridMapObjectDrag objectInCurrentCell = GetObjectDrag(cell.content);

                int oldCellID = mapGrid.TryGetContentCellId(objectDrag.GetMapGridContent());

                if (oldCellID >= 0 && objectInCurrentCell != null)
                {
                    mapGrid.SwitchCellsContent(oldCellID, cellID, true);

                    objectInCurrentCell.SetCurrentParentPositionAsDefalt();

                    objectDrag.SetNewDefaltPostion(middlePosition);
                }
                else
                {
                    Debug.LogError(oldCellID + " or " + objectInCurrentCell + " not exist");
                }
            }
        }

        private GridMapObjectDrag GetObjectDrag(GridContent reference)
        {
            GridMapObjectDrag gridMap;

            if (objectsDragArea.TryGetValue(reference, out gridMap))
                return gridMap;
            else
                return null;
        }


    }
}



    