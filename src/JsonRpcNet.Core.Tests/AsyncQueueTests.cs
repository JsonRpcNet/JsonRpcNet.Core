using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    [TestFixture]
    public class AsyncQueueTests
    {
        private AsyncQueue<object> _queue;
        
        [SetUp]
        public void SetUp()
        {
            _queue = new AsyncQueue<object>();
            
        }

        [Test, Category("Unit")]
        public void Enqueue_Item_ItemEnqueued()
        {
            // ARRANGE
            var item = "testItem";
            
            // ACT
            _queue.Enqueue(item);
            
            // ASSERT
            var dequeuedItem = _queue.DequeueAsync().GetAwaiter().GetResult();
            
            Assert.That(item, Is.EqualTo(dequeuedItem));
        }

        [Test, Category("Unit")]
        public void Dequeue_NothingInQueue_Waits()
        {
            // ARRANGE
            
            // ACT
            var listenerTask = Task.Run(() => 
                _queue.DequeueAsync().GetAwaiter().GetResult());

            _queue.Enqueue("test");
            
            // ASSERT
            var itemDequeued = listenerTask.Wait(100);
            Assert.That(itemDequeued, Is.True);
            Assert.That(listenerTask.IsCompleted, Is.True);
            Assert.That(listenerTask.Result, Is.EqualTo("test"));
        }
    }
    
}