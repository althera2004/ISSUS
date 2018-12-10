// --------------------------------
// <copyright file="UniqueSession.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Implements UniqueSession class</summary>
    public static class UniqueSession
    {
        /// <summary>Data of unique sessions active</summary>
        private static List<UniqueSessionData> data;

        /// <summary>Indicates if session token is present on application</summary>
        /// <param name="token">Session's token</param>
        /// <param name="userId">User identififer</param>
        /// <returns>Session token is present on application</returns>
        public static bool Exists(Guid token, int userId)
        {
            if (data == null)
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
        /// <returns>Indentifier of new user token</returns>
        public static Guid ReplaceUser(int oldUser, int newUser)
        {
            var result = Guid.Empty;
            if (data == null)
            {
                data = new List<UniqueSessionData>();
            }

            UnsetSession(newUser);
            var newData = new List<UniqueSessionData>();
            if (data.Any(d => d.UserId == oldUser))
            {
                foreach (var item in data)
                {
                    if (item.UserId == oldUser)
                    {
                        newData.Add(new UniqueSessionData
                        {
                            IP = item.IP,
                            LastConnection = item.LastConnection,
                            Token = item.Token,
                            UserId = newUser
                        });
                    }
                    else
                    {
                        newData.Add(item);
                    }
                }

                data = newData;
                result = data.First(d => d.UserId == newUser).Token;
            }
            else
            {
                result = SetSession(newUser, string.Empty);
            }

            return result;
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
            data.Add(new UniqueSessionData
            {
                Token = token,
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

            data = data.Where(d => d.UserId != userId).ToList();
        }
    }
}