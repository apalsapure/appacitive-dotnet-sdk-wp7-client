using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Appacitive.Mobile.Windows.Todo.Utility;
using Appacitive.Sdk;
using Newtonsoft.Json.Linq;

namespace Appacitive.Mobile.WindowsPhone7.Todo
{
    internal class UserManager
    {
        public static async Task<User> Save(User user, string password)
        {
            try
            {
                user.Password = password;
                await user.SaveAsync();
                return user;
            }
            catch { return null; }
        }
        public static async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                var credentials = new UsernamePasswordCredentials(email, password);
                var userSession = await credentials.AuthenticateAsync();
                Sdk.App.SetLoggedInUser(userSession.UserToken);
                Context.User = userSession.LoggedInUser;
                return true;
            }
            catch { return false; }
        }
    }
}