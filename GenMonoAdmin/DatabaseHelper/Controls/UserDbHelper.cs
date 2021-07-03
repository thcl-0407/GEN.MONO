using DatabaseHelper.Enums;
using DatabaseHelper.Entities;
using System;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DatabaseHelper.Controls
{
    public interface IUserDbHelper
    {
        Tuple<DatabaseResponse, List<User>> GetUsersByEmail(string email);
        Tuple<DatabaseResponse, List<User>> GetUsersByPhone(string phone);
    }

    public class UserDbHelper : IUserDbHelper
    {
        private IMongoCollection<User> _mongoCollection;

        public UserDbHelper(string ConnectionString)
        {
            var database = new MongoClient(ConnectionString).GetDatabase("genmono");
            _mongoCollection = database.GetCollection<User>("users");
        }

        public Tuple<DatabaseResponse, List<User>> GetUsersByEmail(string email)
        {
            try
            {
                var users = _mongoCollection.Find<User>(u => u.Email.Contains(email)).ToList();

                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        user.HashPassword = null;
                    }
                }

                return new Tuple<DatabaseResponse, List<User>>(DatabaseResponse.SUCCESSFULLY, users);
            }
            catch
            {
                return new Tuple<DatabaseResponse, List<User>>(DatabaseResponse.HAVE_EXCEPTION, null);
            }
        }

        public Tuple<DatabaseResponse, List<User>> GetUsersByPhone(string phone)
        {
            try
            {
                var users = _mongoCollection.Find<User>(u => u.PhoneNo.Contains(phone)).ToList();

                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        user.HashPassword = null;
                    }
                }

                return new Tuple<DatabaseResponse, List<User>>(DatabaseResponse.SUCCESSFULLY, users);
            }
            catch
            {
                return new Tuple<DatabaseResponse, List<User>>(DatabaseResponse.HAVE_EXCEPTION, null);
            }
        }
    }
}
