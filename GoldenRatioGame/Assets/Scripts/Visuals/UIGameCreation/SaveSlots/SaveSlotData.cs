namespace IM.SaveSystem
{
    public class SaveSlotData
    {
        public int SlotIndex { get; set; }
        public bool IsEmpty { get; set; }
        public string DisplayName { get; set; }
        public string Playtime { get; set; }
        public string LastSaved { get; set; } 
        public string ExtraInfo { get; set; }
    }
}