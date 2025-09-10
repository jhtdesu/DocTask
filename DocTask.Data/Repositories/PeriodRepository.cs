using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;
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

    public async Task<(List<Period> items, int totalCount)> GetPeriodsPaginated(PaginationRequest request)
    {
        var query = _context.Periods.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(p => p.PeriodName.Contains(request.SearchTerm));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "name":
                    query = request.SortDescending ? query.OrderByDescending(p => p.PeriodName) : query.OrderBy(p => p.PeriodName);
                    break;
                case "createdat":
                    query = request.SortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt);
                    break;
                case "startdate":
                    query = request.SortDescending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate);
                    break;
                case "enddate":
                    query = request.SortDescending ? query.OrderByDescending(p => p.EndDate) : query.OrderBy(p => p.EndDate);
                    break;
                default:
                    query = query.OrderBy(p => p.PeriodId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(p => p.PeriodId);
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
