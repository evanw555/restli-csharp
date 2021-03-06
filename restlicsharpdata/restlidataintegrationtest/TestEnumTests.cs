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

using com.linkedin.restli.datagenerator.integration;

namespace restlicsharpdata.restlidataintegrationtest
{
    [TestClass]
    public class TestEnumTests
    {
        [TestMethod]
        public void InitWithString()
        {
            TestEnum e = new TestEnum("SYMBOL_1");

            Assert.AreEqual(TestEnum.Symbol.SYMBOL_1, e.symbol);
        }

        [TestMethod]
        public void InitWithSymbol()
        {
            TestEnum e = new TestEnum(TestEnum.Symbol.SYMBOL_2);

            Assert.AreEqual(TestEnum.Symbol.SYMBOL_2, e.symbol);
        }

        [TestMethod]
        public void Invalid()
        {
            TestEnum e = new TestEnum("foobar");

            Assert.AreEqual(TestEnum.Symbol.UNKNOWN, e.symbol);
        }

        [TestMethod]
        public void FullCycle()
        {
            TestEnum e = new TestEnum("foobar");

            TestEnum reclaimed = new TestEnum(e.Data());

            Assert.AreNotSame(e, reclaimed);
            Assert.AreEqual(e.symbol, reclaimed.symbol);
        }
    }
}