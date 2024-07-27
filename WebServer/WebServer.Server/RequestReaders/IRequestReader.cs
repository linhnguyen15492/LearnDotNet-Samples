using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server.RequestReaders
{
    public interface IRequestReader
    {
        Task<WRequest> ReadRequestAsync(CancellationToken cancellationToken);
    }
}
