using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }   
        public virtual bool IsActive { get; set; }
        protected Entity()
        {
        }
    }
}
