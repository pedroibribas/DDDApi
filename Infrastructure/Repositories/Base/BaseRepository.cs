using Domain.Interfaces.Repositories.Base;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infrastructure.Repositories.Base;

/// <summary>
/// Implementação da interface básica de repositório IBaseRepository e do padrão de descarte IDisposible.
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
{
    /// <summary>
    /// Configuração da base de dados injetada por DI.
    /// </summary>
    private readonly DbContextOptions<BaseDbContext> _optionsBuilder;
    /// <summary>
    /// Disponibiliza acesso à base de dados.
    /// </summary>
    public readonly DbContext _db;
    /// <summary>
    /// Disponibiliza acesso a uma tabela da base de dados.
    /// </summary>
    public readonly DbSet<T>? _dbSet;
    // To detect redundant calls
    private bool _disposedValue;
    // Instantiate a SafeHandle instance.
    private SafeHandle? _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

    public BaseRepository()
    {
        _optionsBuilder = new DbContextOptions<BaseDbContext>();
        _db = new BaseDbContext(_optionsBuilder);
    }

    public async Task<T> GetById(string id)
    {
        return await _db.Set<T>().FindAsync(id) ??
            throw new KeyNotFoundException(
                $"Nenhuma entidade encontrada; key={id}");
    }

    public async Task<List<T>> List()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task Add(T Object)
    {
        var entry = await _db.Set<T>().AddAsync(Object);
        Console.WriteLine(entry.ToString());
        await _db.SaveChangesAsync();
    }

    public async Task Update(T Object)
    {
        _db.Set<T>().Update(Object);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var entity = await _db.Set<T>().FindAsync(id) ??
            throw new KeyNotFoundException(
                $"Nenhuma entidade encontrada para exclusão; key={id}");

        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _safeHandle?.Dispose();
                _safeHandle = null;
            }

            _disposedValue = true;
        }
    }
}
