using UnityEngine;
using System.Collections;
namespace SaladChef
{
	public class SaladFactory : SaladFactoryBase
	{
		private static SaladFactory _instance;
		public static SaladFactory GetInstance()
		{
			if (_instance == null)
			{
				_instance = new SaladFactory();
			}
			return _instance;
		}
		private SaladFactory() { }

		public override ISalad GetSalad()
		{
			return new Salad();
		}
	}
}

