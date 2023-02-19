
public interface IDragAndDropInteractable
{
    void OnEnter(InventoryItem item);

    void OnExit(InventoryItem item);

    void OnClick(InventoryItem item, out bool consumeItem);
}