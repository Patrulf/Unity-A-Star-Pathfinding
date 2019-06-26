using UnityEngine;
using UnityEngine.Events;

public class Script_UI {

	private Script_Grid _grid;
	private Script_GameManager _manager;

	private enum ButtonFunctionality
	{
		Nothing,
		Grass,
		Dirt,
		Crater,
		SpaceShip,
		StarChaser,
		FallenStar,
		TradingPost,
		BeginSimulation,
		ResetSimulation,
		RemoveEntity
	}

	private ButtonFunctionality _objectToCreate;

	private Script_Button _buttonCreateGrass;
	private Script_Button _buttonCreateDirt;
	private Script_Button _buttonCreateCrater;

	private Script_Button _buttonCreateStarChaser;
	private Script_Button _buttonCreateTradingPost;
	private Script_Button _buttonCreateSpaceShip;
	private Script_Button _buttonCreateFallenStar;

	private Script_Button _buttonRunSimulation;
	private Script_Button _buttonResetSimulation;

	private Script_Button _buttonRemoveEntity;


	private UnityAction _buttonCallback;

	private string _createGrassButtonText = "Grass";
	private string _createDirtButtonText = "Dirt";
	private string _createCraterButtonText = "Crater";

	private string _createTradingPostButtonText = "TradingPost";
	private string _createSpaceShipButtonText = "SpaceShip";
	private string _createFallenStarButtonText = "FallenStar";

	private string _createStarChaserButtonText = "StarChaser";

	private string _runSimulationButtonText = "Run";
	private string _resetSimulationButtonText = "Reset";

	private string _removeEntityButtonText = "Remove entity";

	public Script_UI(Script_Grid p_grid, Script_GameManager p_gameManager)
	{

		_grid = p_grid;
		_manager = p_gameManager;



		_buttonCallback = SetSpaceGrass;
		Vector3Int position = new Vector3Int (0, 0, -1);
		_buttonCreateGrass = new Script_Button (_buttonCallback, position, _createGrassButtonText);
		position = new Vector3Int (3, 0, -1);
		_buttonCallback = SetDirt;
		_buttonCreateDirt = new Script_Button (_buttonCallback, position, _createDirtButtonText);
		position = new Vector3Int (6, 0, -1);
		_buttonCallback = SetCrater;
		_buttonCreateCrater = new Script_Button (_buttonCallback, position, _createCraterButtonText);


		position = new Vector3Int (0, 0, 11);
		_buttonCallback = SetTradingPost;
		_buttonCreateTradingPost = new Script_Button (_buttonCallback, position, _createTradingPostButtonText);
		position = new Vector3Int (3, 0, 11);
		_buttonCallback = SetSpaceShip;
		_buttonCreateSpaceShip = new Script_Button (_buttonCallback, position, _createSpaceShipButtonText);
		position = new Vector3Int (6, 0, 11);
		_buttonCallback = SetFallenStar;
		_buttonCreateFallenStar = new Script_Button (_buttonCallback, position, _createFallenStarButtonText);

		position = new Vector3Int (0, 0, 12);
		_buttonCallback = SetStarChaser;
		_buttonCreateStarChaser = new Script_Button (_buttonCallback, position, _createStarChaserButtonText);

		position = new Vector3Int (3, 0, 12);
		_buttonCallback = RunSimulation;
		_buttonRunSimulation = new Script_Button (_buttonCallback, position, _runSimulationButtonText);

		position = new Vector3Int (6, 0, 12);
		_buttonCallback = ResetSimulation;
		_buttonResetSimulation = new Script_Button (_buttonCallback, position, _resetSimulationButtonText);

		position = new Vector3Int (9, 0, 12);
		_buttonCallback = SetRemoveEntity;
		_buttonRemoveEntity = new Script_Button (_buttonCallback, position, _removeEntityButtonText);


		_objectToCreate = ButtonFunctionality.Nothing; 



	}


