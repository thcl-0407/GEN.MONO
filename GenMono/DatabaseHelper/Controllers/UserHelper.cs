using System;
using MongoDB.Driver;
using DatabaseHelper.Identities;
using DatabaseHelper.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extension;
using MongoDB.Bson;

namespace DatabaseHelper.Controllers
{
    public interface IUserHelper
    {
        Tuple<DatabaseHelperResponse, User> SignUp(User user);
        Task<List<User>> GetUsersByEmail(string key);
        Task<List<User>> GetUsersByPhoneNo(string key);
        Task<User> GetUserByID(string UserID);
        Tuple<DatabaseHelperResponse, User> SignIn(SignInType type, string EmailOrPhone, string Password);
        Tuple<DatabaseHelperResponse, User> UpdatePassword(string Id, string OldPassword, string NewPassword);
        Tuple<DatabaseHelperResponse, User> UpdateProfile(User user);
        Tuple<DatabaseHelperResponse, User> UpdateVerifyStatus(User user, string statusToUpdate);
    }

    public class UserHelper:IUserHelper
    {
        private IMongoCollection<User> _userCollection;

        public UserHelper(string ConnectionString)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase("genmono");
            _userCollection = database.GetCollection<User>("users");
        }

        public Task<User> GetUserByID(string UserID)
        {
            return _userCollection.Find<User>(u => u.UserID.Equals(UserID)).FirstOrDefaultAsync<User>();
        }

        public Task<List<User>> GetUsersByEmail(string key)
        {
            return _userCollection.Find<User>(u => u.Email.Equals(key)).ToListAsync();
        }

        public Task<List<User>> GetUsersByPhoneNo(string key)
        {
            return _userCollection.Find<User>(u => u.PhoneNo.Equals(key)).ToListAsync();
        }

