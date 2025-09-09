using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Data.Repositories;

public class PeriodRepository : IPeriodRepository
{
    private readonly ApplicationDbContext _context;

    public PeriodRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Period>> GetAllPeriods()
    {
        return await _context.Periods.ToListAsync();
    }

    public async Task<Period?> GetPeriodById(int id)
    {
        return await _context.Periods.FindAsync(id);
    }

    public async Task<Period> CreatePeriod(Period period)
    {
        period.CreatedAt = DateTime.UtcNow;
        _context.Periods.Add(period);
        await _context.SaveChangesAsync();
        return period;
    }

    public async Task<Period?> UpdatePeriod(Period period)
    {
        var existingPeriod = await _context.Periods.FindAsync(period.PeriodId);
        if (existingPeriod == null)
            return null;

        _context.Entry(existingPeriod).CurrentValues.SetValues(period);
        await _context.SaveChangesAsync();
        return existingPeriod;
    }

    public async Task<bool> DeletePeriod(int id)
    {
        var period = await _context.Periods.FindAsync(id);
        if (period == null)
            return false;

        _context.Periods.Remove(period);
        await _context.SaveChangesAsync();
        return true;
    }
}
