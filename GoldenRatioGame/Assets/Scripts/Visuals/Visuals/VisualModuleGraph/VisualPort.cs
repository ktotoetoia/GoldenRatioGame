using UnityEngine;
using IM.Graphs;

namespace IM.ModuleGraph
{
    public class VisualPort : IVisualPort
    {
        public IVisualModule Module { get; }
        IModule IPort.Module => Module;
        public IVisualConnection Connection { get; private set; }
        IConnection IPort.Connection => Connection;
        public bool IsConnected => Connection != null;

        public Vector3 Position
        {
            get => RelativePosition + Module.Position;
            set
            {
            }
        }

        public Vector3 RelativePosition { get; set; }
        public Vector3 Normal { get; set; }

        public VisualPort(IVisualModule module, Vector3 relativePosition,  Vector3 normal)
        {
            Module = module;
            RelativePosition = relativePosition;
            Normal = normal;
        }

        public void Connect(IConnection connection)
        {
            if(connection is not IVisualConnection visualConnection)
                throw new System.Exception("Invalid connection type");
            
            Connection = visualConnection;
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}