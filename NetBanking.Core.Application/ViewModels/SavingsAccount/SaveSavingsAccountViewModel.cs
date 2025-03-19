using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.SavingsAccount
{
    public class SaveSavingsAccountViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "You must specify a client.")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "You must enter a balance.")]
        public double Balance { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsMainAccount { get; set; }
    }
}
