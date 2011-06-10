using System;
namespace Sejalan.Framework.Security.Authentication
{
	public enum ValidateUserResult
    {
        InvalidPassword,
        PasswordAttemptNearExpired,
        UserAccountDoesNotExist,
        UserAccountExpired,
        UserAccountFirstLogin,
        UserAccountLocked,
        UserAccountSuccessfullyAuthenticated,
        UserPasswordExpired,
        UserSuccesfullyValidated
    }
}

