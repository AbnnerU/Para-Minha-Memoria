
using UnityEngine;

public class OnTriggerColetable : MonoBehaviour
{
    [SerializeField] private string targetTag;

    [SerializeField] private bool selfDisable=true;

    [SerializeField] private InteractionAction[] actions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) == false)
            return;

        foreach (InteractionAction i in actions)
        {
            i.ExecuteAction();
        }

        if (selfDisable)
            gameObject.SetActive(false);
    }
}
