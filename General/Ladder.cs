using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform middle;

    [SerializeField] private Collider2D plataformCollider;

    [SerializeField] private LadderPart part;

    public enum LadderPart
    {
        START,
        MIDDLE,
        END
    }


    public LadderPart GetLadderPart()
    {
        return part;
    }

    public Collider2D GetPlataformCollider()
    {
        return plataformCollider;
    }

    public Vector3 GetLadderMiddlePosition()
    {
        return middle.position;
    }
}
