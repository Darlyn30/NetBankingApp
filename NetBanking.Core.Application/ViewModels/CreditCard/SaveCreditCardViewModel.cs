using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.CreditCard
{
    public class SaveCreditCardViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "You must specify a client.")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "You must enter a limit.")]
        public double Limit { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public double? Balance { get; set; }
        public double? Debit { get; set; }
    }
}
