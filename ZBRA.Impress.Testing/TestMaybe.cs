﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ZBRA.Impress.Collections;

namespace ZBRA.Impress.Testing
{
    [TestClass]
    public class TestMayBe
    {

        [TestMethod]
        public void TestMaybeSingle()
        {
            var n = Enumerable.Empty<int>().Concat(1);

            var m = n.MaybeSingle();

            Assert.AreEqual(1, m.Value);

            n = Enumerable.Empty<int>().Concat(0);

            m = n.MaybeSingle(); // an enumerable of the default

            Assert.IsTrue(m.HasValue);

            n = Enumerable.Empty<int>();

            m = n.MaybeSingle();

            Assert.IsFalse(m.HasValue);

        }

        [TestMethod]
        public void TestMaybeSingleMoreElements()
        {
            var n = Enumerable.Empty<int>().Concat(2).Concat(1);

            var m = n.MaybeSingle();

            Assert.IsFalse(m.HasValue);
        }

        [TestMethod]
        public void TestMaybeFirst()
        {
            var n = Enumerable.Empty<string>().Concat("2");

            var m = n.MaybeFirst();

            Assert.AreEqual("2", m.Value);

            n = Enumerable.Empty<string>().Concat("2").Concat("3");

            m = n.MaybeFirst();

            Assert.AreEqual("2", m.Value);

            n = Enumerable.Empty<string>();

            m = n.MaybeFirst();

            Assert.IsFalse(m.HasValue);
        }

        [TestMethod]
        public void TestMaybeOfObject()
        {
            Maybe<Maybe<string>> ahg = Maybe<Maybe<string>>.Nothing;

            Assert.IsFalse(ahg.HasValue);

            object a = "a".ToMaybe();

            var u = a.ToMaybe();

            Assert.IsTrue(u.HasValue);
            Assert.IsFalse(u.Value is Maybe<string>);
            Assert.AreEqual("a", u.Value);

            Maybe<string> s = a.ToMaybe().MaybeCast<object, string>();

            Assert.IsTrue(s.HasValue);
            Assert.AreEqual("a", s.Value);

            Maybe<string> y = 2.ToMaybe().MaybeCast<object, string>();

            Assert.IsFalse(y.HasValue);
        }

        [TestMethod]
        public void TestMaybeEnum()
        {
            var a = 2;

            var m = a.ToMaybe().MaybeEnum<TestPhase>();

            Assert.IsNotNull(m);
            Assert.IsTrue(m.HasValue);
            Assert.AreEqual(TestPhase.Configured, m.Value);

            var r = a.ToMaybe().MaybeEnum<long>();

            Assert.IsNotNull(r);
            Assert.IsFalse(r.HasValue);

            var s = TestPhase.Completed.ToString().ToMaybe().MaybeEnum<TestPhase>();

            Assert.IsNotNull(s);
            Assert.IsTrue(s.HasValue);
            Assert.AreEqual(TestPhase.Completed, s.Value);

            var n = "notanEnumValue".ToMaybe().MaybeEnum<TestPhase>();

            Assert.IsNotNull(n);
            Assert.IsFalse(n.HasValue);

            var h = 1.ToMaybe().MaybeEnum<TestPhase>();

            Assert.IsNotNull(h);
            Assert.IsTrue(h.HasValue);
            Assert.AreEqual(TestPhase.Equality, h.Value);

        }

        [TestMethod]
        public void TestMaybeMaybe()
        {
            string a = "";

            Maybe<String> m = a.ToMaybe();

            var u = m.ToMaybe(); // obsolete but its beeing checked here

            Assert.IsFalse(m.HasValue);
            Assert.IsFalse(u.HasValue);
        }

        [TestMethod]
        public void TestConvert()
        {
            Assert.AreEqual(2, "2".ToMaybe().Convert<int>().Value);
            Assert.AreEqual(2L, "2".ToMaybe().Convert<long>().Value);
            Assert.AreEqual(2.0, "2.0".ToMaybe().Convert<double>().Value);
            Assert.AreEqual(2.0M, "2.0".ToMaybe().Convert<decimal>().Value);
            Assert.IsTrue("true".ToMaybe().Convert<bool>().Value);

            Assert.AreEqual(Maybe<int>.Nothing, "a".ToMaybe().Convert<int>());
            Assert.AreEqual(Maybe<long>.Nothing, "a".ToMaybe().Convert<long>());
            Assert.AreEqual(Maybe<double>.Nothing, "a".ToMaybe().Convert<double>());
            Assert.AreEqual(Maybe<decimal>.Nothing, "a".ToMaybe().Convert<decimal>());
            Assert.IsFalse("a".ToMaybe().Convert<bool>().Value);
        }


        [TestMethod]
        public void TestCast()
        {
            Assert.AreEqual(2L, 2L.MaybeCast<long, long>().Value);
            Assert.AreEqual(2L, 2L.MaybeCast<object, long>().Value);
            Assert.AreEqual(2.5, (2.5d).MaybeCast<double, double>().Value);
            Assert.AreEqual(2.0M, (2.0m).MaybeCast<decimal, decimal>().Value);
            Assert.AreEqual(true, true.MaybeCast<bool, bool>().Value);
            Assert.IsTrue(new B().MaybeCast<A, B>().HasValue);

            object obj = 2L;
            object n = null;

            Assert.AreEqual(2L, obj.MaybeCast<object, long>().Value);
            Assert.IsFalse(n.MaybeCast<object, bool>().HasValue);
            Assert.IsFalse(n.MaybeCast<object, long>().HasValue);
            Assert.IsFalse(n.MaybeCast<object, A>().HasValue);

            Maybe<B> b = new A().MaybeCast<A, B>();

            Assert.IsFalse(b.HasValue);
        }

