using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class Test2 : MonoBehaviour
    {
        [SerializeField] private int _id;
        private GraphTreeGizmosDrawer _gizmosDrawer;
        private ModuleGraph _moduleGraph;

        private void Awake()
        {
            _gizmosDrawer = GetComponent<GraphTreeGizmosDrawer>();
        }

        private void Update()
        {
            _moduleGraph = new ModuleGraph();
            
            switch (_id)
            {
                case 0:
                    BuildStarGraph();
                    break;
                case 1:
                    BuildChainGraph();
                    break;
                case 2:
                    BuildBinaryTreeGraph();
                    break;
                default:
                    BuildStarGraph();
                    break;
            }

            _gizmosDrawer.Graph = _moduleGraph;
        }

        private void BuildStarGraph()
        {
            var root = new Module();
            for (int i = 0; i < 4; i++)
                root.AddPort(new ModulePort(root, PortDirection.Output));

            _moduleGraph.AddModule(root);

            foreach (var port in root.Ports)
            {
                var child = new Module();
                var input = new ModulePort(child, PortDirection.Input);
                child.AddPort(input);
                _moduleGraph.AddModule(child);
                _moduleGraph.Connect(port, input);
            }
        }

        private void BuildChainGraph()
        {
            Module previous = null;

            for (int i = 0; i < 5; i++)
            {
                var module = new Module();
                var input = new ModulePort(module, PortDirection.Input);
                var output = new ModulePort(module, PortDirection.Output);
                module.AddPort(input);
                module.AddPort(output);
                _moduleGraph.AddModule(module);

                if (previous != null)
                    _moduleGraph.Connect(previous.Ports.Last(), input);

                previous = module;
            }
        }

        private void BuildBinaryTreeGraph()
        {
            var root = new Module();
            root.AddPort(new ModulePort(root, PortDirection.Output));
            root.AddPort(new ModulePort(root, PortDirection.Output));
            _moduleGraph.AddModule(root);

            var queue = new Queue<Module>();
            queue.Enqueue(root);

            int depth = 2;
            while (depth-- > 0)
            {
                int count = queue.Count;
                for (int i = 0; i < count; i++)
                {
                    var parent = queue.Dequeue();
                    foreach (var port in parent.Ports.Where(p => p.Direction == PortDirection.Output))
                    {
                        var child = new Module();
                        var input = new ModulePort(child, PortDirection.Input);
                        var output1 = new ModulePort(child, PortDirection.Output);
                        var output2 = new ModulePort(child, PortDirection.Output);
                        child.AddPort(input);
                        child.AddPort(output1);
                        child.AddPort(output2);

                        _moduleGraph.AddModule(child);
                        _moduleGraph.Connect(port, input);
                        queue.Enqueue(child);
                    }
                }
            }
        }
    }
}