using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using M365.RoadMapInfo.Authentication;
using M365.RoadMapInfo.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M365.RoadMapInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = Policies.CanImport)]
    public class ImportController : Controller
    {
        private readonly DataImporter _dataImporter;

        public ImportController( DataImporter dataImporter)
        {
            _dataImporter = dataImporter;
        }

        [HttpPost]
        public async Task<IActionResult> ImportUploadFile()
        {
            var form = await Request.ReadFormAsync();
            if (form.Files.Count <= 0) return BadRequest("no csv file specified");
            if (form.Files.Count > 1) return BadRequest("to many csv files specified");
            var formFile = form.Files[0];

            var fileInfo = ExtractFileInfo(formFile.Name);
            if (fileInfo == null) return BadRequest("invalid file name format");
            
            var content =  formFile.OpenReadStream();
            await _dataImporter.ImportAsync(fileInfo, content);
            return Ok();
        }
        
        
        
        
        private static UploadFileInfo ExtractFileInfo(string fileName)
        {
            var regex = new Regex(@".+_(?<Date>\d{1,2}-\d{1,2}-\d{4})\.csv");
            var match = regex.Match(fileName);
            if (!match.Success) return null;
            if (!match.Groups["Date"].Success) return null;
            var fileDateString = match.Groups["Date"].Value;
            if (!DateTime.TryParseExact(fileDateString, "MM-dd-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
                return null;
            return new UploadFileInfo
            {
                FileName = fileName,
                DownloadTime = date
            };
        }
        
        
    }
}