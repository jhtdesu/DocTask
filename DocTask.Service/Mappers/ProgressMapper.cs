using DocTask.Core.Dtos.Progress;
using DocTask.Core.Models;

namespace DocTask.Service.Mappers;

public static class ProgressMapper
{
    public static ProgressDto ToDto(Progress progress)
    {
        return new ProgressDto
        {
            ProgressId = progress.ProgressId,
            TaskId = progress.TaskId,
            PeriodId = progress.PeriodId,
            PercentageComplete = progress.PercentageComplete,
            Comment = progress.Comment,
            Status = progress.Status,
            UpdatedBy = progress.UpdatedBy,
            UpdatedAt = progress.UpdatedAt,
            FileName = progress.FileName,
            FilePath = progress.FilePath,
            Proposal = progress.Proposal,
            Result = progress.Result,
            Feedback = progress.Feedback,
            PeriodName = progress.Period?.PeriodName,
            TaskName = progress.Task?.Title,
            UpdatedByUserName = progress.UpdatedByNavigation?.UserName
        };
    }

    public static List<ProgressDto> ToDtoList(List<Progress> progressList)
    {
        return progressList.Select(ToDto).ToList();
    }
}
