using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Domain.Entities;
public class Feedback: BaseAuditableEntity
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string Email { get; set; } 
    public string Phone { get; set; } 
    public string Name { get; set; }
    

}
