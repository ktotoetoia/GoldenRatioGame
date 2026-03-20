namespace IM.Factions
{
    public interface IFaction
    {
        FactionRelation Get(IFaction target);
    }
}