	public void ResetSimulation()
	{
		_objectToCreate = ButtonFunctionality.Nothing;
		_manager.ResetSimulation ();
	}

	public void RunSimulation()
	{
		_objectToCreate = ButtonFunctionality.Nothing;
		_manager.RunSimulation ();
	}


	public void SetSpaceGrass()
	{
		_objectToCreate = ButtonFunctionality.Grass;
	}

	void SetDirt()
	{
		_objectToCreate = ButtonFunctionality.Dirt;
	}

	void SetCrater()
	{
		_objectToCreate = ButtonFunctionality.Crater;
	}

	void SetSpaceShip()
	{
		_objectToCreate = ButtonFunctionality.SpaceShip;
	}

	void SetTradingPost()
	{
		_objectToCreate = ButtonFunctionality.TradingPost;
	}

	void SetFallenStar()
	{
		_objectToCreate = ButtonFunctionality.FallenStar;
	}

	void SetStarChaser()
	{
		_objectToCreate = ButtonFunctionality.StarChaser;
	}

	void SetNothingToCreate()
	{
		_objectToCreate = ButtonFunctionality.Nothing;
	}

	void SetRemoveEntity()
	{
		_objectToCreate = ButtonFunctionality.RemoveEntity;
	}


	public void RegisterButtonClicks()
	{
		//Getting buttonhits
		//https://answers.unity.com/questions/332085/how-do-you-make-an-object-respond-to-a-click-in-c.html User Aldonaletto.

		Ray ray;
		RaycastHit hit;

		if (Input.GetButtonDown (Constants.leftButtonClick)) {
			
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject == _buttonCreateGrass.GetButtonObject()) {
					_buttonCreateGrass.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonCreateDirt.GetButtonObject()) {
					_buttonCreateDirt.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonCreateCrater.GetButtonObject()) {
					_buttonCreateCrater.ButtonClick ();
				}



				if (hit.collider.gameObject == _buttonCreateFallenStar.GetButtonObject ()) {
					_buttonCreateFallenStar.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonCreateSpaceShip.GetButtonObject()){
					_buttonCreateSpaceShip.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonCreateTradingPost.GetButtonObject()) {
					_buttonCreateTradingPost.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonCreateStarChaser.GetButtonObject()) {
					_buttonCreateStarChaser.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonRunSimulation.GetButtonObject ()) {
					_buttonRunSimulation.ButtonClick ();
				}
				if (hit.collider.gameObject == _buttonResetSimulation.GetButtonObject ()) {
					_buttonResetSimulation.ButtonClick ();
				}

				if (hit.collider.gameObject == _buttonRemoveEntity.GetButtonObject ()) {
					_buttonRemoveEntity.ButtonClick ();
				}


			}

		}
	}

	public void RegisterGridClick()
	{
			Ray ray;
			RaycastHit hit;

		if (Input.GetButtonDown (Constants.leftButtonClick)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				int x = Mathf.RoundToInt(hit.collider.gameObject.transform.position.x);
				int z = Mathf.RoundToInt(hit.collider.gameObject.transform.position.z);

				if (x < _grid.GetWidth () && x >= 0 && z >= 0 && z < _grid.GetHeight ()) {
					if (_objectToCreate == ButtonFunctionality.Grass) {
						_grid.ReplaceTile<Script_TileGrass> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.Dirt) {
						_grid.ReplaceTile<Script_TileDirt> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.Crater) {
						_grid.ReplaceTile<Script_TileCrater> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.TradingPost) {
						_manager.PlaceEntity<Script_TradingPost> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.SpaceShip) {
						_manager.PlaceEntity<Script_SpaceShip> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.FallenStar) {
						_manager.PlaceEntity<Script_FallenStar> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.StarChaser) {
						_manager.PlaceEntity<Script_StarChaser> (x, z);
					}

					if (_objectToCreate == ButtonFunctionality.RemoveEntity) {
						_manager.RemoveEntity (x, z);
					}


				}


			}

		}

	}







}
