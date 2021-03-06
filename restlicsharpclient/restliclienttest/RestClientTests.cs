﻿/*
   Copyright (c) 2017 LinkedIn Corp.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.IO;

using com.linkedin.restli.test.api;
using restlicsharpclient.restliclient.util;

namespace restlicsharpclient.restliclienttest
{
    [TestClass]
    public class RestClientTests
    {
        [TestMethod]
        public void DeserializeDataMap()
        {
            string dataMapString = @"
                {
                    'foo': 'bar',
                    'nested': {
                        'foofoo': 'barbar',
                        'list': [1, 2, 3]
                    },
                    'list': [10, 20, 30],
                    'null': null,
                    'primitive': 42,
                    'one': {
                        'two': {
                            'three': {
                                'four': [1, 'hello', null, 4.2]
                            }
                        }
                    },
                    'complexlist': [1, [2, [3, 4]]]
                }";

            Dictionary<string, object> dataMap = DataUtil.StringToMap(dataMapString);

            Assert.AreEqual("bar", dataMap["foo"]);

            Dictionary<string, object> nested = (Dictionary<string, object>)dataMap["nested"];
            Assert.AreEqual("barbar", nested["foofoo"]);
            CollectionAssert.AreEqual(new List<object> { (long)1, (long)2, (long)3 }, (List<object>)nested["list"], "Failed on [1, 2, 3]");

            CollectionAssert.AreEqual(new List<object> { (long)10, (long)20, (long)30 }, (List<object>)dataMap["list"], "Failed on [10, 20, 30]");
            Assert.IsNull(dataMap["null"]);
            Assert.AreEqual((long)42, dataMap["primitive"]);

            CollectionAssert.AreEqual(new List<object> { (long)1, "hello", null, (double)4.2 }, (List<object>)(((dataMap["one"] as Dictionary<string, object>)["two"] as Dictionary<string, object>)["three"] as Dictionary<string, object>)["four"], "Failed on [1, 'hello', null, 4.2]");

            Assert.AreEqual((long)4, (((dataMap["complexlist"] as List<object>)[1] as List<object>)[1] as List<object>)[1]);
        }

        [TestMethod]
        public void StreamReadAllBytes()
        {
            byte[] original = { 48, 49, 50, 51, 0, 1, 2, 3, 4};
            Stream stream = new MemoryStream(original);
            byte[] reclaimed = stream.ReadAllBytes();

            CollectionAssert.AreEqual(original, reclaimed);            
        }

        [TestMethod]
        public void SerializeAndDeserializeRecordWithEnum()
        {
            GreetingBuilder greetingBuilder = new GreetingBuilder();
            greetingBuilder.id = 123;
            greetingBuilder.tone = new Tone(Tone.Symbol.SINCERE);
            greetingBuilder.message = "Hello, Serialize test!";
            Greeting g = greetingBuilder.Build();

            string serialized = DataUtil.MapToString(g.Data());

            Dictionary<string, object> dataMap = DataUtil.StringToMap(serialized);
            Greeting reclaimed = new Greeting(dataMap);

            Assert.AreEqual(g.id, reclaimed.id);
            Assert.AreEqual(g.message, reclaimed.message);
            Assert.AreEqual(g.tone.symbol, reclaimed.tone.symbol);
        }
    }
}