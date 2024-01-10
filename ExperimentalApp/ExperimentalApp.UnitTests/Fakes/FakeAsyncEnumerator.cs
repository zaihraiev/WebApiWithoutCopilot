using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests.Fakes
{
    internal class FakeAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public FakeAsyncEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public T Current => this.enumerator.Current;

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.Run(() => this.enumerator.Dispose()));
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(this.enumerator.MoveNext());
        }
    }
}
