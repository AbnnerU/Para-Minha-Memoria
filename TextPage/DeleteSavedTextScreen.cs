
using UnityEngine;

public class DeleteSavedTextScreen : MonoBehaviour
{
    [SerializeField] private string defaultFileName="TextPages.dr";

    public void DeleteFile()
    {
        SaveTextScreen.DeleteFile(defaultFileName);
    }
}
