namespace IM.SaveSystem
{
    public interface ISaveSlotFactory<in T>
    {
        SaveSlotData CreateFromSave(T save, int slotIndex);
        SaveSlotData CreateEmpty(int slotIndex);
    }
}