using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using DocTask.Core.DTOs.ApiResponses;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Data.Repositories;

public class FrequencyRepository : IFrequencyRepository
{
    private readonly ApplicationDbContext _context;

    public FrequencyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Frequency>> GetAllFrequencies()
    {
        return await _context.Frequencies.ToListAsync();
    }

    public async Task<(List<Frequency> items, int totalCount)> GetFrequenciesPaginated(PaginationRequest request)
    {
        var query = _context.Frequencies.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(f => f.FrequencyType.Contains(request.SearchTerm) || 
                                   (f.FrequencyDetail != null && f.FrequencyDetail.Contains(request.SearchTerm)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "name":
                    query = request.SortDescending ? query.OrderByDescending(f => f.FrequencyType) : query.OrderBy(f => f.FrequencyType);
                    break;
                case "createdat":
                    query = request.SortDescending ? query.OrderByDescending(f => f.CreatedAt) : query.OrderBy(f => f.CreatedAt);
                    break;
                default:
                    query = query.OrderBy(f => f.FrequencyId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(f => f.FrequencyId);
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

    public async Task<Frequency?> GetFrequencyById(int id)
    {
        return await _context.Frequencies.FindAsync(id);
    }

    public async Task<Frequency> CreateFrequency(Frequency frequency)
    {
        frequency.CreatedAt = DateTime.UtcNow;
        _context.Frequencies.Add(frequency);
        await _context.SaveChangesAsync();
        return frequency;
    }

    public async Task<Frequency?> UpdateFrequency(Frequency frequency)
    {
        var existingFrequency = await _context.Frequencies.FindAsync(frequency.FrequencyId);
        if (existingFrequency == null)
            return null;

        _context.Entry(existingFrequency).CurrentValues.SetValues(frequency);
        await _context.SaveChangesAsync();
        return existingFrequency;
    }

    public async Task<bool> DeleteFrequency(int id)
    {
        var frequency = await _context.Frequencies.FindAsync(id);
        if (frequency == null)
            return false;

        _context.Frequencies.Remove(frequency);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<FrequencyDetail>> GetFrequencyDetailsByFrequencyId(int frequencyId)
    {
        return await _context.FrequencyDetails
            .Where(fd => fd.FrequencyId == frequencyId)
            .ToListAsync();
    }

    public async Task<(List<FrequencyDetail> items, int totalCount)> GetFrequencyDetailsPaginated(int frequencyId, PaginationRequest request)
    {
        var query = _context.FrequencyDetails
            .Where(fd => fd.FrequencyId == frequencyId);

        // Apply search filter
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(fd => (fd.DayOfWeek.HasValue && fd.DayOfWeek.ToString().Contains(request.SearchTerm)) || 
                                   (fd.DayOfMonth.HasValue && fd.DayOfMonth.ToString().Contains(request.SearchTerm)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "dayofweek":
                    query = request.SortDescending ? query.OrderByDescending(fd => fd.DayOfWeek) : query.OrderBy(fd => fd.DayOfWeek);
                    break;
                case "dayofmonth":
                    query = request.SortDescending ? query.OrderByDescending(fd => fd.DayOfMonth) : query.OrderBy(fd => fd.DayOfMonth);
                    break;
                default:
                    query = query.OrderBy(fd => fd.Id);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(fd => fd.Id);
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

    public async Task<FrequencyDetail> CreateFrequencyDetail(FrequencyDetail frequencyDetail)
    {
        _context.FrequencyDetails.Add(frequencyDetail);
        await _context.SaveChangesAsync();
        return frequencyDetail;
    }

    public async Task<bool> DeleteFrequencyDetail(int id)
    {
        var frequencyDetail = await _context.FrequencyDetails.FindAsync(id);
        if (frequencyDetail == null)
            return false;

        _context.FrequencyDetails.Remove(frequencyDetail);
        await _context.SaveChangesAsync();
        return true;
    }
}
