using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace JsonSchemaValidator.Test
{
    [TestFixture]
    class JSonSchemaValidatorTests
    {
        private string jsonInput;

        [SetUp]
        public void SetUp()
        {
            jsonInput = @"{
                          'Roles': [
                            {
                              'Role': 'Administrator',
                              'Accounts': [
                                {
                                  'Domain': 'alab.int',
                                  'Username': 'administrator',
                                  'Password': 'Welcome789'
                                },
                                {
                                  'Domain': 'alab.int',
                                  'Username': 'manrique',
                                  'Password': 'Welcome123'
                                }
                              ]
                            },
                            {
                              'Role': 'ServiceDesk',
                              'Accounts': [
                                {
                                  'Domain': 'alab.int',
                                  'Username': 'juarez',
                                  'Password': 'Welcome123'
                                }
                              ]
                            }
                          ]
                        }";
        }

        [Test]
        public void ValidateAndCreateObjects()
        {
            var schemaGenerator = new JSchemaGenerator();
            var schema = schemaGenerator.Generate(typeof(Roles));

            var textReader = new StringReader(jsonInput);
            var jsonTextReader = new JsonTextReader(textReader);

            var jSchemaValidatingReader = new JSchemaValidatingReader(jsonTextReader);
            jSchemaValidatingReader.Schema = schema;

            var jsonSerializer = new JsonSerializer();
            var role = jsonSerializer.Deserialize<Roles>(jSchemaValidatingReader);
        }

        public class Roles
        {
            [JsonProperty("Roles")]
            public List<Role> roles = new List<Role>();
        }

        public class Role
        {          
            [JsonProperty("Role", Required = Required.Always)]
            public string RoleName { get; set; }
            [JsonProperty("Accounts")]
            public List<Account> accounts = new List<Account>();
        }

        public class Account
        {
            [JsonProperty("Username", Required = Required.Always)]
            public string Username;
            [JsonProperty("Domain", Required = Required.Always)]
            public string Domain;
            [JsonProperty("Password", Required = Required.Always)]
            public string Password;
        }
    }
}