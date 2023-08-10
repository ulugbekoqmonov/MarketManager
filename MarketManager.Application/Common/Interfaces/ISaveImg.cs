using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.Common.Interfaces
{
    public interface ISaveImg
    {
        string SaveImage(IFormFile newFile);
    }
}
