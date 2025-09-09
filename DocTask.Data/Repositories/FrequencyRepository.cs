using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
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
