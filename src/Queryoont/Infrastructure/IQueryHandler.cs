using System;
using System.Threading.Tasks;

namespace Queryoont.Infrastructure
{
    public interface IQueryHandler
    {
        Task<string> Handle(string bodyStr, Type requestType);

    }
}