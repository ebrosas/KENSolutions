using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Interfaces
{
    public interface ISocialConnect
    {
        string? LinkedInAccount { get; set; }
        string? FacebookAccount { get; set; }
        string? TwitterAccount { get; set; }
        string? InstagramAccount { get; set; }
    }
}
