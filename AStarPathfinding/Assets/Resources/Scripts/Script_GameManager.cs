using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Script_GameManager : MonoBehaviour {

	private Vector3 cameraForward;
	private Vector3 cameraUp;
	private Vector3 cameraOffset;

	private float tileSize = 1.0f;
	private int width = 10;
	private int height = 10;

	private float cameraXPosition;
	private float cameraZPosition;
	private float cameraYPosition;

	private Script_Grid _grid;

	private Script_StarChaser _chaser;
	private Script_TradingPost _tradingPost;
	private Script_FallenStar _fallenStar;
	private Script_SpaceShip _spaceShip;

	private Script_UI _ui;

	private bool _isRunningSimulation;

	private List<Script_IEntity> _entityList;

	void Start () {

		_isRunningSimulation = false;
		_entityList = new List<Script_IEntity> ();

		cameraXPosition =  width * 0.5f - tileSize * 0.5f;
		cameraZPosition = height * 0.5f - tileSize * 0.5f;
		cameraYPosition = 15.0f;

		cameraForward = new Vector3 (0, -1, 0);
		cameraUp = new Vector3 (0, 0, 1);

		Camera.main.transform.position = new Vector3 (cameraXPosition, cameraYPosition, cameraZPosition);
		Camera.main.transform.rotation = Quaternion.LookRotation (cameraForward, cameraUp);

		_grid = new Script_Grid (this);
		_grid.InstantiateGrid (10, 10, 0);

		InstantiateEntityAtRandomLocation<Script_TradingPost> ();
		InstantiateEntityAtRandomLocation<Script_FallenStar> ();
		InstantiateEntityAtRandomLocation<Script_SpaceShip> ();
		InstantiateEntityAtRandomLocation<Script_StarChaser>();

		_ui = new Script_UI (_grid, this);

	}

	void Update () {
		if (_isRunningSimulation && _chaser != null) {
			_chaser.Update ();
		}
		_ui.RegisterButtonClicks ();
		_ui.RegisterGridClick ();
	}

	public GameObject InstantiateObject(GameObject p_object, Vector3Int p_position)
	{
		return Instantiate (p_object, p_position, transform.rotation);
	}

	public void DestroyGameObject(GameObject p_object)
	{
		Destroy (p_object);
	}

	public void DestroyMaterial(Material p_material)
	{
		Destroy (p_material);
	}


	void InstantiateEntityAtRandomLocation<T> () where T : Script_IEntity
	{

	//While this does not have a 100% chance of success, seeing as there is a very small chance of not having any walkable tiles,
	//and we could also be randoming at positions that aren't walkable until we've tried 64 times, i deem it enough.
	//if this fails, we will have to place them on our own.
	int randomWalkablePositionX = Random.Range (0, width);
	int randomWalkablePositionZ = Random.Range (0, height);
	int attemptsToSpawn = 64;
	int currentAttempts = 0;

	while (_grid.AccessGridTile (randomWalkablePositionX, randomWalkablePositionZ).GetWalkable () == false || currentAttempts < attemptsToSpawn) {
		randomWalkablePositionX = Random.Range (0, width);
		randomWalkablePositionZ = Random.Range (0, height);
		currentAttempts++;
	}

	if (_grid.AccessGridTile (randomWalkablePositionX, randomWalkablePositionZ).GetWalkable () == true) {
			Vector3Int spawnLocation = new Vector3Int (randomWalkablePositionX, 0, randomWalkablePositionZ);

			if (typeof(T) == typeof(Script_StarChaser)) {
				_chaser = new Script_StarChaser (this, _grid, spawnLocation);
				_entityList.Add (_chaser as Script_IEntity);
			}
			if (typeof(T) == typeof(Script_TradingPost)) {
				_tradingPost = new Script_TradingPost(this, _grid, spawnLocation);
				_entityList.Add (_tradingPost as Script_IEntity);
			}
			if (typeof(T) == typeof(Script_FallenStar)) {
				_fallenStar = new Script_FallenStar (this, _grid, spawnLocation);
				_entityList.Add (_fallenStar as Script_IEntity);
			}
			if (typeof(T) == typeof(Script_SpaceShip)) {
				_spaceShip = new Script_SpaceShip (this, _grid, spawnLocation);
				_entityList.Add (_spaceShip as Script_IEntity);
			}	
		}
	}
		
	public Vector3Int GetEntityLocation<T>() where T : Script_IEntity{

		if (typeof(T) == typeof(Script_StarChaser)) {
			return _chaser.GetGridPosition();
		}
		if (typeof(T) == typeof(Script_TradingPost)) {
			return _tradingPost.GetGridPosition();
		}
		if (typeof(T) == typeof(Script_FallenStar)) {
			return _fallenStar.GetGridPosition();
		}
		if (typeof(T) == typeof(Script_SpaceShip)) {
			return _spaceShip.GetGridPosition ();
		}	

		return Vector3Int.zero;

	}

	public bool GetRunningSimulation()
	{
		return _isRunningSimulation;
	}

	public void SetRunningSimulation(bool p_value)
	{
		_isRunningSimulation = p_value;
	}


	public void RunSimulation()
	{
		_isRunningSimulation = true;
	}

	public void ResetSimulation()
	{
		_isRunningSimulation = false;
		if (_tradingPost != null) {
			int x = _tradingPost.GetStartLocation ().x;
			int z = _tradingPost.GetStartLocation ().z;
			PlaceEntity<Script_TradingPost> (x,z);
		}
			
		if (_fallenStar != null) {
			int x = _fallenStar.GetStartLocation ().x;
			int z = _fallenStar.GetStartLocation ().z;
			PlaceEntity<Script_FallenStar> (x,z);
		}
			
		if (_spaceShip != null) {
			int x = _spaceShip.GetStartLocation ().x;
			int z = _spaceShip.GetStartLocation ().z;
			PlaceEntity<Script_SpaceShip> (x,z);
		}

		if (_chaser != null) {
			int x = _chaser.GetStartLocation ().x;
			int z = _chaser.GetStartLocation ().z;
			PlaceEntity<Script_StarChaser> (x,z);
		}


	}


	public Script_IEntity GetEntityOfType<T>() where T : Script_IEntity
	{
		foreach (Script_IEntity entity in _entityList) {
			if (entity is T) {
				return entity;
			}
		}


		return null;
	}


	public void PlaceEntity<T> (int p_x, int p_z) where T : Script_IEntity
	{
		if (typeof(T) == typeof(Script_TradingPost)) {
			bool alreadyExists = false;
			foreach (Script_IEntity entity in _entityList.ToList()) {
				if (entity is Script_TradingPost) {
					entity.Destruction();
					_entityList.Remove (entity);
					Vector3Int position = new Vector3Int (p_x, 0, p_z);
					_tradingPost = new Script_TradingPost (this, _grid, position);
					_entityList.Add (_tradingPost);
					alreadyExists = true;
					break;
				}					
			}	
			if (!alreadyExists) {
				Vector3Int position = new Vector3Int (p_x, 0, p_z);
				_tradingPost = new Script_TradingPost (this, _grid, position);
				_entityList.Add (_tradingPost);
			}


		}

		if (typeof(T) == typeof(Script_StarChaser)) {
			bool alreadyExists = false;
			foreach (Script_IEntity entity in _entityList.ToList()) {
				if (entity is Script_StarChaser) {
					entity.Destruction();
					_entityList.Remove (entity);
					Vector3Int position = new Vector3Int (p_x, 0, p_z);
					_chaser = new Script_StarChaser (this, _grid, position);
					_entityList.Add (_chaser);
					alreadyExists = true;
					break;
				}					
			}	
			if (!alreadyExists) {
				Vector3Int position = new Vector3Int (p_x, 0, p_z);
				_chaser = new Script_StarChaser (this, _grid, position);
				_entityList.Add (_chaser);
			}
		}

		if (typeof(T) == typeof(Script_FallenStar)) {
			bool alreadyExists = false;
			foreach (Script_IEntity entity in _entityList.ToList()) {
				if (entity is Script_FallenStar) {
					entity.Destruction();
					_entityList.Remove (entity);
					Vector3Int position = new Vector3Int (p_x, 0, p_z);
					_fallenStar = new Script_FallenStar (this, _grid, position);
					_entityList.Add (_fallenStar);
					alreadyExists = true;
					break;
				}					
			}	
			if (!alreadyExists) {
				Vector3Int position = new Vector3Int (p_x, 0, p_z);
				_fallenStar = new Script_FallenStar (this, _grid, position);
				_entityList.Add (_fallenStar);
			}
		}

		if (typeof(T) == typeof(Script_SpaceShip)) {
			bool alreadyExists = false;
			foreach (Script_IEntity entity in _entityList.ToList()) {
				if (entity is Script_SpaceShip) {
					entity.Destruction();
					_entityList.Remove (entity);
					Vector3Int position = new Vector3Int (p_x, 0, p_z);
					_spaceShip = new Script_SpaceShip (this, _grid, position);
					_entityList.Add (_spaceShip);
					alreadyExists = true;
					break;
				}					
			}	
			if (!alreadyExists) {
				Vector3Int position = new Vector3Int (p_x, 0, p_z);
				_spaceShip = new Script_SpaceShip (this, _grid, position);
				_entityList.Add (_spaceShip);
			}
		}

	}

	public void RemoveEntity(int p_x, int p_z)
	{
			foreach (Script_IEntity entity in _entityList.ToList())
			{
			if (entity is Script_TradingPost) {
				int x = entity.GetGridPosition ().x;
				int z = entity.GetGridPosition ().z;

				if (x == p_x && z == p_z) {
					_tradingPost = null;
					_entityList.Remove (entity);
					entity.Destruction();
				}
			}

			if (entity is Script_StarChaser) {
				int x = entity.GetGridPosition ().x;
				int z = entity.GetGridPosition ().z;

				if (x == p_x && z == p_z) {
					_chaser = null;
					_entityList.Remove (entity);
					entity.Destruction ();
				}
			}


			if (entity is Script_SpaceShip) {
				int x = entity.GetGridPosition ().x;
				int z = entity.GetGridPosition ().z;

				if (x == p_x && z == p_z) {
					_spaceShip = null;
					_entityList.Remove (entity);
					entity.Destruction ();
				}
			}


			if (entity is Script_FallenStar) {
				int x = entity.GetGridPosition ().x;
				int z = entity.GetGridPosition ().z;

				if (x == p_x && z == p_z) {
					_fallenStar = null;
					_entityList.Remove (entity);
					entity.Destruction ();
				}
			}				

		}
	}		

}
