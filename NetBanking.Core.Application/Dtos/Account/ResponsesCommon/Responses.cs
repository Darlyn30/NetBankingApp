using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Dtos.Account.ResponsesCommon
{
    public class Responses
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
