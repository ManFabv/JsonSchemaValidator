using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private JSchema _schema;

        [SetUp]
        public void SetUp()
        {
            _schema = JSchema.Parse(@"{  
                           'Roles':[  
                              {
                                        'Administrator':[
                                           {  
                                       'Username':'administrator',
                                              'Password':'Welcome789',
                                              'Domain':'alab.int'
                                    },
                                    {  
                                       'Username':'manrique',
                                       'Password':'Welcome123',
                                       'Domain':'alab.int'
                                    }
                                 ]
                              },
                              {  
                                 'ServiceDesk':[
                                    {  
                                       'Username':'juarez',
                                       'Password':'Welcome123',
                                       'Domain':'alab.int'
                                    }
                                 ]
                              }
                           ]
                        }");
        }

        [Test]
        public void TestSchema()
        {
            JObject user = JObject.Parse(@"{
              'Roles': 'Administrator',
              'Username': 'manrique'
            }");

            bool valid = user.IsValid(_schema);

            Assert.IsTrue(valid);
        }

        [Test]
        public void TestGenerator()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(Account));

            Assert.IsNotNull(schema);
        }

        [Test]
        public void TestDeserializator()
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(@"[
                                  'Administrator',
                                  'ServiceDesk'
                                ]"));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
            validatingReader.Schema = _schema;

            JsonSerializer serializer = new JsonSerializer();
            List<string> roles = serializer.Deserialize<List<string>>(validatingReader);

            Assert.IsNotNull(roles);
        }

        private class Account
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
