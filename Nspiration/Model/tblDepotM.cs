using System.ComponentModel.DataAnnotations;

namespace Nspiration.Model
{
    public class tblDepotM
    {
        [Key]
        public int DepotId { get; set; }
        public int OperationalTeamId { get; set; }
    }
}
