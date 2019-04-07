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
        [Test]
        public void TestSchema()
        {
            JSchema schema = JSchema.Parse(@"{
              'type': 'object',
              'properties': {
                'name': {'type':'string'},
                'roles': {'type': 'array'}
              }
            }");

            JObject user = JObject.Parse(@"{
              'name': 'Arnie Admin',
              'roles': ['Developer', 'Administrator']
            }");

            bool valid = user.IsValid(schema);

            Assert.IsTrue(valid);
        }

        [Test]
        public void TestGenerator()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(Account));
            // {
            //   "type": "object",
            //   "properties": {
            //     "email": { "type": "string", "format": "email" }
            //   },
            //   "required": [ "email" ]
            // }

            Assert.IsNotNull(schema);
        }

        [Test]
        public void TestDeserializator()
        {
            JSchema schema = JSchema.Parse(@"{
                              'type': 'array',
                              'item': {'type':'string'}
                                }");
            JsonTextReader reader = new JsonTextReader(new StringReader(@"[
                                  'Developer',
                                  'Administrator'
                                ]"));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
            validatingReader.Schema = schema;

            JsonSerializer serializer = new JsonSerializer();
            List<string> roles = serializer.Deserialize<List<string>>(validatingReader);

            Assert.IsNotNull(roles);
        }

        private class Account
        {
            [JsonProperty("email", Required = Required.Always)]
            public string Email;
        }
    }
}
