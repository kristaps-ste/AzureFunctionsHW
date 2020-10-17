using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos.Table;

namespace StorageService.Models
{
    public class LogEntity : TableEntity
    {
      public bool Success { get; set; }
       
    }
}
