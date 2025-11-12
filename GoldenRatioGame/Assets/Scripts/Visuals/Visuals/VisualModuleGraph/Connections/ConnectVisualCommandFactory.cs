using System;
using System.Collections.Generic;
using IM.Base;
using IM.Graphs;

namespace IM.Visuals
{
    public class ConnectVisualCommandFactory : IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>>
    {
        public IConnectCommand Create(IPort param1, IPort param2, ICollection<IConnection> param3)
        {
            if (param1 is not IVisualPort visual1 || param2 is not IVisualPort visual2)
                throw new ArgumentException($"{nameof(ConnectVisualCommandFactory)} must be used with visual graph"); 
            
            return new ConnectVisualModulesCommand(visual1,visual2,param3);
        }
    }
}