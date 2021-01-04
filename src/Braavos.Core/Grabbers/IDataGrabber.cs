using System.IO;
using System.Threading.Tasks;

namespace Braavos.Core.Grabbers
{
    public interface IDataGrabber
    {
        Task<(string FileName, Stream DataStream)> GetTodaysFileAsync(CnFileType fileType);
    }
}
