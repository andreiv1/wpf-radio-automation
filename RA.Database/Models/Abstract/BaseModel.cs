using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA.Database.Models.Abstract
{
    public abstract class BaseModel : IEquatable<BaseModel>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool Equals(BaseModel other)
        {
            if(other is null)
            {
                return false;
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseModel);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
