using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankofdotNet.IdentitySvr
{
    public class Config
    {

        //creo gli utenti per il gran_type Resource Owner
        public static List<TestUser> GetUsers() {

            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Gaetano",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "damiano",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
               new ApiResource("BankofdotNetAPI","Api resourcer for Identity application")
            };

        }

        //creo i client per il grant_type ClientCredential 
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256(),"password usata dal client")
                    },
                    AllowedScopes = { "BankofdotNetAPI" }   //elenco degli scopes o resources a cui può accedere il client
                },

                //Resource owner-password grant 
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "BankofdotNetAPI" }
                }

            };
        }
    }
}
