using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Node {

	Vector3Int _position;
	Vector3Int _destination;
	Script_Node _parent;

	int _gScore;
	int _hScore;
	int _fScore;

	public Script_Node(Vector3Int p_position,Vector3Int p_destination, Script_Node p_parent)
	{
		_position = p_position;
		_destination = p_destination;
		_parent = p_parent;


		if (p_parent != null) {
			_gScore = p_parent.GetGScore ();
		}

		_gScore = 0;
		_hScore = 0; 
		CalculateHScore();
		_fScore = 0; 
		CalculateFScore();



	}

	public Script_Node GetParent()
	{
		return _parent;
	}

	public void SetParent(Script_Node p_parent)
	{
		_parent = p_parent;
	}

	public void CalculateHScore()
	{
		//source http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html

		int dX = Mathf.Abs (_position.x - _destination.x);
		int dZ = Mathf.Abs (_position.z - _destination.z);


		_hScore = (Constants.linearMovementCost * (dX + dZ)) + (Constants.diagonalMovementCost - (2 * Constants.linearMovementCost)) * Mathf.Min(dX,dZ);

	}

	public int GetHScore()
	{
		return _hScore;
	}

	public Vector3Int GetNodePosition()
	{
		return _position;
	}

	public Vector3Int GetNodeDestination()
	{
		return _destination;
	}


	public int GetGScore()
	{
		return _gScore;
	}

	public int GetFScore()
	{
		return _fScore;
	}

	public void SetGScore(int p_value)
	{
		_gScore = p_value;
	}

	public void CalculateFScore()
	{
		_fScore = _gScore + _hScore;
	}

}
