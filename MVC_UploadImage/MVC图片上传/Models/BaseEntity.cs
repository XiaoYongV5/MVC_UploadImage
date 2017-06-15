using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC图片上传.Models
{
    public abstract partial class BaseEntity
    {

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}