using UnityEngine;

public class Script_Grid {

	private Script_GameManager _gameManager;

	Script_ITile[] _grid = new Script_ITile[0];

	private int _width = 0;
	private int _height = 0;

	public Script_Grid(Script_GameManager p_creator)
	{
		_gameManager = p_creator;

	}		

	public Script_ITile AccessGridTile(int p_x, int p_z)
	{
		return _grid[p_z * _width + p_x];
	}

	private void SetGridTile(int p_x, int p_z, Script_ITile p_tile)
	{
		_grid [p_z * _width + p_x] = p_tile;
	}




	public void InstantiateGrid (int p_width, int p_height, int p_yOffset )
	{
		_width = p_width;
		_height = p_height;

		System.Array.Resize (ref _grid, p_width * p_height);

		for (int z = 0; z < _height; z++) {
			for (int x = 0; x < _width; x++) {

				Vector3Int tilePosition = new Vector3Int (x, 0, z);

				int randomNumber = Random.Range (0, 3);

				int numberToSpawnGrass = 0;
				int numberToSpawnCrater = 1;
				int numberToSpawnDirt = 2;

				if (randomNumber == numberToSpawnGrass) {
					Script_TileGrass myTile = new Script_TileGrass (_gameManager, this, tilePosition);
					SetGridTile (x, z, myTile);

				} else if (randomNumber == numberToSpawnCrater) {
					Script_TileCrater myTile = new Script_TileCrater (_gameManager, this, tilePosition);
					SetGridTile (x, z, myTile);
				} else if (randomNumber == numberToSpawnDirt) {
					Script_TileDirt myTile = new Script_TileDirt (_gameManager, this, tilePosition);
					SetGridTile (x, z, myTile);
				}


			}
		}			
	}

	public void ReplaceTile<T> (int p_x, int p_z) where T : Script_ITile
	{

		if (typeof(T) == typeof(Script_TileGrass)) {
			
			Script_ITile tile = AccessGridTile (p_x, p_z);
			tile.DestroyGameObject ();

			Vector3Int newPos = new Vector3Int(p_x,0,p_z);
			Script_TileGrass newTile = new Script_TileGrass (_gameManager, this, newPos);
			SetGridTile(p_x,p_z,newTile);

		}

		if (typeof(T) == typeof(Script_TileDirt)) {

			Script_ITile tile = AccessGridTile (p_x, p_z);
			tile.DestroyGameObject ();

			Vector3Int newPos = new Vector3Int(p_x,0,p_z);
			Script_TileDirt newTile = new Script_TileDirt (_gameManager, this, newPos);
			SetGridTile(p_x,p_z,newTile);

		}

		if (typeof(T) == typeof(Script_TileCrater)) {

			Script_ITile tile = AccessGridTile (p_x, p_z);
			tile.DestroyGameObject ();

			Vector3Int newPos = new Vector3Int(p_x,0,p_z);
			Script_TileCrater newTile = new Script_TileCrater (_gameManager, this, newPos);
			SetGridTile(p_x,p_z,newTile);

		}			

	}

	bool GetOccupied(int p_x, int p_z)
	{
		return true;
	}

	public int GetWidth()
	{
		return _width;
	}

	public int GetHeight()
	{
		return _height;
	}


}
