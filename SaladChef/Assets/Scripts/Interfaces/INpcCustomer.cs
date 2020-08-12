using System;
using System.Collections.Generic;

namespace SaladChef
{
	public interface INpcCustomer
	{
		int id { get; }
		float waitingTime { get; }
		int orderNumber { get; }
		bool isAngry { get; }
		Satisfaction satisfaction { get; }
		List<string> servicedBy { get; }
		bool Service(ISalad salad, string serverID);
		void Initialize(int orderNumber);
	}
}