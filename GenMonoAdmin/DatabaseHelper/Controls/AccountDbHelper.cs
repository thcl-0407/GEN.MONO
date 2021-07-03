using System;
using MongoDB.Driver;
using DatabaseHelper.Entities;
using DatabaseHelper.Enums;
using Extension;
using MongoDB;
using System.Collections.Generic;

namespace DatabaseHelper.Controls
{
    public interface IAccountDbHelper
    {
        Tuple<DatabaseResponse, Admin> SignIn(string UserName, string Password);
    }

    public class AccountDbHelper : IAccountDbHelper
    {
        private IMongoCollection<Admin> _mongoCollection;

        public AccountDbHelper(string ConnectionString)
        {
            var database = new MongoClient(ConnectionString).GetDatabase("genmono");
            _mongoCollection = database.GetCollection<Admin>("admins");
        }

        private Admin GetAdminByUserName(string UserName)
        {
            return _mongoCollection.Find<Admin>(a => a.UserName.Equals(UserName)).FirstOrDefault<Admin>();
        }

        public Tuple<DatabaseResponse, Admin> SignIn(string UserName, string Password)
        {
            if (UserName.isNullOrEmpty() || Password.isNullOrEmpty())
            {
                return new Tuple<DatabaseResponse, Admin>(DatabaseResponse.OBJECT_NULL, null);
            }

            var CurrentAdmin = GetAdminByUserName(UserName);

            if (CurrentAdmin == null)
            {
                return new Tuple<DatabaseResponse, Admin>(DatabaseResponse.NOT_FOUND, null);
            }

            if (!BCrypt.Net.BCrypt.Verify(Password, CurrentAdmin.HashPassword))
            {
                return new Tuple<DatabaseResponse, Admin>(DatabaseResponse.NOT_EQUAL, null);
            }
            else
            {
                CurrentAdmin.HashPassword = null;
                return new Tuple<DatabaseResponse, Admin>(DatabaseResponse.SUCCESSFULLY, CurrentAdmin);
            }
        }
    }
}
