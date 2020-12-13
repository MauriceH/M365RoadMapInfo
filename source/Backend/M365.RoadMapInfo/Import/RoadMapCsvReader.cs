using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace M365.RoadMapInfo.Import
{
    public class RoadMapCsvReader : IDisposable
    {
        private CsvReader _reader;
        
        public RoadMapCsvReader(TextReader textReader, bool leaveOpen = false)
        {
            var csvCulture = CultureInfo.GetCultureInfo("en-US");
            var config = new CsvConfiguration(csvCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
            };
            _reader = new CsvReader(textReader, config, leaveOpen);
        }

        public IAsyncEnumerable<RoadMapCsvRow> ReadAllRowsAsync()
        {
            return _reader.GetRecordsAsync<RoadMapCsvRow>();
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}