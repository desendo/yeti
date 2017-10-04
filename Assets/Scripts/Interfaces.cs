public interface IInteractable    {



    bool isActive
    {
        get;
        set;
    }
    void DefaultInteraction();
   


}
public interface IPickable
{


    bool isActive
    {
        get;
        set;
    }
    void AddToInventory(Inventory inv);
    void DropFromInventory(Inventory inv);
    void Destroy();

}
