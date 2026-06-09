using System;
using IM.Modules;
using IM.StateMachines;

namespace IM
{
    public class GraphEditingState : State
    {
	    private readonly IModuleEntity _entity;
	    private readonly Action<IModuleEditingContext> _onEditStarted;
	    private readonly Action _onEditEnded;
	    private readonly Func<IModuleEditingContextReadOnly, bool> _shouldTrySaveChanges;
	    private IModuleEditingContext _context;
	    
	    public GraphEditingState(IModuleEntity entity, Action<IModuleEditingContext> onEditStarted, Action onEditEnded, Func<IModuleEditingContextReadOnly,bool> shouldTrySaveChanges)
	    {
		    _entity = entity;
		    _onEditStarted = onEditStarted;
		    _onEditEnded = onEditEnded;
		    _shouldTrySaveChanges = shouldTrySaveChanges;
	    }

	    public override void OnEnter()
	    {
		    _context = _entity.ModuleEditingContextEditor.BeginEdit();
		    
		    _onEditStarted(_context);
	    }

	    public override void OnExit()
	    {
		    if (!_shouldTrySaveChanges(_context) || !_entity.ModuleEditingContextEditor.TryApplyChanges() )
		    {
			    _entity.ModuleEditingContextEditor.DiscardChanges();
		    }
		    
		    _context = null;
		    
		    _onEditEnded();
	    }
    }
}