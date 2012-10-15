// Copyright (c) 2012, Event Store LLP
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
// 
// Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.
// Redistributions in binary form must reproduce the above copyright
// notice, this list of conditions and the following disclaimer in the
// documentation and/or other materials provided with the distribution.
// Neither the name of the Event Store LLP nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
using System;
using System.IO;
using EventStore.Core.TransactionLog.Chunks;
using EventStore.Core.TransactionLog.LogRecords;
using NUnit.Framework;

namespace EventStore.Core.Tests.TransactionLog.Chunks
{
    [TestFixture]
    public class when_opening_existing_tfchunk
    {
        private readonly string _filename = Path.Combine(Path.GetTempPath(), "foo");
        private TFChunk _chunk;
        private TFChunk _testChunk;

        [SetUp]
        public void Setup()
        {
            _chunk = TFChunk.CreateNew(_filename, 4096, 0, 0);
            _chunk.Complete();
            _testChunk = TFChunk.FromCompletedFile(_filename, verifyHash: true);
        }

        [Test]
        public void the_chunk_is_not_cached()
        {
            Assert.IsFalse(_testChunk.IsCached);
        }

        [Test]
        public void the_chunk_is_readonly()
        {
            Assert.IsTrue(_testChunk.IsReadOnly);
        }

        [Test]
        public void append_throws_invalid_operation_exception()
        {
            Assert.Throws<InvalidOperationException>(() => _testChunk.TryAppend(new CommitLogRecord(0, Guid.NewGuid(), 0, DateTime.UtcNow, 0)));
        }

        [Test]
        public void flush_throws_invalid_operation_exception()
        {
            Assert.Throws<InvalidOperationException>(() => _testChunk.Flush());
        }

        [TearDown]
        public void TearDown()
        {
            _chunk.Dispose();
            _testChunk.Dispose();
            File.Delete(_filename);
        }
    }
}