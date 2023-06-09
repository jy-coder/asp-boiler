using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class ProfileDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }


        public byte[] PasswordSalt { get; set; }

        public DateTime Created { get; set; }

        public string PhotoUrl { get; set; }

        public List<PhotoDto> Photos { get; set; }
    }
}