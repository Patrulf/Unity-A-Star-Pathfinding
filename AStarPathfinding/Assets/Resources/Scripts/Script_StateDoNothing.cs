using UnityEngine;
using Namespace_EntityStateIDs;

public class Script_StateDoNothing : Script_IState{

	private Script_StarChaser _creator;
	private Script_GameManager _manager;
	private StateIDs _nextState;

	public Script_StateDoNothing(Script_GameManager p_manager, Script_StarChaser p_creator)
	{

		_creator = p_creator;
		_manager = p_manager;
		_nextState = StateIDs.DOINGNOTHING;
	}

	public void SetNextState(StateIDs p_nextState)
	{
		_nextState = p_nextState;
	}

	public override void Enter(){
		_manager.SetRunningSimulation (false);
	}

	public override void Exit(){
	}

	public override void Update(){
		DetermineStateChange ();
	}

	void DetermineStateChange()
	{
		if (_manager.GetRunningSimulation () ) {
			_creator.ChangeState(_nextState);
		} 
	}

}
