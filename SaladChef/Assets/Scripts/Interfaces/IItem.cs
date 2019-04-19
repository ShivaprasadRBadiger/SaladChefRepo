namespace SaladChef
{
	/// <summary>
	/// Any intractable non-character objects should implement IItem
	/// </summary>
	public interface IItem
	{
		int id { get; }
		string name { get; }
	}

}