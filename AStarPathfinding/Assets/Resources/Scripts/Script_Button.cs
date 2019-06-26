using UnityEngine;
using UnityEngine.Events;

public class Script_Button {

	private UnityAction _callback;

	private int _width;
	private int _height;

	Vector3Int _position;

	private TextMesh _text;
	private Transform _textTransform;

	private GameObject _buttonObject;
	private GameObject _textObject;
	private SpriteRenderer _renderer;

	private string _meshObjectName;
	private Vector3 _meshObjectScale;

	public Script_Button(UnityAction p_callback, Vector3Int p_position, string p_text)
	{
		_callback = p_callback;
		_position = p_position;

		_meshObjectName = "ObjectButton";
		_meshObjectScale = new Vector3 (1.0f, 1.0f, 1.0f);

		CreateGameObject ();


		_textObject = new GameObject ();
		_textObject.transform.position = new Vector3 (_position.x, _position.y+1.0f, _position.z);
		_textObject.transform.Rotate(90,0,0);
		_text = _buttonObject.GetComponentInChildren<TextMesh> ();
		_text = _textObject.AddComponent<TextMesh> ();

		_textTransform = _textObject.transform;
		_text.text = p_text;
		_text.color = Color.black;
		_text.characterSize = 0.3f;
		_text.lineSpacing = 1;
		_text.offsetZ = 0.0f;
		_textTransform.position = new Vector3 (_position.x, _position.y+1.0f, _position.z);
		_text.anchor = TextAnchor.UpperCenter;
		_text.alignment = TextAlignment.Center;
		_text.tabSize = 4;
		_text.fontSize = 0;
		_text.fontStyle = FontStyle.Normal;
		_text.richText = true;
		_text.font = null;


	}

	public GameObject GetButtonObject()
	{
		return _buttonObject;
	}


	public void ButtonClick()
	{
		_callback();

	}

	private void CreateGameObject()
	{
		_buttonObject =  new GameObject();
		_buttonObject.name = _meshObjectName;
		_buttonObject.transform.position = _position;
		_buttonObject.transform.localScale = _meshObjectScale;
		_renderer = _buttonObject.AddComponent<SpriteRenderer>();
		_renderer.sprite = Resources.Load<Sprite> (Constants.ButtonSpriteName);
		_buttonObject.transform.Rotate(90,0,0);
		_buttonObject.AddComponent<BoxCollider> ();
	}



}
