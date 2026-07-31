using IM.Items;
using IM.Map;
using IM.Visuals;
using UnityEditor;
using UnityEngine;

namespace IM.Modules
{
    public static class ModuleFactory
    {
        [MenuItem("GameObject/Presets/Create Core Module", false, 10)]
        private static void CreateCoreModule(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Core Module");
        
            go.AddComponent<SpriteRendererIconDrawer>();
            go.AddComponent<ModuleSerializer>();
            go.AddComponent<InteractableWhenNoOwner>();
            go.AddComponent<RoomVisitor>();
            go.AddComponent<ExtensibleModuleMono>();
            go.AddComponent<ModuleVisualObjectProvider>();
        
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create Core Module");
            Selection.activeObject = go;
        }
        
        [MenuItem("GameObject/Presets/Create Module", false, 10)]
        private static void CreateModule(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Module");
            
            go.AddComponent<SpriteRendererIconDrawer>();
            go.AddComponent<ModuleSerializer>();
            go.AddComponent<InteractableWhenNoOwner>();
            go.AddComponent<RoomVisitor>();
            go.AddComponent<ExtensibleModuleMono>();
            go.AddComponent<ModuleVisualObjectProvider>();
        
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create Module");
            Selection.activeObject = go;
        }
    }
}