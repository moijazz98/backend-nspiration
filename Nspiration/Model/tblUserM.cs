using System.ComponentModel.DataAnnotations;

namespace Nspiration.Model
{
    public class tblUserM
    {
        [Key]
        public int iUserId { get; set; }
        public string sPhone { get; set; }
        public string sUserEmailId { get; set; }
        public string sUserType { get; set; }
        public string sPassword { get; set; }
        public char cStatus { get; set; }
    }
}
