using System.Collections.Generic;

namespace SaladChef
{
	/// <summary>
	/// Keeps a pool of vegetables
	/// </summary>
	public class VegitablePool : IPool<IVegitable>
	{
		public Queue<IVegitable> objectPool { get; }

		public VegitablePool()
		{
			this.objectPool = new Queue<IVegitable>();
		}

		public void DeSpawn(IVegitable pooledObject)
		{
			pooledObject.Reset();
			objectPool.Enqueue(pooledObject);
		}

		public IVegitable Spawn()
		{
			if (objectPool.Count > 0)
			{
				return objectPool.Dequeue();
			}
			else
			{
				return null;
			}
		}

		IVegitable IPool<IVegitable>.Spawn()
		{
			throw new System.NotImplementedException();
		}
	}
}

