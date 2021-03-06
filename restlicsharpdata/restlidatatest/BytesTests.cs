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

using restlicsharpdata.restlidata;

namespace restlicsharpdata.restlidatatest
{
    [TestClass]
    public class BytesTests
    {
        [TestMethod]
        public void Init()
        {
            byte[] expected = new byte[] { 0, 1, 2, 3 };
            string input = "\u0000\u0001\u0002\u0003";

            try
            {
                Bytes output = new Bytes(BytesUtil.StringToBytes(input));
            }
            catch
            {
                Assert.Fail("Exception while instantiating Bytes object.");
            }
        }

        [TestMethod]
        public void Immutable()
        {
            string input = "\u0000\u0001\u0002\u0003";

            Bytes output = new Bytes(BytesUtil.StringToBytes(input));

            byte[] copy = output.GetBytes();
            const byte VALUE = 0x00ff;
            copy[0] = VALUE;
            CollectionAssert.AreNotEqual(copy, output.GetBytes());

            byte[] copy2 = output.GetBytes();
            const byte VALUE2 = 0x00ee;
            copy2[0] = VALUE2;
            CollectionAssert.AreNotEqual(copy, output.GetBytes());

            // Test that both copies are separate copies
            Assert.AreNotSame(copy2, copy);
            CollectionAssert.AreNotEqual(copy2, copy);
        }

        [TestMethod]
        public void CopyInitArg()
        {
            string input = "\u0000\u0001\u0002\u0003";
            byte[] inputBytes = BytesUtil.StringToBytes(input);

            Bytes output = new Bytes(inputBytes);

            Assert.AreNotSame(inputBytes, output.GetBytes());

            byte VALUE = 0x00ff;
            inputBytes[0] = VALUE;
            Assert.AreNotEqual(VALUE, output.GetBytes()[0]);
        }
    }
}