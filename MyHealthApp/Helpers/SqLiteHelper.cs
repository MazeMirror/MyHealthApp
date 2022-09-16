﻿using System.Linq;
using System.Threading.Tasks;
using MyHealthApp.Models.SqlLite;
using SQLite;
namespace MyHealthApp.Helpers
{
    public class SqLiteHelper
    {
        private SQLiteAsyncConnection db;

        public SqLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<UserEntity>().Wait();
            db.CreateTableAsync<ProfileEntity>().Wait();
        }

        public Task<int> SaveUserAsync(UserEntity user)
        {
            return db.InsertAsync(user);
        }
        
        public Task<int> SaveProfileAsync(ProfileEntity profile)
        {
            return db.InsertAsync(profile);
        }

        public Task<UserEntity> GetUserAsync()
        {
            return db.Table<UserEntity>().FirstAsync();
        }
        
        public Task<ProfileEntity> GetProfileAsync()
        {
            return db.Table<ProfileEntity>().FirstAsync();
        }

    }
}