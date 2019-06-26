using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Namespace_EntityStateIDs;

public class Script_StarChaser : Script_IEntity {

	private Script_Statemachine _stateManager;
	private Script_GameManager _manager;
	private Script_Grid _grid;

	private Script_StateChasingStar _chasingState;
	private Script_StateRest _restingState;
	private Script_StateSellStar _sellingState;
	private Script_StateDoNothing _doNothingState;


	private Vector3Int _gridPosition;
	private Vector3 _position;

	private GameObject _meshObject;

	private List<Script_Node> _pathList; 
	private Script_Node _targetNode;
	private Script_IEntity p_target;

	private float _speed;

	private float _nodeCollisionRange;

	private Vector3Int _startLocation;

	private Material _meshObjectMaterial;

	private string _meshObjectName;
	private Vector3 _meshObjectScale;
	private Color _meshObjectColor;

	private StateIDs _nextState;


	public Script_StarChaser(Script_GameManager p_manager, Script_Grid p_grid, Vector3Int p_position)
	{
		_meshObjectName = "ObjectStarChaser";
		_meshObjectColor = new Color (1.0f, 1.0f, 1.0f);
		_meshObjectScale = new Vector3 (1.0f, 1.0f, 1.0f);


		_nodeCollisionRange = 0.1f;

		_targetNode = null;
		_pathList = new List<Script_Node> ();
		_gridPosition = p_position;
		_manager = p_manager;
		_grid = p_grid;

		_speed = 1.0f;

		_position = (Vector3)p_position;
		_startLocation = p_position;


		_chasingState = new Script_StateChasingStar(_grid,_manager,this);
		_sellingState = new Script_StateSellStar (_grid, _manager, this);
		_restingState = new Script_StateRest (_grid, _manager, this);
		_doNothingState = new Script_StateDoNothing (_manager, this);

		_stateManager = new Script_Statemachine();

		_doNothingState.SetNextState (StateIDs.CHASINGFALLENSTAR);
		_chasingState.SetNextState (StateIDs.SELLINGFALLENSTAR);
		_sellingState.SetNextState(StateIDs.RESTING);
		_restingState.SetNextState (StateIDs.DOINGNOTHING);

		_stateManager.AddState (StateIDs.CHASINGFALLENSTAR, _chasingState);
		_stateManager.AddState (StateIDs.SELLINGFALLENSTAR, _sellingState);
		_stateManager.AddState (StateIDs.RESTING, _restingState);
		_stateManager.AddState (StateIDs.DOINGNOTHING, _doNothingState);
		_stateManager.SetState (StateIDs.DOINGNOTHING);
		_nextState = StateIDs.DOINGNOTHING;

		CreateGameObject ();

	}

	private void CreateGameObject()
	{
		_meshObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_meshObject.name = _meshObjectName;
		_meshObject.transform.position = _gridPosition;
		_meshObject.transform.localScale = _meshObjectScale;
		_meshObjectMaterial = _meshObject.GetComponent<Renderer>().material;
		_meshObjectMaterial.color = _meshObjectColor;
	}




	public void Update()
	{
		_stateManager.Update();
		UpdatePosition ();
		_stateManager.SetState (_nextState);
	}

	public void SetPath(List<Script_Node> p_pathList)
	{
		_pathList = p_pathList;
	}

	public void SetPathBeginning(Script_Node p_path)
	{
		_targetNode = p_path;
	}

	public void WalkAlongPath()
	{
			Vector3 targetPosition = (Vector3)_targetNode.GetNodePosition ();

			if (Vector3.Distance (_position, targetPosition) < _nodeCollisionRange) {			
				if (_pathList.IndexOf (_targetNode) != _pathList.Count - 1) {
					_targetNode = _pathList [_pathList.IndexOf (_targetNode) + 1];
				} else {
					_meshObject.transform.position = targetPosition;
				}
			
			} else {
				Vector3 nodePosition = _targetNode.GetNodePosition ();
				_meshObject.transform.position += (nodePosition - _meshObject.transform.position).normalized * _speed * Time.deltaTime;
			}
	}

	public float GetNodeCollisionRange()
	{
		return _nodeCollisionRange;
	}


	private void UpdatePosition()
	{
		int x = Mathf.RoundToInt(_meshObject.transform.position.x);
		int y = Mathf.RoundToInt(_meshObject.transform.position.y);
		int z = Mathf.RoundToInt(_meshObject.transform.position.z);

		_position = _meshObject.transform.position;
		_gridPosition = new Vector3Int (x, y, z);
	}


	public void ChangeState(StateIDs p_stateID)
	{
		_nextState = p_stateID;
	}


	public Vector3 GetPosition()
	{
		return _position;
	}

	public override Vector3Int GetGridPosition()
	{
		return _gridPosition;
	}


	public override void Destruction()
	{
		_manager.DestroyMaterial (_meshObjectMaterial);
		_manager.DestroyGameObject (_meshObject);
		_stateManager.Destruction ();
	}


	public override Vector3Int GetStartLocation()
	{
		return _startLocation;
	}

}
