namespace IM.Factions
{
    public interface IFaction
    {
        FactionRelation GetRelationWith(IFaction other);
    }
}