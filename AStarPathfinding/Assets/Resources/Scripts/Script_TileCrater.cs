using UnityEngine;

public class Script_TileCrater : Script_ITile {

	private Script_GameManager _gameManager;
	private Vector3Int _position;

	private bool _isWalkable;
	private GameObject _meshObject;

	private Material _meshObjectMaterial;

	private string _meshObjectName;
	private Vector3 _meshObjectScale;
	private Color _meshObjectColor;

	public Script_TileCrater(Script_GameManager p_gameManager,Script_Grid p_grid, Vector3Int p_position)
	{
		_meshObjectName = "ObjectCrater";
		_meshObjectColor = new Color (0.0f, 0.0f, 0.0f);
		_meshObjectScale = new Vector3 (0.1f, 0.1f, 0.1f);

		_isWalkable = false;
		_position = p_position;
		_gameManager = p_gameManager;

		CreateGameObject ();
	}


	public override bool GetWalkable()
	{
		return _isWalkable;
	}

	public override void DestroyGameObject()
	{
		_gameManager.DestroyMaterial (_meshObjectMaterial);
		_gameManager.DestroyGameObject(_meshObject);
	}

	private void CreateGameObject()
	{
		_meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		_meshObject.name = _meshObjectName;
		_meshObject.transform.position = _position;
		_meshObject.transform.localScale = _meshObjectScale;
		_meshObjectMaterial = _meshObject.GetComponent<Renderer>().material;
		_meshObjectMaterial.color = _meshObjectColor;
	}

}
