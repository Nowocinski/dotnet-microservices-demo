using EShop.Infrastructure.Command.User;
using EShop.Infrastructure.Security;

namespace EShop.User.Api.Extension
{
    public static class Extension
    {
        public static CreateUser SetPassword(this CreateUser user, IEncrypter encrypter)
        {
            var salt = encrypter.GetSalt();
            user.Password = encrypter.GetHash(user.Password, salt);

            return user;
        }

        public static bool ValidatePassword(this CreateUser user, CreateUser savedUser, IEncrypter encrypter)
        {
            var pswd = encrypter.GetHash(user.Password, encrypter.GetSalt());
            return user.Password.Equals(pswd);
        }
    }
}
