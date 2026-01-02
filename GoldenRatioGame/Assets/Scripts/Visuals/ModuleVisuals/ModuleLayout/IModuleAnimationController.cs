using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleAnimationController : IModuleExtension
    {
        IAnimationModule ReferenceModule { get; }
        
        ITransformPort GetReferencePort(IPort port);
        IAnimationModule CreateNewReferenceModule();
        IAnimationModule CreateAnimationModuleCopy(IDictionary<IPort, ITransformPort> visualPortMap);
    }
}