using System;
using System.Collections.Generic;

namespace M365.RoadMapInfo.Api
{
    public class ChangeSetDTO
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public IList<ChangeDTO> Changes { get; set; }
    }
}