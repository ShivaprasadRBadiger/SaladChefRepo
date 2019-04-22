using System;

namespace SaladChef
{
	public interface INpcCustomer
	{
		int id { get; }
		float waitingTime { get; }
		bool isAngry { get; }
		Satisfaction satisfaction { get; }
		string servicedBy { get; }
		bool Service(ISalad salad, string serverID);
	}
}