        public Tuple<DatabaseHelperResponse, User> UpdatePassword(string Id, string OldPassword, string NewPassword)
        {
            var user = GetUserByID(Id).Result;

            if (user != null)
            {
                if(BCrypt.Net.BCrypt.Verify(OldPassword, user.HashPassword))
                {
                    try
                    {
                        var update = Builders<User>.Update.Set("HashPassword", BCrypt.Net.BCrypt.HashPassword(NewPassword));

                        _userCollection.UpdateOne(x => x.UserID.Equals(Id), update);
                    }
                    catch
                    {
                        return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
                    }
                    
                    user.HashPassword = null;
                    return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, user);
                }

                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.NOT_EQUAL, null);
            }
            else
            {
                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.NOT_FOUND, null);
            }
        }

        public Tuple<DatabaseHelperResponse, User> SignIn(SignInType type, string EmailOrPhone, string Password)
        {
            if (EmailOrPhone.IsNullOrEmpty() || Password.IsNullOrEmpty())
            {
                throw new ArgumentNullException();
            }

            //Login By Email
            if (SignInType.EMAIL.Equals(type))
            {
                var users = GetUsersByEmail(EmailOrPhone).Result;

                if (users.Count == 1)
                {
                    if (users[0].isActived.IsNotNullOrEmpty())
                    {
                        if (Convert.ToBoolean(users[0].isActived))
                        {
                            if (BCrypt.Net.BCrypt.Verify(Password, users[0].HashPassword))
                            {
                                users[0].HashPassword = null;
                                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, users[0]);
                            }
                            else
                            {
                                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.NOT_EQUAL, null);
                            }
                        }
                    }
                }
            }

            //Login By Phone
            if (SignInType.PHONE_NO.Equals(type))
            {
                var users = GetUsersByPhoneNo(EmailOrPhone).Result;

                if (users.Count == 1)
                {
                    if (users[0].isActived.IsNotNullOrEmpty())
                    {
                        if (Convert.ToBoolean(users[0].isActived))
                        {
                            if (BCrypt.Net.BCrypt.Verify(Password, users[0].HashPassword))
                            {
                                users[0].HashPassword = null;
                                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, users[0]);
                            }
                            else
                            {
                                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.NOT_EQUAL, null);
                            }
                        }
                    }
                }
            }

            return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
        }

        public Tuple<DatabaseHelperResponse, User> SignUp(User user)
        {
            if(user == null)
            {
                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.OBJECT_IS_NULL, null);
            }

            if (!user.isValid())
            {
                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.VALUE_IS_NULL_OR_EMPTY, null);
            }

            try
            {
                if (user.PhoneNo.IsNotNullOrEmpty())
                {
                    var users = GetUsersByPhoneNo(user.PhoneNo).Result;

                    if(users.Count > 0)
                    {
                        return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
                    }
                }

                if (user.Email.IsNotNullOrEmpty())
                {
                    var users = GetUsersByEmail(user.Email).Result;

                    if (users.Count > 0)
                    {
                        return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
                    }
                }

                //If all of constraints is right
                _userCollection.InsertOne(user);

                user.HashPassword = null;

                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, user);
            }
            catch
            {
                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
            }
        }

        public Tuple<DatabaseHelperResponse, User> UpdateProfile(User user)
        {
            if (user == null)
            {
                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.OBJECT_IS_NULL, null);
            }

            try
            {
                var CurrentUser = GetUserByID(user.UserID).Result;

                if (CurrentUser == null)
                {
                    return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.NOT_FOUND, null);
                }

                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("FullName", user.FullName));
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("DateOfBirth", user.DateOfBirth));
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("Gender", user.Gender));
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("AddressLine", user.AddressLine));
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("StateOrCity", user.StateOrCity));
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("Country", user.Country));

                var NewUpdate = GetUserByID(user.UserID).Result;

                if (NewUpdate != null) {
                    NewUpdate.HashPassword = null;
                }

                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, NewUpdate);
            }
            catch
            {
                return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
            }
        }

        public Tuple<DatabaseHelperResponse, User> UpdateVerifyStatus(User user, string statusToUpdate)
        {
            if (user != null && user.UserID.IsNotNullOrEmpty())
            {
                try
                {
                    //Handle For Email
                    if (user.Email.IsNotNullOrEmpty() && user.Email.IsRightEmail())
                    {
                        UpdateVerifyStatusForEmail(user, statusToUpdate.IsNullOrEmpty() ? Boolean.FalseString : statusToUpdate);
                    }

                    //Handle For PhoneNo
                    if (user.PhoneNo.IsNotNullOrEmpty() && user.PhoneNo.IsRightPhoneNo())
                    {
                        UpdateVerifyStatusForPhoneNo(user, statusToUpdate.IsNullOrEmpty() ? Boolean.FalseString : statusToUpdate);
                    }

                    //Handle Response Success
                    var NewUpdate = GetUserByID(user.UserID).Result;

                    if (NewUpdate != null)
                    {
                        NewUpdate.HashPassword = null;
                    }

                    return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.SUCCESS, NewUpdate);
                }
                catch
                {
                    return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.HAVE_EXCEPTION, null);
                }
            }

            return new Tuple<DatabaseHelperResponse, User>(DatabaseHelperResponse.OBJECT_IS_NULL, null);
        }

        private void UpdateVerifyStatusForEmail(User user, string statusToUpdate)
        {
            var CurrentUser = GetUsersByEmail(user.Email).Result;

            if (CurrentUser != null && CurrentUser.Count == 1 && CurrentUser[0].UserID.Equals(user.UserID))
            {
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("isVerifiedEmail", statusToUpdate));
            }
            else
            {
                throw new FieldAccessException();
            }
        }

        private void UpdateVerifyStatusForPhoneNo(User user, string statusToUpdate)
        {
            var CurrentUser = GetUsersByPhoneNo(user.PhoneNo).Result;

            if (CurrentUser != null && CurrentUser.Count == 1 && CurrentUser[0].UserID.Equals(user.UserID))
            {
                _userCollection.UpdateOne(x => x.UserID.Equals(user.UserID), Builders<User>.Update.Set("isVerifiedPhone", statusToUpdate));
            }
            else
            {
                throw new FieldAccessException();
            }
        }
    }
}
