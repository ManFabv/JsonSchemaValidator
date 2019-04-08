using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using System.Collections;
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
            jsonInput = @"{'Role' : [{ 'Domain':'alab.int', 'Username':'administrator', 'Password':'Welcome789' }, { 'Domain':'alab.int', 'Username':'manrique', 'Password':'Welcome123' }]}";
        }

        [Test]
        public void ValidateAndCreateObjects()
        {
            var schemaGenerator = new JSchemaGenerator();
            var schema = schemaGenerator.Generate(typeof(Role));

            var textReader = new StringReader(jsonInput);
            var jsonTextReader = new JsonTextReader(textReader);

            var jSchemaValidatingReader = new JSchemaValidatingReader(jsonTextReader);
            jSchemaValidatingReader.Schema = schema;

            var jsonSerializer = new JsonSerializer();
            var role = jsonSerializer.Deserialize<Role>(jSchemaValidatingReader);
        }

        public class Role
        {          
            [JsonProperty("Role")]
            public List<Account> roles;
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