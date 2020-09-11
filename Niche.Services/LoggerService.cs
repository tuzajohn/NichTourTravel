using Niche.Core.Enums;
using Niche.Core.ExtensionHelpers;
using Niche.Core.IServices;
using Niche.Core.Models;
using Niche.Core.Repositories;

namespace Niche.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IDataManager<Log> _repo;
        public LoggerService(IDataManager<Log> repo)
        {
            _repo = repo;
        }
        public void Log(LogLevel level, string message)
        {
            var log = new Log
            {
                Id = Helpers.GetID(),
                Level = level,
                Message = message
            };
            _repo.Add(log);
        }
    }
}
