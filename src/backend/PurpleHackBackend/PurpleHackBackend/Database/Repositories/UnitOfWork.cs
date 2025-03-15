using Microsoft.EntityFrameworkCore.Storage;
using PurpleHackBackend.Models.Database;

namespace PurpleHackBackend.Database.Repositories;

public class UnitOfWork : IDisposable
{
    private ApplicationContext _context;
    private GenericRepository<User> _userRepository;
    private GenericRepository<UserProfile> _userProfileRepository;
    private GenericRepository<Role> _roleRepository;
    private IDbContextTransaction _transaction;
    
    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }
    
    public GenericRepository<User> UserRepository
    {
        get
        {
            if (this._userRepository == null)
            {
                this._userRepository = new GenericRepository<User>(_context);
            }
            return _userRepository;
        }
    }

    public GenericRepository<UserProfile> UserProfileRepository
    {
        get
        {
            if (this._userProfileRepository == null)
            {
                this._userProfileRepository = new GenericRepository<UserProfile>(_context);
            }
            return _userProfileRepository;
        }
    }

    public GenericRepository<Role> RoleRepository
    {
        get
        {
            if (this._roleRepository == null)
            {
                this._roleRepository = new GenericRepository<Role>(_context);
            }
            return _roleRepository;
        }
    }
    

    public void Save()
    {
        _context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }
    public async Task BeginTransactionAsync()
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction has already been started.");
        _transaction = await _context.Database.BeginTransactionAsync();
    }
    public async Task CommitAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("A transaction has not been started.");
    
        try
        {
            await _transaction.CommitAsync();
            _transaction.Dispose();
            _transaction = null;
        }
        catch (Exception)
        {
            if (_transaction is not null)
                await _transaction.RollbackAsync();
            throw;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
