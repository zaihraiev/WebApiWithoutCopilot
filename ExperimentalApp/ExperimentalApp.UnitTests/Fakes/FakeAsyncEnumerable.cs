using MockQueryable.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests.Fakes
{
    internal class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public FakeAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token)
        {
            return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }
}
