using System;
using System.Collections.Generic;
namespace Sejalan.Framework.Security
{
	public interface IAuditable
	{
		string ID { get; set; }
        string Name { get; set; }
	}
}

