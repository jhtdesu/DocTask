using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Data;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Data.Repositories;

public class UploadFileRepository : IUploadFileRepository
{
    private readonly ApplicationDbContext _context;

    public UploadFileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Uploadfile> CreateAsync(Uploadfile uploadFile)
    {
        _context.Uploadfiles.Add(uploadFile);
        await _context.SaveChangesAsync();
        return uploadFile;
    }

    public async Task<Uploadfile?> GetByIdAsync(int fileId)
    {
        return await _context.Uploadfiles
            .Include(u => u.UploadedByNavigation)
            .FirstOrDefaultAsync(u => u.FileId == fileId);
    }

    public async Task<List<Uploadfile>> GetByUserIdAsync(string userId)
    {
        return await _context.Uploadfiles
            .Where(u => u.UploadedBy == userId)
            .OrderByDescending(u => u.UploadedAt)
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int fileId)
    {
        var uploadFile = await _context.Uploadfiles.FindAsync(fileId);
        if (uploadFile == null)
            return false;

        _context.Uploadfiles.Remove(uploadFile);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Uploadfile?> GetByIdAndUserIdAsync(int fileId, string userId)
    {
        return await _context.Uploadfiles
            .FirstOrDefaultAsync(u => u.FileId == fileId && u.UploadedBy == userId);
    }
}
