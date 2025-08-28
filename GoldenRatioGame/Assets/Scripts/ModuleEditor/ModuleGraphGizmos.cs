using IM.Graphs;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class ModuleGraphGizmos : MonoBehaviour
    {
        public IModuleGraph ModuleGraph { get; set; }

        private void OnDrawGizmos()
        {
            if(ModuleGraph == null) return;

            foreach (IModule module in ModuleGraph.Modules)
            {
                Gizmos.color = Color.white;

                if (module is IHavePosition p and IHaveSize size)
                {
                    Gizmos.DrawCube(p.Position, size.Size);
                }

                foreach (IModulePort port in module.Ports)
                {
                    if (port.IsConnected)
                    {
                        DrawConnection(port.Connection);
                    }
                    
                    Gizmos.color = Color.red;
                    
                    if (port.Direction is PortDirection.Input)
                    {
                        Gizmos.color = Color.green;
                    }

                    if (port is IHavePosition pp and IHaveSize ss)
                    {
                        Gizmos.DrawSphere(pp.Position,ss.Size.x);
                    }
                }
            }
            
            foreach (IModuleConnection connection in ModuleGraph.Connections)
            {
                Gizmos.color = Color.green;
                DrawConnection(connection);
            }
        }

        private void DrawConnection(IModuleConnection connection)
        {
            if (connection.Input is IHavePosition input && connection.Output is IHavePosition output)
            {
                Gizmos.DrawLine(input.Position, output.Position);
            }
        }
    }
}