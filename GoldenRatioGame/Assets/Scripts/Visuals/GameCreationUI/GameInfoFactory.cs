using IM.LifeCycle;

namespace IM.SaveSystem
{
    public class GameInfoFactory : ISaveSlotFactory<GameInfo>
    {
        public SaveSlotData CreateFromSave(GameInfo save, int slotIndex)
        {
            return new SaveSlotData
            {
                SlotIndex = slotIndex,
                IsEmpty  = false,
                DisplayName = $"Slot {slotIndex + 1}",
            };
        }

        public SaveSlotData CreateEmpty(int slotIndex)=> new ()
        {
            SlotIndex = slotIndex,
            IsEmpty = true,
            DisplayName = $"Slot {slotIndex + 1}",
        };
    }
}