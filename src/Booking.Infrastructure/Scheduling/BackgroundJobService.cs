using Booking.ApplicationServices.Abstractions.Scheduling;
using Hangfire;
using System.Linq.Expressions;

namespace Booking.Infrastructure.Scheduling
{
    public class BackgroundJobService : IBackgroundJobService
    {
        public string Enqueue<T>(Expression<Func<T, Task>> job)
        {
            return BackgroundJob.Enqueue(job);
        }

        public bool Requeue(string jobId)
        {
            return BackgroundJob.Requeue(jobId);
        }

        public void SetRecurringJob(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        }

        public void TriggerRecurringJobNow(string recurringJobId)
        {
            RecurringJob.TriggerJob(recurringJobId);
        }
    }
}
