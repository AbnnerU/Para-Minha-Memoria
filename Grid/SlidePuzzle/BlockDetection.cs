
using UnityEngine;

public class BlockDetection : MonoBehaviour
{
    [SerializeField] private Transform parent;

    [SerializeField] private GridContent content;

    private void Awake()
    {
        if (parent == null)
            parent = GetComponentInParent<Transform>();

        if (content == null)
            content = parent.GetComponent<GridContent>();

        if (content == null)
            Debug.LogError("Do not have content component at object " + parent.name);
    }

    public GridContent GetGridContent()
    {
        return content;
    }

    public Transform GetBlockParent()
    {
        return parent;
    }
}
