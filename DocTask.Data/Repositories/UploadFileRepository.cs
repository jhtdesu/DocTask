using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;
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

    public async Task<(List<Uploadfile> items, int totalCount)> GetByUserIdPaginated(string userId, PaginationRequest request)
    {
        var query = _context.Uploadfiles
            .Where(u => u.UploadedBy == userId);

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(u => u.FileName.Contains(request.SearchTerm));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "filename":
                    query = request.SortDescending ? query.OrderByDescending(u => u.FileName) : query.OrderBy(u => u.FileName);
                    break;
                case "uploadedat":
                    query = request.SortDescending ? query.OrderByDescending(u => u.UploadedAt) : query.OrderBy(u => u.UploadedAt);
                    break;
                case "filesize":
                    query = request.SortDescending ? query.OrderByDescending(u => u.FileName.Length) : query.OrderBy(u => u.FileName.Length);
                    break;
                case "contenttype":
                    query = request.SortDescending ? query.OrderByDescending(u => u.FileName) : query.OrderBy(u => u.FileName);
                    break;
                default:
                    query = query.OrderByDescending(u => u.UploadedAt);
                    break;
            }
        }
        else
        {
            query = query.OrderByDescending(u => u.UploadedAt);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (items, totalCount);
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
