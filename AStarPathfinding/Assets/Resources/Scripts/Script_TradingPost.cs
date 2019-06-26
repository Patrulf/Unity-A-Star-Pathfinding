using UnityEngine;

public class Script_TradingPost : Script_IEntity{

	private Script_GameManager _gameManager;
	private Vector3Int _gridPosition;
	private GameObject _meshObject;
	private Vector3Int _startLocation;

	private Material _meshObjectMaterial;

	private string _meshObjectName;
	private Vector3 _meshObjectScale;
	private Color _meshObjectColor;

	public Script_TradingPost(Script_GameManager p_gameManager,Script_Grid p_grid, Vector3Int p_gridPosition)
	{
		_meshObjectName = "ObjectTradingPost";
		_meshObjectColor = new Color (1.0f, 0.2f, 0.0f);
		_meshObjectScale = new Vector3 (0.8f, 0.8f, 0.8f);

		_startLocation = p_gridPosition;
		_gridPosition = p_gridPosition;
		_gameManager = p_gameManager;

		CreateGameObject ();

	}

	public override Vector3Int GetGridPosition ()
	{
		return _gridPosition;
	}

	public override void Destruction()
	{
		_gameManager.DestroyMaterial (_meshObjectMaterial);
		_gameManager.DestroyGameObject (_meshObject);
	}

	public override Vector3Int GetStartLocation()
	{
		return _startLocation;
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



}
