
using Microsoft.EntityFrameworkCore;
using USER.API.Helpers;
using USER.API.Models;

namespace USER.API.Repositories
{
    public class AuthUserRepositories : IAuthUser
    {
        private readonly DatabaseContext _dbContext;

        public AuthUserRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

		//public async Task<User> Login(Login login)
		//{
		//	var user = login.LoginToken != null
		//		? await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail && u.ConfirmationToken == login.LoginToken)
		//		: await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail && u.EmailConfirmed == true);

		//	if (user != null)
		//	{
		//		bool veriPass = PasswordBcrypt.VerifyPassword(login.Password, user.Password);
		//		if (veriPass)
		//		{
		//			return user;
		//		}
		//	}

		//	return null;
		//}

		public async Task<User> Login(Login login)
		{
			if (login.LoginToken != null)
			{
				var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail);
				if (user != null)
				{
                    if (user.ConfirmationToken == login.LoginToken && user.ConfirmationTokenExpiry > DateTime.UtcNow)
					{
						bool veriPass = PasswordBcrypt.VerifyPassword(login.Password, user.Password);
						if (veriPass)
						{
							user.EmailConfirmed = true;
							user.ConfirmationTokenExpiry = null;
							_dbContext.Users.Update(user);
							await _dbContext.SaveChangesAsync();
							return user;
						}
						return null;

					}
				}
			}
			else
			{
				var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail && u.EmailConfirmed == true);

				if (user != null)
				{
                    if (user.EmailConfirmed == true)
					{
						bool veriPass = PasswordBcrypt.VerifyPassword(login.Password, user.Password);
						if (veriPass)
						{
							return user;
						}
						return null;

					}
				}

			}

			return null;
		}

		// login with token
		//public async Task<User> LoginToken(Login login)
		//{


		//	var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail && u.ConfirmationToken == login.LoginToken && u.ConfirmationTokenExpiry > DateTime.UtcNow);

		//	if (user != null)
		//	{
		//		bool veriPass = PasswordBcrypt.VerifyPassword(login.Password, user.Password);
		//		if (veriPass)
		//		{
		//                  user.EmailConfirmed = true;
		//                  _dbContext.Users.Update(user);
		//			return user;
		//		}
		//		return null;
		//	}
		//	return null;
		//}

	}
}
