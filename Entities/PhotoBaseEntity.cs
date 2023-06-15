using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class PhotoBaseEntity
    {

        public DateTime Created { get; set; }

        public List<Photo> Photos { get; set; } = new();



    }
}