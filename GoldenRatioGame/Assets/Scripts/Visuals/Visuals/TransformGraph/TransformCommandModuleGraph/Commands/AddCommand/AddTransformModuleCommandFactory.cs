using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;

namespace IM.Visuals
{
    public class AddTransformModuleCommandFactory : IAddModuleCommandFactory
    {
        private readonly IHierarchyTransform _parent;
        
        public AddTransformModuleCommandFactory(IHierarchyTransform parent)
        {
            _parent = parent;
        }
        
        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            if(param1 is not ITransformModule visualModule)
                throw new ArgumentException($"{nameof(AddTransformModuleCommandFactory)} must be used with visual graph");

            return new AddTransformModuleCommand(visualModule, param2, _parent);
        }
    }
}