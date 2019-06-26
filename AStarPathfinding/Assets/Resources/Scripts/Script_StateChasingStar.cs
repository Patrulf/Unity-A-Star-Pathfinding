using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Namespace_EntityStateIDs;

public class Script_StateChasingStar : Script_IState {

	private Script_GameManager _manager;
	private Script_Grid _grid;

	private Vector3Int _gridPosition;

	private Script_AStar _pathfinding;

	private List<Script_Node> _pathList; 

	private Script_StarChaser _creator;

	private List<GameObject> _pathObjects;
	private List<Material> _pathObjectsMaterials;

	private Color _pathObjectColor;
	private string _pathObjectString;
	private Vector3 _pathObjectScale;

	private StateIDs _nextState;

	public Script_StateChasingStar(Script_Grid p_grid, Script_GameManager p_manager, Script_StarChaser p_creator)
	{
		_pathObjectScale = new Vector3 (0.1f, 0.1f, 0.1f);
		_pathObjectColor = Color.yellow;
		_pathObjectString = "PathObject";

		_manager = p_manager;
		_pathObjects = new List<GameObject> ();
		_pathObjectsMaterials = new List<Material> ();

		_pathfinding = new Script_AStar(p_grid);
		_gridPosition = p_creator.GetGridPosition ();


		_pathList = new List<Script_Node> ();

		_grid = p_grid;

		_pathfinding = new Script_AStar (_grid);
		_creator = p_creator;

		_nextState = StateIDs.DOINGNOTHING;

	}

	public override void Enter()
	{
		_gridPosition = _creator.GetGridPosition ();
		_pathList = new List<Script_Node> ();

		if (_manager.GetEntityOfType<Script_FallenStar> () != null) {
			FindPath (_gridPosition, _manager.GetEntityLocation<Script_FallenStar> ());
			_creator.SetPath (_pathList);

			if (FoundPath ()) {
				_creator.SetPathBeginning (_pathList [0]);
			}
		}
	}

	public void SetNextState(StateIDs p_nextState)
	{		
		_nextState = p_nextState;
	}


	private void DetermineStateChange()
	{

		if (_pathList != null && _pathList.Count > 0) {

			Vector3 endOfPathNodePosition = _pathList [_pathList.Count - 1].GetNodePosition ();

			if (Vector3.Distance (_creator.GetPosition(), endOfPathNodePosition) < _creator.GetNodeCollisionRange() ) {		

				if (_manager.GetEntityOfType<Script_FallenStar> () != null) {
					if (_manager.GetEntityLocation<Script_FallenStar> () == _gridPosition) {						
						_creator.ChangeState (_nextState);
					}
				}
			}
		}
	}




	private bool FoundPath()
	{
		if (_pathList != null && _pathList.Count > 0) {
			return true;
		}
		return false;
	}

	public override void Exit()
	{
		if (_pathObjects.Count > 0) {
			foreach (GameObject pathObject in _pathObjects.ToList() ) {
				_manager.DestroyGameObject (pathObject);
				_pathObjects.Remove (pathObject);
			
			}
		}
		if (_pathObjectsMaterials.Count > 0) {
			foreach (Material pathObjectMaterial in _pathObjectsMaterials.ToList() ) {
				_manager.DestroyMaterial (pathObjectMaterial);
				_pathObjectsMaterials.Remove (pathObjectMaterial);
			}
		}	
	}

	public override void Update()
	{
		if (FoundPath ()) {
			_creator.WalkAlongPath ();
			_gridPosition = _creator.GetGridPosition ();
			DetermineStateChange ();
		}
	}


	public void FindPath(Vector3Int p_startPos, Vector3Int p_endPos)
	{
		Script_Node start = new Script_Node (p_startPos,p_endPos,null);
		Script_Node _destination = _pathfinding.CalculatePath (start);

		if (_destination != null) {
			_pathList = _pathfinding.GetLocationOfPath(_destination);

			if (FoundPath ()) {
				foreach (Script_Node node in _pathList) {
					GameObject pathObject = CreatePathObject (node.GetNodePosition ());
					_pathObjects.Add (pathObject);

				}
			}

		}

	}

	public GameObject CreatePathObject(Vector3Int p_position)
	{
		GameObject pathObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pathObject.name = _pathObjectString;
		pathObject.transform.position = p_position;
		pathObject.transform.localScale = _pathObjectScale;
		Material material = pathObject.GetComponent<Renderer>().material;
		material.color = _pathObjectColor;
		AddPathObjectMaterial (material);
		return pathObject;
	}

	public void AddPathObjectMaterial(Material p_material)
	{
		_pathObjectsMaterials.Add (p_material);
	}

}
