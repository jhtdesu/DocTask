using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
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
