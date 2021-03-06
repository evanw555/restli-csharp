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
using System;

using com.linkedin.restli.datagenerator.integration;

namespace restlicsharpdata.restlidataintegrationtest
{
    [TestClass]
    public class UnionTestTests
    {
        [TestMethod]
        public void DataMap_IntAndString()
        {
            UnionTest u;

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "unionEmpty", new Dictionary<string, object>() },
                { "unionWithoutNull", new Dictionary<string, object>()
                    {
                        { "int", 20 }
                    }
                },
                { "unionWithInline", new Dictionary<string, object>()
                    {
                        { "string", "hello, world!" }
                    }
                }
            };

            u = new UnionTest(data);

            Assert.IsInstanceOfType(u.unionEmpty, typeof(UnionTest.UnionEmpty));
            Assert.IsInstanceOfType(u.unionWithoutNull, typeof(UnionTest.UnionWithoutNull));
            Assert.IsInstanceOfType(u.unionWithInline, typeof(UnionTest.UnionWithInline));

            Assert.AreEqual(UnionTest.UnionWithoutNull.Member.Int, u.unionWithoutNull.member);
            Assert.AreEqual(20, u.unionWithoutNull.asInt);

            Assert.AreEqual(UnionTest.UnionWithInline.Member.String, u.unionWithInline.member);
            Assert.AreEqual("hello, world!", u.unionWithInline.asString);
        }

        [TestMethod]
        public void DataMap_BytesAndMap()
        {
            UnionTest u;
            
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "unionEmpty", new Dictionary<string, object>() },
                { "unionWithoutNull", new Dictionary<string, object>()
                    {
                        { "bytes", "\u0000\u0001\u0002\u0003" }
                    }
                },
                { "unionWithInline", new Dictionary<string, object>()
                    {
                        { "map", new Dictionary<string, object>()
                            {
                                { "key", 9999 }
                            }
                        }
                    }
                }
            };

            u = new UnionTest(data);

            Assert.IsInstanceOfType(u.unionEmpty, typeof(UnionTest.UnionEmpty));
            Assert.IsInstanceOfType(u.unionWithoutNull, typeof(UnionTest.UnionWithoutNull));
            Assert.IsInstanceOfType(u.unionWithInline, typeof(UnionTest.UnionWithInline));

            byte[] expectedBytes = new byte[] { 0, 1, 2, 3 };
            Assert.AreEqual(UnionTest.UnionWithoutNull.Member.Bytes, u.unionWithoutNull.member);
            CollectionAssert.AreEqual(expectedBytes, u.unionWithoutNull.asBytes.GetBytes());

            Assert.AreEqual(UnionTest.UnionWithInline.Member.Map, u.unionWithInline.member);
            Assert.AreEqual(9999, u.unionWithInline.asMap["key"]);
        }

        [TestMethod]
        public void Builder()
        {
            UnionTest u;

            UnionTestBuilder b = new UnionTestBuilder();
            b.unionEmpty = new UnionTest.UnionEmpty(new Dictionary<string, object>());
            b.unionWithoutNull = new UnionTest.UnionWithoutNull(new Dictionary<string, object>()
            {
                { "float", 1.23F }
            });
            b.unionWithInline = new UnionTest.UnionWithInline(new Dictionary<string, object>()
            {
                { "com.linkedin.restli.datagenerator.integration.EnumInUnionTest", "B" }
            });

            u = b.Build();

            Assert.IsInstanceOfType(u.unionEmpty, typeof(UnionTest.UnionEmpty));
            Assert.IsInstanceOfType(u.unionWithoutNull, typeof(UnionTest.UnionWithoutNull));
            Assert.IsInstanceOfType(u.unionWithInline, typeof(UnionTest.UnionWithInline));

            Assert.AreEqual(UnionTest.UnionWithoutNull.Member.Float, u.unionWithoutNull.member);
            Assert.AreEqual(1.23F, u.unionWithoutNull.asFloat);

            Assert.AreEqual(UnionTest.UnionWithInline.Member.EnumInUnionTest, u.unionWithInline.member);
            Assert.AreEqual(EnumInUnionTest.Symbol.B, u.unionWithInline.asEnumInUnionTest.symbol);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Builder_OmitRequired()
        {
            UnionTest u;

            UnionTestBuilder b = new UnionTestBuilder();
            b.unionEmpty = new UnionTest.UnionEmpty(new Dictionary<string, object>());

            u = b.Build();
        }

        [TestMethod]
        public void FullCycle()
        {
            UnionTest u;

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "unionEmpty", new Dictionary<string, object>() },
                { "unionWithoutNull", new Dictionary<string, object>()
                    {
                        { "bytes", "\u0000\u0001\u0002\u0003" }
                    }
                },
                { "unionWithInline", new Dictionary<string, object>()
                    {
                        { "map", new Dictionary<string, object>()
                            {
                                { "key", 9999 }
                            }
                        }
                    }
                }
            };

            u = new UnionTest(data);

            UnionTest reclaimed = new UnionTest(u.Data());

            Assert.AreNotSame(u, reclaimed);
            Assert.AreEqual(u.hasUnionWithInline, reclaimed.hasUnionWithInline);
            Assert.AreEqual(u.unionWithInline.asMap["key"], reclaimed.unionWithInline.asMap["key"]);
        }
    }
}