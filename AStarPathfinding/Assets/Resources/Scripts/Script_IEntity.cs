using UnityEngine;

public abstract class Script_IEntity{

	public abstract Vector3Int GetGridPosition ();
	public abstract void Destruction();
	public abstract Vector3Int GetStartLocation();

}
