﻿using System;
using QuantoAgent.Exceptions;
using QuantoAgent.Log;
using SQLite;

namespace QuantoAgent.Database {
    public static class UserManager {
        const string FileName = "users.db";
        static readonly SQLiteConnection conn;

        static UserManager() {
            conn = new SQLiteConnection(FileName);
            var x = conn.GetTableInfo("DBUser");
            if (x.Count == 0) {
                conn.CreateTable<DBUser>();
            }

            if (GetUser("admin") == null) {
                Logger.Log("UserManager", "Admin user does not exists. Creating default admin with password admin");
                AddUser("Administrator", "admin", "admin");
            }

            AppDomain.CurrentDomain.ProcessExit += (sender, e) => UserManagerDestructor();
        }

        public static void AddUser(string name, string username, string password) {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            string id = Guid.NewGuid().ToString();

            var user = GetUser(username);
            if (user == null) {
                conn.Insert(new DBUser { UserId = id, Name = name, Password = hashedPassword });
            } else {
                throw new UserAlreadyExists();
            }
        }

        static DBUser CheckUser(string username, string password) {
            var user = GetUser(username);
            if (user == null) {
                return null;
            }

            var hash = user.Password;
            user.Password = "";

            return BCrypt.Net.BCrypt.Verify(password, hash) ? user : null;
        }

        static DBUser GetUser(string username) {
            var res = conn.Table<DBUser>().Where(a => a.UserName == username);
            return res.Count() > 0 ? res.First() : null;
        }

        static void UserManagerDestructor() {
            conn.Close();
        }
    }
}