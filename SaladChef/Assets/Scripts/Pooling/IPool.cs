using System.Collections.Generic;

namespace SaladChef
{
	public interface IPool<T> where T : IPoolable
	{
		Queue<T> objectPool { get; }
		T Spawn();
		void DeSpawn(T pooledObject);
	}
}

