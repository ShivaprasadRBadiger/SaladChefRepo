using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBindings", menuName = "Input/Bindings", order = 1)]
public class InputBindings : ScriptableObject
{
	public KeyCode Up;
	public KeyCode Down;
	public KeyCode Left;
	public KeyCode Right;
	public KeyCode Action1;
	public KeyCode Action2;
}



