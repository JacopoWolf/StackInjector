using System.Threading;
using System.Threading.Tasks;

namespace StackInjector
{
    /// <summary>
    /// Entry point for asyncronous Stacks.
    /// </summary>
    public interface IAsyncStackEntryPoint
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> Digest ( object item, CancellationToken cancellationToken );

    }
}
