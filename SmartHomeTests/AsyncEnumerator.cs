using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

public class AsyncEnumerator<T> : IAsyncEnumerator<T>, IDbAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public AsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    object IDbAsyncEnumerator.Current => Current;

    public void Dispose()
    {
        _inner.Dispose();
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        return await Task.FromResult(_inner.MoveNext());
    }

    public async Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        return await Task.FromResult(_inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    Task<bool> IDbAsyncEnumerator.MoveNextAsync(CancellationToken cancellationToken)
    {
        return MoveNext(cancellationToken);
    }
}
