using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Visuals
{
    public class ConnectTransformCommandFactory : IConnectCommandFactory
    {
        public IConnectCommand Create(IPort param1, IPort param2, ICollection<IConnection> param3)
        {
            if (param1 is not ITransformPort visual1 || param2 is not ITransformPort visual2)
                throw new ArgumentException($"{nameof(ConnectTransformCommandFactory)} must be used with visual graph"); 
            
            return new ConnectTransformModulesCommand(visual1,visual2,param3);
        }
    }
}