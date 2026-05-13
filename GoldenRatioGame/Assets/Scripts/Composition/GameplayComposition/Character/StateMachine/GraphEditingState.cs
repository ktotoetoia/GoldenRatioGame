using System;
using IM.Graphs;
using IM.Modules;
using IM.StateMachines;

namespace IM
{
    public class GraphEditingState : State
    {
	    private readonly IModuleEntity _entity;
	    private readonly Action<IModuleEditingContext> _onEditStarted;
	    private readonly Action _onEditEnded;
	    private IModuleEditingContext _context;
	    
	    public GraphEditingState(IModuleEntity entity, Action<IModuleEditingContext> onEditStarted, Action onEditEnded)
	    {
		    _entity = entity;
		    _onEditStarted = onEditStarted;
		    _onEditEnded = onEditEnded;
	    }

	    public override void OnEnter()
	    {
		    _context = _entity.ModuleEditingContextEditor.BeginEdit();
		    
		    _onEditStarted(_context);
	    }

	    public override void OnExit()
	    {
		    _context = null;
		    
		    if(!_entity.ModuleEditingContextEditor.TryApplyChanges()) _entity.ModuleEditingContextEditor.DiscardChanges();
		    
		    _onEditEnded();
	    }
    }
}