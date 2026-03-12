namespace IM.SaveSystem
{
    public interface IStateSerializable
    {
        GameObjectData Capture();
        void Restore(GameObjectData data);
    }
}