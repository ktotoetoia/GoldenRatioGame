using IM.Modules;
using UnityEngine;

namespace IM.Map
{
    public class ModuleEditingContextRoomVisitorController : MonoBehaviour
    {
        [SerializeField] private GameObject _moduleEditingContextEventsAndRoomVisitorSource;
        private IModuleEditingContextEvents _moduleEditingContext;
        private IRoomVisitor _roomVisitor;

        private void Awake()
        {
            _moduleEditingContext = _moduleEditingContextEventsAndRoomVisitorSource.GetComponent<IModuleEditingContextEvents>();
            _roomVisitor = _moduleEditingContextEventsAndRoomVisitorSource.GetComponent<IRoomVisitor>();
            
            _moduleEditingContext.AddedToContext += OnAdded;
            _moduleEditingContext.RemovedFromContext += OnRemoved;
        }

        private void OnAdded(IExtensibleModule module)
        {
            if (module is not MonoBehaviour moduleMono || !moduleMono.TryGetComponent(out IRoomVisitor roomVisitor)) return;
            
            roomVisitor.CurrentRoom?.Remove(roomVisitor);
        }

        private void OnRemoved(IExtensibleModule module)
        {
            if (module is not MonoBehaviour moduleMono || !moduleMono.TryGetComponent(out IRoomVisitor roomVisitor)) return;
            
            roomVisitor.CurrentRoom?.Remove(roomVisitor);
            _roomVisitor.CurrentRoom?.Add(roomVisitor);
        }
    }
}