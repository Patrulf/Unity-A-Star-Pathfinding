using System.Collections.Generic;
using UnityEngine;

public class Script_AStar {
	
	private Dictionary<int,Script_Node> _openDictionary;
	private Dictionary<int,Script_Node> _closedDictionary;

	private Script_Grid _grid;

	public Script_AStar(Script_Grid p_grid)
	{
		_openDictionary = new Dictionary<int,Script_Node> ();
		_closedDictionary = new Dictionary<int,Script_Node> ();

		_grid = p_grid;
	}


	public Dictionary<int,Script_Node> GetOpenList()
	{
		return _openDictionary;
	}

	public Dictionary<int,Script_Node> GetClosedList()
	{
		return _closedDictionary;
	}		

	public Script_Node GetOpenListElement(int p_x, int p_z)
	{
		return _openDictionary[p_z * _grid.GetWidth() + p_x];
	}

	public Script_Node GetClosedListElement(int p_x, int p_z)
	{
		return _closedDictionary [p_z * _grid.GetWidth () + p_x];
	}

	private void AddOpenListElementAtPosition(int p_x, int p_z, Script_Node p_elementToAdd)
	{
		_openDictionary.Add (p_z * _grid.GetWidth() + p_x, p_elementToAdd);
	}

	private void AddClosedListElementAtPosition(int p_x, int p_z, Script_Node p_elementToAdd)
	{
		_closedDictionary.Add (p_z * _grid.GetWidth() + p_x, p_elementToAdd);
	}

	private void RemoveClosedListElementAtPosition(int p_x, int p_z)
	{
		_closedDictionary.Remove (p_z * _grid.GetWidth () + p_x);
	}

	private void RemoveOpenListElementAtPosition(int p_x, int p_z)
	{
		_openDictionary.Remove (p_z * _grid.GetWidth () + p_x);
	}



	public Script_Node GetLowestFScoreOpenDictionary()
	{
		int fScore = int.MaxValue;
		Script_Node lowestFScoreNode = null;

		foreach (Script_Node node in _openDictionary.Values) {

			if (fScore > node.GetFScore ()) {
				lowestFScoreNode = node;
				fScore = node.GetFScore ();
			}

		}
		if (lowestFScoreNode != null) {
			return lowestFScoreNode;
		}


		return null;

	}
		


	private void CalculateNeighbours(Script_Node p_node)
	{

		int xCurrent = p_node.GetNodePosition().x;
		int zCurrent = p_node.GetNodePosition().z;

		int xMin = xCurrent;
		int xMax = xCurrent;
		int zMin = zCurrent;
		int zMax = zCurrent;

		for (int x = xCurrent - 1; x <= xCurrent; x++) {
			if (x < 0)
				continue;

			xMin = x;
			break;
		}
		for (int x = xCurrent + 1; x >= xCurrent; x--) {
			if (x >= _grid.GetWidth ())
				continue;

			xMax = x;
			break;
		}
		for (int z = zCurrent - 1; z <= zCurrent; z++) {
			if (z < 0)
				continue;

			zMin = z;
			break;
		}
		for (int z = zCurrent + 1; z >= zCurrent; z--) {
			if (z >= _grid.GetHeight ())
				continue;

			zMax = z;
			break;
		}

		for (int z = zMin; z <= zMax; z++) {
			for (int x = xMin; x <= xMax; x++) {
				if (_grid.AccessGridTile (x, z).GetWalkable() == true && !_closedDictionary.ContainsKey(z*_grid.GetWidth() + x)) {

					if ((x != xCurrent && z != zCurrent)) {
						if (_grid.AccessGridTile (x, z +(zCurrent - z)) != null && _grid.AccessGridTile (x, z +(zCurrent - z)).GetWalkable () == false)
							continue;

						if (_grid.AccessGridTile (x + (xCurrent - x), z) != null && _grid.AccessGridTile (x + (xCurrent - x), z).GetWalkable () == false)
							continue;
					}

					int tempGScore = 0;
					if (x == xCurrent || z == zCurrent)
						tempGScore = Constants.linearMovementCost; 
					else
						tempGScore = Constants.diagonalMovementCost;

					if (_openDictionary.ContainsKey(z*_grid.GetWidth() + x)) {

						Script_Node node = GetOpenListElement (x, z);

						if (node.GetParent () != null) {
							tempGScore += p_node.GetGScore ();
						}

						if (tempGScore < node.GetGScore () ) {
							node.SetGScore (tempGScore);
							node.CalculateFScore ();
							node.SetParent (p_node);
						}
					}


					if (!_openDictionary.ContainsKey(z*_grid.GetWidth() + x)) {

						Vector3Int location = new Vector3Int(x,0,z);

						Script_Node newNode = new Script_Node(location,p_node.GetNodeDestination(),p_node);
						tempGScore += newNode.GetParent ().GetGScore (); 

						newNode.SetGScore(tempGScore);
						newNode.CalculateHScore ();
						newNode.CalculateFScore ();
						AddOpenListElementAtPosition(x,z,newNode);
					}

				}

			}
		}


	}


	public Script_Node CalculatePath(Script_Node p_initialNode)
	{
		//Based on pseudoCode found here: https://www.raywenderlich.com/3016-introduction-to-a-pathfinding
		AddOpenListElementAtPosition(p_initialNode.GetNodePosition().x,p_initialNode.GetNodePosition().z,p_initialNode);

		while (_openDictionary.Values.Count > 0) {
			Script_Node currentNode = GetLowestFScoreOpenDictionary ();

			AddClosedListElementAtPosition (currentNode.GetNodePosition ().x, currentNode.GetNodePosition ().z, currentNode);
			RemoveOpenListElementAtPosition (currentNode.GetNodePosition ().x, currentNode.GetNodePosition ().z);

			foreach (Script_Node node in _closedDictionary.Values) {
				if (node.GetNodePosition ().x == node.GetNodeDestination ().x && node.GetNodePosition ().z == node.GetNodeDestination ().z) {
					return node;
				}
			}				
			CalculateNeighbours (currentNode);
		} 
		return null;
	}
		




	public List<Script_Node> GetLocationOfPath(Script_Node p_endNode)
	{
		List<Script_Node> _pathList = new List<Script_Node>();
			
		GetCurrentPathRecursive (p_endNode, _pathList);

		_pathList.Reverse ();

		return _pathList;

	}


	private Script_Node GetCurrentPathRecursive(Script_Node p_node, List<Script_Node> p_pathList)
	{
		p_pathList.Add(p_node);

		if (p_node.GetParent () != null) {			
			return GetCurrentPathRecursive (p_node.GetParent (), p_pathList);
		} else {
			return p_node;
		}
	}





}
