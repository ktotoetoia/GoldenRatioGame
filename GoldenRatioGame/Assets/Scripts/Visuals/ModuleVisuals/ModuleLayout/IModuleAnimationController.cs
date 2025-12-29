using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleAnimationController : IModuleExtension
    {
        IAnimationModule ReferenceModule { get; }
        
        IVisualPort GetReferencePort(IPort port);
        IAnimationModule CreateNewReferenceModule();
        IAnimationModule CreateVisualModuleCopy(IDictionary<IPort, IVisualPort> visualPortMap);
    }
}