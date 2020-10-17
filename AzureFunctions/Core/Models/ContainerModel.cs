using System.Collections.Generic;
namespace AzureFunctions.Core.Models
{ class ContainerModel<T>
    { public int TotalItems
        {
         get => Items == null ? 0 : Items.Count;
        } 
       public IList<T> Items { get; set; }
    }
}
