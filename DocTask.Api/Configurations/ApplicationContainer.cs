using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Data.Repositories;
using DocTask.Service.Services;

namespace DockTask.Api.Configurations;

public static class ApplicationContainer
{
    public static IServiceCollection AddApplicationContainer(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUploadFileService, UploadFileService>();
        services.AddScoped<IUploadFileRepository, UploadFileRepository>();
        services.AddScoped<IProgressService, ProgressService>();
        services.AddScoped<IProgressRepository, ProgressRepository>();
        return services;
    }
    
    
}