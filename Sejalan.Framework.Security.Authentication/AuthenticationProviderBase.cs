using System;
using Sejalan.Framework.Provider;
using Sejalan.Framework.Preferences;
namespace Sejalan.Framework.Security.Authentication
{
	public abstract class AuthenticationProviderBase : ProviderBase
	{
		
		public AuthenticationProviderBase ()
		{
		}
		
		protected PreferenceItemCollection Preferences
		{
			get{
				return ProviderFactory.GetInstance<PreferencesFactory>(ProviderRepositoryFactory.Instance.Providers[Parameters["preferencesRepositoryName"]]).GetProviders<PreferencesProviderBase>()[Parameters["preferencesProviderName"]].GetPreferences(this.Parameters["preferencesCollectionName"]);
			}
		}
		
		#region Properties
				                                                
				                                                

        /// <summary>
        /// Gets the maximum password attempt.
        /// </summary>
        /// <value>The maximum password attempt.</value>
        public int MaximumPasswordAttempt
        {
            get
            {
                return Convert.ToInt32(Preferences["MaximumPasswordAttempt"].Value);
            }
        }

        /// <summary>
        /// Gets the maximum password expiration day.
        /// </summary>
        /// <value>The maximum password expiration day.</value>
        public int MaximumPasswordExpirationDay
        {
            get
            {
                return Convert.ToInt32(Preferences["MaximumPasswordExpirationDay"].Value);
            }
        }

        /// <summary>
        /// Gets the maximum password stored in history.
        /// </summary>
        /// <value>The maximum password stored in history.</value>
        public int MaximumPasswordStoredInHistory
        {
            get
            {
                return Convert.ToInt32(Preferences["MaximumPasswordStoredInHistory"].Value);
            }
        }

        /// <summary>
        /// Gets the maximum user expiration month.
        /// </summary>
        /// <value>The maximum user expiration month.</value>
        public int MaximumUserExpirationMonth
        {
            get
            {
                return Convert.ToInt32(Preferences["MaximumUserExpirationMonth"].Value);
            }
        }

        /// <summary>
        /// Gets the minimum length of the required password.
        /// </summary>
        /// <value>The minimum length of the required password.</value>
        public int MinimumRequiredPasswordLength
        {
            get
            {
                return Convert.ToInt32(Preferences["MinimumRequiredPasswordLength"].Value);
            }
        }

        /// <summary>
        /// Gets the minimum length of the user name.
        /// </summary>
        /// <value>The minimum length of the user name.</value>
        public int MinimumUserNameLength
        {
            get
            {
                return Convert.ToInt32(Preferences["MinimumUserNameLength"].Value);
            }
        }

        #endregion
                
  
		
		#region Methods
        
        /// <summary>
        /// When overriden in a derived class, changes the user's password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="NewPassword">The new password.</param>
        public abstract ChangePasswordResult ChangePassword(IMembership user, string oldPassword, string NewPassword);

        /// <summary>
        /// When overriden in a derived class, creates a user in the database.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="password">The password.</param>
        /// <param name="userGroupId">The user group id.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public abstract CreateUserResult CreateUser(IMembership user);

        /// <summary>
        /// When overriden in a derived class, deletes a user from the database.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="deleteAllRelatedData">if set to <c>true</c> [delete all related data].</param>
        public abstract void DeleteUser(IMembership user, bool deleteAllRelatedData);

        /// <summary>
        /// When overriden in a derived class, gets the user information.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public abstract IMembership GetUser(string userId);

        public abstract DateTime GetUserLoginTime(IMembership user);

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public abstract string HashPassword(string password);

        public abstract bool IsAlreadyLogin(IMembership user, object loginIdentifier);

        public abstract bool IsEligibleToLogin(IMembership user, object loginIdentifier);

        public abstract bool IsPasswordHasBeenUsedBefore(IMembership user, string password);

        public abstract bool IsUserPasswordExpired(IMembership user);

        /// <summary>
        /// Determines whether [is password meets required complexity] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// <c>true</c> if [is password meets required complexity] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsPasswordMeetsRequiredComplexity(string password);

        /// <summary>
        /// When overriden in a derived class, locks the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public abstract void LockUser(IMembership user);

        public abstract void SetUserLoginTime(IMembership user, DateTime loginTime);

        /// <summary>
        /// When overriden in a derived class, unlock the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public abstract void UnlockUser(IMembership user);

        /// <summary>
        /// When overriden in a derived class, updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public abstract void UpdateUser(IMembership user);

        /// <summary>
        /// When overriden in a derived class, validates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public abstract ValidateUserResult ValidateUser(string userId, string password);

        #endregion

	}
}

