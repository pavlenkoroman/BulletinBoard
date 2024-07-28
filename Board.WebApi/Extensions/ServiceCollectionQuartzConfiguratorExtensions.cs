using Board.Infrastructure.Jobs;
using Quartz;

namespace Board.WebApi.Extensions;

public static class ServiceCollectionQuartzConfiguratorExtensions
{
    public static void ScheduleDeactivatingExpiredBulletins(this IServiceCollectionQuartzConfigurator configurator)
    {
        configurator.ScheduleJob<BulletinExpirationJob>(trigger => trigger
            .WithIdentity("CheckBulletinExpirationJobTrigger")
            .WithCronSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 1)));
    }
}
