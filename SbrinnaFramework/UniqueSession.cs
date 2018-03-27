// --------------------------------
// <copyright file="UniqueSession.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace SbrinnaCoreFramework
{
    /// <summary>Implements UniqueSession class</summary>
    public static class UniqueSession
    {
        /// <summary>Data of unique sessions active</summary>
        private static List<UniqueSessionData> data;

        /// <summary>Indicates if session toke is present on application</summary>
        /// <param name="token">Session's token</param>
        /// <param name="userId">User identififer</param>
        /// <returns></returns>
        public static bool Exists(Guid token, int userId)
        {
            if(data == null)
            {
                data = new List<UniqueSessionData>();
            }

            if (data.Any(d => d.Token == token && d.UserId == userId))
            {
                return true;
            }

            return false;
        }

        /// <summary>Replace user session</summary>
        /// <param name="oldUser">Old user session</param>
        /// <param name="newUser">New user seesion</param>
        /// <returns></returns>
        public static Guid ReplaceUser(int oldUser, int newUser)
        {
            if (data == null)
            {
                data = new List<UniqueSessionData>();
            }

            UnsetSession(newUser);
            if (data.Any(d => d.UserId == oldUser))
            {
                data.First(d => d.UserId == oldUser).UserId = newUser;
                return data.First(d => d.UserId == newUser).Token;
            }
            else
            {
                return SetSession(newUser, string.Empty);
            }
        }

        /// <summary>Set session</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="ip">IP address of last connection</param>
        /// <returns>Token of session user</returns>
        public static Guid SetSession(int userId, string ip)
        {
            if (data == null)
            {
                data = new List<UniqueSessionData>();
            }

            UnsetSession(userId);

            var token = Guid.NewGuid();
            data.Add(new UniqueSessionData()
            {
                Token =token,
                UserId = userId,
                IP = ip,
                LastConnection = DateTime.Now
            });

            return token;
        }

        /// <summary>Unset session of user</summary>
        /// <param name="userId">User identifier</param>
        public static void UnsetSession(int userId)
        {
            if (data == null)
            {
                data = new List<UniqueSessionData>();
            }

            data = data.Where(d=>d.UserId != userId).ToList();
        }
    }
}