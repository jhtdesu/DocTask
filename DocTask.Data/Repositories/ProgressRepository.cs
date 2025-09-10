using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Data;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Data.Repositories;

public class ProgressRepository : IProgressRepository
{
    private readonly ApplicationDbContext _context;

    public ProgressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Progress>> GetAllProgress(DateTime? startDate = null, DateTime? dueDate = null)
    {
        var query = _context.Progresses
            .Include(p => p.Task)
            .Include(p => p.Period)
            .Include(p => p.UpdatedByNavigation)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(p => p.Task.StartDate >= startDate.Value);
        }

        if (dueDate.HasValue)
        {
            query = query.Where(p => p.Task.DueDate <= dueDate.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<(List<Progress> items, int totalCount)> GetProgressPaginated(PaginationRequest request, DateTime? startDate = null, DateTime? dueDate = null)
    {
        var query = _context.Progresses
            .Include(p => p.Task)
            .Include(p => p.Period)
            .Include(p => p.UpdatedByNavigation)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(p => p.Task.StartDate >= startDate.Value);
        }

        if (dueDate.HasValue)
        {
            query = query.Where(p => p.Task.DueDate <= dueDate.Value);
        }

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(p => p.Task.Title.Contains(request.SearchTerm) || 
                                   (p.Comment != null && p.Comment.Contains(request.SearchTerm)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "taskname":
                    query = request.SortDescending ? query.OrderByDescending(p => p.Task.Title) : query.OrderBy(p => p.Task.Title);
                    break;
                case "updatedat":
                    query = request.SortDescending ? query.OrderByDescending(p => p.UpdatedAt) : query.OrderBy(p => p.UpdatedAt);
                    break;
                case "progresspercentage":
                    query = request.SortDescending ? query.OrderByDescending(p => p.PercentageComplete) : query.OrderBy(p => p.PercentageComplete);
                    break;
                case "duedate":
                    query = request.SortDescending ? query.OrderByDescending(p => p.Task.DueDate) : query.OrderBy(p => p.Task.DueDate);
                    break;
                default:
                    query = query.OrderBy(p => p.ProgressId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(p => p.ProgressId);
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

    public async Task<Progress?> GetProgressById(int id)
    {
        return await _context.Progresses
            .Include(p => p.Task)
            .Include(p => p.Period)
            .Include(p => p.UpdatedByNavigation)
            .FirstOrDefaultAsync(p => p.ProgressId == id);
    }


    public async Task<Progress> CreateProgress(Progress progress)
    {
        _context.Progresses.Add(progress);
        await _context.SaveChangesAsync();
        return progress;
    }

    public async Task<Progress?> UpdateProgress(Progress progress)
    {
        var existingProgress = await _context.Progresses.FindAsync(progress.ProgressId);
        if (existingProgress == null)
            return null;

        _context.Entry(existingProgress).CurrentValues.SetValues(progress);
        await _context.SaveChangesAsync();
        return existingProgress;
    }

    public async Task<bool> DeleteProgress(int id)
    {
        var progress = await _context.Progresses.FindAsync(id);
        if (progress == null)
            return false;

        _context.Progresses.Remove(progress);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ProgressExists(int id)
    {
        return await _context.Progresses.AnyAsync(p => p.ProgressId == id);
    }
}
