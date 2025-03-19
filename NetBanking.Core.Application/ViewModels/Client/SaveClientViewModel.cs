using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Client
{
    public class SaveClientViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