        class A { }

        class B : A { }

        [TestMethod]
        public void TestNullToMaybe()
        {
            string a = null;

            Maybe<String> m = a.ToMaybe();

            Assert.IsFalse(m.HasValue);
        }

        [TestMethod]
        public void TestValueToMaybe()
        {
            string a = "test";

            Maybe<String> m = a.ToMaybe();

            Assert.IsTrue(m.HasValue);
            Assert.AreEqual(a, m.Value);
        }

        [TestMethod]
        public void TestEmptyStringToMaybe()
        {
            string a = "";

            Maybe<String> m = a.ToMaybe();

            Assert.IsFalse(m.HasValue);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void TestCallEmptyValueMaybe()
        {
            string a = "";

            Maybe<String> m = a.ToMaybe();

            var value = m.Value; // expected exception
        }
        [TestMethod]
        public void TestStrutToMaybe()
        {
            int a = 1;

            Maybe<int> m = a.ToMaybe();

            Assert.IsTrue(m.HasValue);
            Assert.AreEqual(a, m.Value);
        }

        [TestMethod]
        public void TestStrutToNothingMaybe()
        {

            Maybe<int> m = Maybe<int>.Nothing;

            Assert.IsFalse(m.HasValue);

        }

        [TestMethod]
        public void TestNullableToMaybe()
        {
            int? a = 2;

            Maybe<int> m = a.ToMaybe();

            Assert.IsTrue(m.HasValue);
            Assert.AreEqual(a, m.Value);
        }

        [TestMethod]
        public void TestMaybeOperations()
        {
            var r = 5.ToMaybe().SelectMany(
             x => 6.ToMaybe().SelectMany(
                 y => (x + y).ToMaybe()));

            Assert.IsTrue(r.HasValue);
            Assert.AreEqual(11, r.Value);

            var s = 5.ToMaybe().SelectMany(x => 6.ToMaybe(), (x, y) => x + y);

            Assert.IsTrue(r.HasValue);
            Assert.AreEqual(11, r.Value);

            var t = from x in 5.ToMaybe()
                    from y in 6.ToMaybe()
                    select x + y;

            Assert.IsTrue(t.HasValue);
            Assert.AreEqual(11, r.Value);

            var w = from x in 5.ToMaybe()
                    from y in Maybe<int>.Nothing
                    select x + y;

            Assert.IsFalse(w.HasValue);
        }


        [TestMethod]
        public void TestTelescopicOperatorToMaybe()
        {
            string a = null;

            Maybe<int> m = a.ToMaybe().Select(s => s.Length);

            Assert.IsFalse(m.HasValue);

            a = "Test";

            m = a.ToMaybe().Select(s => s.Length);

            Assert.IsTrue(m.HasValue);
            Assert.AreEqual(4, m.Value);
        }

        [TestMethod]
        public void TestWithAlternative()
        {
            var m = "".ToMaybe().WithAlternative("b");

            Assert.IsTrue(m.HasValue);
            Assert.AreEqual("b", m.Value);

            var n = "c".ToMaybe().WithAlternative("d");

            Assert.IsTrue(n.HasValue);
            Assert.AreEqual("c", n.Value);

            var o = "e".ToMaybe().WithAlternative("".ToMaybe());

            Assert.IsTrue(o.HasValue);
            Assert.AreEqual("e", o.Value);
        }

        [TestMethod]
        public void TestMaybeAsDictionarKeyy()
        {
            var dictionary = new Dictionary<Maybe<string>, string>();
            dictionary.Add("a".ToMaybe(), "a");

            dictionary.Add(Maybe<string>.Nothing, "nothing");

            Assert.AreEqual("a", dictionary["a".ToMaybe()]);
        }

        [TestMethod]
        public void TestMaybeGetForDifferentDictionaryInterfaces()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("a", "a");

            Assert.AreEqual("a", dictionary.MaybeGet("a").Value);

            IDictionary<string, string> idictionary = dictionary;

            Assert.AreEqual("a", idictionary.MaybeGet("a").Value);

            IReadOnlyDictionary<string, string> readOnlyDictionary = dictionary;

            Assert.AreEqual("a", readOnlyDictionary.MaybeGet("a").Value);

            var list = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("b", "b") };

            Assert.AreEqual("b", list.MaybeGet("b").Value);


            Dictionary<string, Maybe<string>> dictionary2 = new Dictionary<string, Maybe<string>>();
            dictionary2.Add("a", "a".ToMaybe());

            Assert.AreEqual("a", dictionary2.MaybeGet("a").Value);

            IDictionary<string, Maybe<string>> idictionary2 = dictionary2;

            Assert.AreEqual("a", idictionary2.MaybeGet("a").Value);

            IReadOnlyDictionary<string, Maybe<string>> readOnlyDictionary2 = dictionary2;

            Assert.AreEqual("a", readOnlyDictionary2.MaybeGet("a").Value);

            var list2 = new List<KeyValuePair<string, Maybe<string>>> { new KeyValuePair<string, Maybe<string>>("b", "b".ToMaybe()) };

            Assert.AreEqual("b", list2.MaybeGet("b").Value);
        }


        [TestMethod]
        public void TestMaybeNothingEquals()
        {
            var a = Maybe<string>.Nothing;
            var b = "".ToMaybe();
            var c = Maybe<int>.Nothing;

            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);

        }
    }


    public enum TestPhase
    {
        Completed,
        Equality,
        Configured,

    }
}
