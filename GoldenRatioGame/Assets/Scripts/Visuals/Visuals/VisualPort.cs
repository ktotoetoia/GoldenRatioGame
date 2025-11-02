using UnityEngine;
using IM.Graphs;

namespace IM.ModuleGraph
{
    public class VisualPort : IVisualPort
    {
        public IVisualModule VisualModule { get; }

        public IModule Module => VisualModule;
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;

        public Vector3 Position
        {
            get => RelativePosition + VisualModule.Position;
            set
            {
            }
        }

        public Vector3 RelativePosition { get; set; }
        public Vector3 Normal { get; set; }

        public VisualPort(IVisualModule module, Vector3 relativePosition,  Vector3 normal)
        {
            VisualModule = module;
            RelativePosition = relativePosition;
            Normal = normal;
        }

        public void Connect(IConnection connection)
        {
            Connection = connection;
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}