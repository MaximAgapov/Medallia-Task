using MedalliaTask.Application.Common.Interfaces;
using System;

namespace MedalliaTask.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}