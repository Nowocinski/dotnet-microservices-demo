using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Event.User;
using EShop.Infrastructure.Security;

namespace EShop.User.DataProvider.Extension
{
    public static class Extension
    {
        public static CreateUser SetPassword(this CreateUser user, IEncrypter encrypter)
        {
            var salt = encrypter.GetSalt();
            user.Password = encrypter.GetHash(user.Password, salt);

            return user;
        }

        public static bool ValidatePassword(this UserCreated userCreated, LoginUser user, IEncrypter encrypter)
        {
            var pswd = encrypter.GetHash(user.Password, encrypter.GetSalt());
            return userCreated.Password.Equals(pswd);
        }
    }
}
