using System;
using System.Threading.Tasks;
using Braavos.Core.Grabbers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Braavos.Function
{
    public class TimerEntryPoint
    {
        private readonly IDataGrabber _cnFileGrabber;

        public TimerEntryPoint(IDataGrabber cnFileGrabber) => _cnFileGrabber = cnFileGrabber;

        [FunctionName(nameof(CnAlliancesFileGrabber))]
        public async Task CnAlliancesFileGrabber(
            [TimerTrigger("0 0 6,18 * * *")] TimerInfo myTimer,
            [Blob("alliances", Connection = "AzureWebJobsStorage")] CloudBlobContainer outputContainer,
            ILogger log)
        {
            log.LogInformation($"{nameof(CnAlliancesFileGrabber)} function started execution at: {DateTime.Now}");

            await outputContainer.CreateIfNotExistsAsync();

            var cnResponse = await _cnFileGrabber.GetTodaysFileAsync(CnFileType.Alliances);

            var cloudBlockBlob = outputContainer.GetBlockBlobReference($"{cnResponse.FileName}.txt");
            await cloudBlockBlob.UploadFromStreamAsync(cnResponse.DataStream);

            log.LogInformation($"{nameof(CnAlliancesFileGrabber)} function completed execution at: {DateTime.Now}");
        }

        [FunctionName(nameof(CnNationsFileGrabber))]
        public async Task CnNationsFileGrabber(
            [TimerTrigger("0 5 6,18 * * *")] TimerInfo myTimer,
            [Blob("nations", Connection = "AzureWebJobsStorage")] CloudBlobContainer outputContainer,
            ILogger log)
        {
            log.LogInformation($"{nameof(CnNationsFileGrabber)} function started execution at: {DateTime.Now}");

            await outputContainer.CreateIfNotExistsAsync();

            var cnResponse = await _cnFileGrabber.GetTodaysFileAsync(CnFileType.Nations);

            var cloudBlockBlob = outputContainer.GetBlockBlobReference($"{cnResponse.FileName}.txt");
            await cloudBlockBlob.UploadFromStreamAsync(cnResponse.DataStream);

            log.LogInformation($"{nameof(CnNationsFileGrabber)} function completed execution at: {DateTime.Now}");
        }

        [FunctionName(nameof(CnAidFileGrabber))]
        public async Task CnAidFileGrabber(
            [TimerTrigger("0 10 6,18 * * *")] TimerInfo myTimer,
            [Blob("aid", Connection = "AzureWebJobsStorage")] CloudBlobContainer outputContainer,
            ILogger log)
        {
            log.LogInformation($"{nameof(CnAidFileGrabber)} function started execution at: {DateTime.Now}");

            await outputContainer.CreateIfNotExistsAsync();

            var cnResponse = await _cnFileGrabber.GetTodaysFileAsync(CnFileType.Aid);

            var cloudBlockBlob = outputContainer.GetBlockBlobReference($"{cnResponse.FileName}.txt");
            await cloudBlockBlob.UploadFromStreamAsync(cnResponse.DataStream);

            log.LogInformation($"{nameof(CnAidFileGrabber)} function completed execution at: {DateTime.Now}");
        }

        [FunctionName(nameof(CnWarFileGrabber))]
        public async Task CnWarFileGrabber(
            [TimerTrigger("0 15 6,18 * * *")] TimerInfo myTimer,
            [Blob("war", Connection = "AzureWebJobsStorage")] CloudBlobContainer outputContainer,
            ILogger log)
        {
            log.LogInformation($"{nameof(CnWarFileGrabber)} function started execution at: {DateTime.Now}");

            await outputContainer.CreateIfNotExistsAsync();

            var cnResponse = await _cnFileGrabber.GetTodaysFileAsync(CnFileType.War);

            var cloudBlockBlob = outputContainer.GetBlockBlobReference($"{cnResponse.FileName}.txt");
            await cloudBlockBlob.UploadFromStreamAsync(cnResponse.DataStream);

            log.LogInformation($"{nameof(CnWarFileGrabber)} function completed execution at: {DateTime.Now}");
        }
    }
}
