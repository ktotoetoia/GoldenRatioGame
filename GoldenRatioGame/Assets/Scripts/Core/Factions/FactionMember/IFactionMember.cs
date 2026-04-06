namespace IM.Factions
{
    public interface IFactionMember : IFactionMemberReadOnly
    {
        new IFaction Faction { get; set; }
    }
}