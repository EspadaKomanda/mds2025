using MDSBackend.Models.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace MDSBackend.Database.Repositories;

public class UnitOfWork : IDisposable
{
    #region  fields
    
    private ApplicationContext _context;
    private GenericRepository<UserProfile> _userProfileRepository;
    private GenericRepository<ApplicationRole> _roleRepository;
    private GenericRepository<Right?> _rightRepository;
    private GenericRepository<RefreshToken> _refreshTokenRepository;
    private GenericRepository<RoleRight> _roleRightRepository;
    private GenericRepository<UserRole> _userRoleRepository;
    private GenericRepository<Instruction> _instructionRepository;
    private GenericRepository<InstructionParagraph> _instructionParagraphRepository;
    private GenericRepository<InstructionCategory> _instructionCategoryRepository;
    private GenericRepository<InstructionTest> _instructionTestRepository;
    private GenericRepository<InstructionTestQuestion> _instructionTestQuestionRepository;
    private GenericRepository<InstructionTestResult> _instructionTestResultRepository;
    
    #endregion
    
    
    private IDbContextTransaction _transaction;
    
    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }


    #region Properties

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
    
    public GenericRepository<ApplicationRole> RoleRepository
    {
        get
        {
            if (this._roleRepository == null)
            {
                this._roleRepository = new GenericRepository<ApplicationRole>(_context);
            }
            return _roleRepository;
        }
    }
    
    public GenericRepository<Right?> RightRepository
    {
        get
        {
            if (this._rightRepository == null)
            {
                this._rightRepository = new GenericRepository<Right?>(_context);
            }
            return _rightRepository;
        }
    }
    
    public GenericRepository<RefreshToken> RefreshTokenRepository
    {
        get
        {
            if (this._refreshTokenRepository == null)
            {
                this._refreshTokenRepository = new GenericRepository<RefreshToken>(_context);
            }
            return _refreshTokenRepository;
        }
    }
    
    public GenericRepository<RoleRight> RoleRightRepository
    {
        get
        {
            if (this._roleRightRepository == null)
            {
                this._roleRightRepository = new GenericRepository<RoleRight>(_context);
            }
            return _roleRightRepository;
        }
    }
    
    public GenericRepository<UserRole> UserRoleRepository
    {
        get
        {
            if (this._userRoleRepository == null)
            {
                this._userRoleRepository = new GenericRepository<UserRole>(_context);
            }
            return _userRoleRepository;
        }
    }
    
    public GenericRepository<Instruction> InstructionRepository
    {
        get
        {
            if (this._instructionRepository == null)
            {
                this._instructionRepository = new GenericRepository<Instruction>(_context);              
            }            
            return _instructionRepository;
        }
    }
    
    public GenericRepository<InstructionParagraph> InstructionParagraphRepository
    {
        get
        {
            if (this._instructionParagraphRepository == null)
            {
                this._instructionParagraphRepository = new GenericRepository<InstructionParagraph>(_context);              
            }            
            return _instructionParagraphRepository;
        }
    }

    public GenericRepository<InstructionCategory> InstructionCategoryRepository
    {
        get
        {
            if (this._instructionCategoryRepository == null)
            {
                this._instructionCategoryRepository = new GenericRepository<InstructionCategory>(_context);              
            }            
            return _instructionCategoryRepository;
        }
    }

    public GenericRepository<InstructionTest> InstructionTestRepository
    {
        get
        {
            if (this._instructionTestRepository == null)
            {
                this._instructionTestRepository = new GenericRepository<InstructionTest>(_context);              
            }            
            return _instructionTestRepository;
        }
    }

    public GenericRepository<InstructionTestQuestion> InstructionTestQuestionRepository
    {
        get
        {
            if (this._instructionTestQuestionRepository == null)
            {
                this._instructionTestQuestionRepository = new GenericRepository<InstructionTestQuestion>(_context);              
            }            
            return _instructionTestQuestionRepository;
        }
    }

    public GenericRepository<InstructionTestResult> InstructionTestResultRepository
    {
        get
        {
            if (this._instructionTestResultRepository == null)
            {
                this._instructionTestResultRepository = new GenericRepository<InstructionTestResult>(_context);              
            }            
            return _instructionTestResultRepository;
        }
    }

    #endregion
 

    

    public bool Save()
    {
       return _context.SaveChanges() > 0;
    }

    public async Task<bool> SaveAsync()
    {
       return await _context.SaveChangesAsync() > 0;
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
