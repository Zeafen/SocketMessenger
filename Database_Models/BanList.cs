using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessangerServer.Database_Models
{
    public class BanList
    {
        public BanList(User user, string ban_Reason, DateTime forgivnessDate)
        {
            User_ID = user.Id;
            Ban_Reason = ban_Reason;
            ForgivnessDate = forgivnessDate;
        }
        public BanList() { }
        public int Id { get; set; }


        public int User_ID { get; set; }
        [ForeignKey("User_ID")]
        User User { get; set; }

        [Column("Reason")]
        public string Ban_Reason {  get; set; }

        public DateTime ForgivnessDate { get; set; }
    }
}
