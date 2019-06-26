using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
//Based on the statemachine in the Augustus engine by Jerry Jonsson.

public class Script_Statemachine {

	private Script_IState _currentState;
	private Dictionary<int,Script_IState> _states;
	public Script_Statemachine()
	{
		_currentState = null;
		_states = new Dictionary<int,Script_IState> ();
	}
		
	public void Update () {
		Assert.IsNotNull (_currentState);
		_currentState.Update ();
	}
		
	public void SetState(Enum p_enum)
	{
		int key = Convert.ToInt32 (p_enum);
		if (_states [key] != _currentState) {
			if (_states.ContainsKey (key)) {
				if (_currentState != null)
					_currentState.Exit ();
				_currentState = _states [key];
				_currentState.Enter ();
			}
		}
	}
		
	public void AddState(Enum p_enum, Script_IState p_state)
	{
		int key = Convert.ToInt32 (p_enum);
		if (!_states.ContainsKey (key)) {
			_states.Add (key, p_state);
		}
	}
		
	public void Destruction()
	{		
		_currentState.Exit ();
		_states.Clear ();
		_states = null;
	}

}
