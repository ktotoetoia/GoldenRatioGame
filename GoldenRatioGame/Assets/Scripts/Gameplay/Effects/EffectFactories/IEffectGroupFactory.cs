using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Effects
{
    public interface IEffectGroupFactory : IFactory<IEffectGroup,IEffectContext>, IFactory<IEffectGroup,IEnumerable<IEffectModifier>,IEffectContext>
    {
        
    }
}