using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Dtos.DTO
{
    public class ResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int OTP { get; set; }
        public string? Token { get; set; }
        public object Data { get; set; }
    }
}

