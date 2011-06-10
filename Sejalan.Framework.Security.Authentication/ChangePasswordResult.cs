using System;
namespace Sejalan.Framework.Security.Authentication
{
	public enum ChangePasswordResult
    {

        PasswordSuccessfullyChanged,
        InvalidPasswordLength,
        InvalidOldPassword,
        PasswordDoesNotMeetRequiredComplexity,
        PasswordHasBeenUsedBefore

    }

}

