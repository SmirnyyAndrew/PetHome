using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHome.Domain.VolunteerEntity
{
    public class SocialNetwork
    {
        // Для EF core
        private SocialNetwork() { }


        public int Id {  get; private set; }
        public string Url { get; private set; }
    }
}
