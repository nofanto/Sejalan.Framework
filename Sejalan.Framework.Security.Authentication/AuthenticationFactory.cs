using System;
using Sejalan.Framework.Provider;
namespace Sejalan.Framework.Security.Authentication
{
	public class AuthenticationFactory: ProviderFactory
	{
		public AuthenticationFactory () : base()
		{
		}

		protected override string Key {
			get {
				return "sejalan.framework/authentication";
			}
		}


	}
}

