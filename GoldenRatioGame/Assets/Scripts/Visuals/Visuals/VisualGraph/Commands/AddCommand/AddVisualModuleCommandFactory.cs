using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;

namespace IM.Visuals
{
    public class AddVisualModuleCommandFactory : IAddModuleCommandFactory
    {
        private readonly ITransform _parent;
        
        public AddVisualModuleCommandFactory(ITransform parent)
        {
            _parent = parent;
        }
        
        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            if(param1 is not IVisualModule visualModule)
                throw new ArgumentException($"{nameof(AddVisualModuleCommandFactory)} must be used with visual graph");

            return new AddVisualModuleCommand(visualModule, param2, _parent);
        }
    }
}