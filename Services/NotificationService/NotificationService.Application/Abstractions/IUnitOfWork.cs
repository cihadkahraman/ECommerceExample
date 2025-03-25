﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        INotificationLogRepository NotificationLogs { get; }
        ICustomerRepository Customers { get; }
    }
}
