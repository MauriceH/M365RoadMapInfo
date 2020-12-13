using System;
using System.Collections.Generic;

namespace M365.RoadMapInfo.Import
{
    public class ImportEntryData
    {
        public ImportEntryData(DateTime date, IEnumerable<RoadMapCsvRow> rows)
        {
            Date = date;
            Rows = rows;
        }

        public DateTime Date { get; }
        public IEnumerable<RoadMapCsvRow> Rows { get; }
    }
}