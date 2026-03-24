namespace IM.Factions
{
    public interface IFactionDatabase
    {
        string GetIdOf(IFaction faction);
        IFaction GetById(string id);
    }
}