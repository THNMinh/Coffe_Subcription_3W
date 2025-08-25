using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public abstract class Entity
    {
        [Column("is_delete")]
        public virtual bool IsDelete { get; set; }
        protected Entity()
        {
        }
    }
}
