namespace IM.WeaponSystem
{
    public interface IWeaponContainer : IWeaponContainerReadOnly
    {
        new IWeapon Weapon { get; set; } 
    }
}