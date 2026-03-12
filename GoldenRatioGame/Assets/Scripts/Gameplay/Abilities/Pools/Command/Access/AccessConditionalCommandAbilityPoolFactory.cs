using IM.Common;

namespace IM.Abilities
{
    public class AccessConditionalCommandAbilityPoolFactory : IFactory<AccessConditionalCommandAbilityPool,IConditionalCommandAbilityPool>
    {
        public AccessConditionalCommandAbilityPool Create(IConditionalCommandAbilityPool param1)
        {
            return new AccessConditionalCommandAbilityPool(param1);
        }
    }
}