using System.Collections.Generic;

namespace Contraindicator.Models.Entities
{
    public class SubstanceList
    {
        public IEnumerable<SubstanceListItem> Substances { get; set; }
    }
}