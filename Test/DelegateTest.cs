using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class DelegateTest
    {
        [TestMethod]
        public void TestDelegate()
        {
            var sut = new List("chicken", "applesauce", "banana");

            sut.Transform((s) => s.PadRight(12, 'h'));

            Console.WriteLine(sut.ToString());
        }
    }

    public class List
    {
        public string _a;
        public string _b;
        public string _c;

        public delegate string Transformer(string s);

        //public Func<string, string> TransformMe;

        public List(string a, string b, string c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public void Transform(Transformer transformer)
        {
            _a = transformer(_a);
            _b = transformer(_b);
            _c = transformer(_c);
        }
    }
}
