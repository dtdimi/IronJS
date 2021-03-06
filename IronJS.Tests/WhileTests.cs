﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IronJS.Tests
{
    [TestClass]
    public class WhileTests
    {
        [TestMethod]
        public void TestWhile()
        {
            Assert.AreEqual(
                "01234",
                ScriptRunner.Run(
                    "i = 0; while(i < 5) { emit(i); ++i; } "
                )
            );
        }

        [TestMethod]
        public void TestWhileBreak()
        {
            Assert.AreEqual(
                "01234",
                ScriptRunner.Run(
                    "i = 0; while(true) { if(i >= 5) break; emit(i); ++i; } "
                )
            );
        }

        [TestMethod]
        public void TestWhileContinue()
        {
            Assert.AreEqual(
                "1245",
                ScriptRunner.Run(
                    "i = 0; while(i < 5) { ++i; if(i == 3) continue; emit(i); } "
                )
            );
        }

        [TestMethod]
        public void TestWhileBreakLabelled()
        {
            Assert.AreEqual(
                "foo",
                ScriptRunner.Run(
                    " outer: while(true) { while(true) { emit('foo'); break outer; } emit('bar'); } "
                )
            );
        }

        [TestMethod]
        public void TestWhileContinueLabelled()
        {
            Assert.AreEqual(
                "foofoofoofoofoo",
                ScriptRunner.Run(
                    "i = 0; outer: while(i < 5) { while(true) { emit('foo'); ++i; continue outer; } emit('bar'); } "
                )
            );
        }

        [TestMethod]
        public void TestWhileWorksWithOtherTrueishValues()
        {
            Assert.AreEqual(
                "foo",
                ScriptRunner.Run(
                    "while(1) { emit('foo'); break; }"
                )
            );
        }

        [TestMethod]
        public void TestDoWhile()
        {
            Assert.AreEqual(
                "0",
                ScriptRunner.Run(
                    "i = 0; do { emit(i); } while(i < 0); "
                )
            );
        }

        [TestMethod]
        public void TestDoWhileBreak()
        {
            Assert.AreEqual(
                "0",
                ScriptRunner.Run(
                    "i = 0; do { emit(i); break; ++i; } while(i < 2); "
                )
            );
        }

        [TestMethod]
        public void TestDoWhileLabelledBreak()
        {
            Assert.AreEqual(
                "foo",
                ScriptRunner.Run(
                    "outer: do { do { emit('foo'); break outer; } while(true); } while(true); "
                )
            );
        }

        [TestMethod]
        public void TestDoWhileContinue()
        {
            Assert.AreEqual(
                "23",
                ScriptRunner.Run(
                    "i = 0; do { ++i; if(i == 1) continue; emit(i); } while(i < 3); "
                )
            );
        }

        [TestMethod]
        public void TestDoWhileLabelledContinue()
        {
            Assert.AreEqual(
                "foofoofoofoofoo",
                ScriptRunner.Run(
                    "i = 0; outer: do { ++i; if(i == 6) break; do { emit('foo'); continue outer; } while(true); } while(true); "
                )
            );
        }

        [TestMethod]
        public void TestDoWhileWorksWithOtherTrueishValues()
        {
            Assert.AreEqual(
                "foo",
                ScriptRunner.Run(
                    " do { emit('foo'); } while(0); "
                )
            );
        }
    }
}
