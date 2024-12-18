using System.Linq.Expressions;

namespace Booking.ApplicationServices.Abstractions.Scheduling
{
    public interface IBackgroundJobService
    {
        void SetRecurringJob(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression);
        void TriggerRecurringJobNow(string recurringJobId);
        string Enqueue<T>(Expression<Func<T, Task>> job);
    }
}
