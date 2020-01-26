using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    class TestClass{}
    [TestFixture]
    public class JsonTypeHelperTests
    {
        [TestCase(typeof(bool), "boolean")]
        [TestCase(typeof(byte), "integer")]
        [TestCase(typeof(sbyte), "integer")]
        [TestCase(typeof(char), "string")]
        [TestCase(typeof(DateTime), "string")]
        [TestCase(typeof(decimal), "number")]
        [TestCase(typeof(double), "number")]
        [TestCase(typeof(Int16), "integer")]
        [TestCase(typeof(Int32), "integer")]
        [TestCase(typeof(Int64), "integer")]
        [TestCase(typeof(Single), "number")]
        [TestCase(typeof(string), "string")]
        [TestCase(typeof(UInt16), "integer")]
        [TestCase(typeof(UInt32), "integer")]
        [TestCase(typeof(UInt64), "integer")]
        [TestCase(typeof(Task), "void")]
        [TestCase(typeof(Task<TestClass>), "object")]
        [TestCase(typeof(TestClass), "object")]
        [TestCase(typeof(IEnumerable<object>), "array")]
        [TestCase(typeof(List<TestClass>), "array")]
        public void GetSchemaTypeString_Valid_ReturnsTypeString(Type type, string expectedString)
        {
            // ARRANGE
            
            // ACT
            var result = JsonTypeHelper.GetSchemaTypeString(type);

            // ASSERT
            Assert.That(result, Is.EqualTo(expectedString));
        }
    }
}