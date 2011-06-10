using System;
using System.Collections.Generic;
namespace Sejalan.Framework.Security
{
	public interface IMembership
	{
		string ID { get;  set; }
        string Name { get;  set; }
        string AuthenticationName { get; }
        ICollection<IMembershipGroup> Groups{ get; }
	}
}

