using UnityEngine;

public interface IInputBindings
{
	KeyCode Action1 { get; set; }
	KeyCode Action2 { get; set; }
	KeyCode Down { get; set; }
	KeyCode Left { get; set; }
	KeyCode Right { get; set; }
	KeyCode Up { get; set; }
}