using System.Collections.Generic;
using System.Threading.Tasks;
using SqlKata;

namespace Queryoont.Interfaces
{
	public interface IFilterActions<T>
	{
        Task<IEnumerable<T>> AfterQueryAsync(IEnumerable<dynamic> rows);
    }
}